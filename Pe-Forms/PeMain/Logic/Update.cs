﻿namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using Microsoft.Win32.SafeHandles;
	using ObjectDumper;

	public class UpdateInfo
	{
		IEnumerable<string> _log;
		
		public UpdateInfo(IEnumerable<string> log)
		{
			this._log = log;
		}
		
		public string Version { get; set; }
		public bool IsUpdate { get; set; }
		public bool IsRcVersion { get; set; }
		public bool IsError { get; set; }
		public int ErrorCode { get; set; }
		
		public string Log
		{
			get
			{
				return string.Join(Environment.NewLine, this._log);
			}
		}
	}
	
	public class UpdateData
	{
		readonly string _downloadPath;
		readonly bool _donwloadRc;
		readonly CommonData _commonData;
		
		public UpdateInfo Info { get; private set; }
		
		public static string UpdaterExe
		{
			get { return Path.Combine(Literal.ApplicationSBinDirPath, Literal.updateProgramDirectoryName, Literal.updateProgramName); }
		}
		
		public UpdateData(string downloadPath, bool donwloadRc, CommonData commonData)
		{
			this._downloadPath = downloadPath;
			this._donwloadRc = donwloadRc;
			this._commonData = commonData;
		}
		
		Process CreateProcess(Dictionary<string,string> map)
		{
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = UpdaterExe;
			
			var defaultMap = new Dictionary<string,string>() {
				{ "pid",      string.Format("{0}", Process.GetCurrentProcess().Id) },
				{ "version",  Literal.Version.FileVersion },
				{ "uri",      Literal.UpdateURL },
				{ "platform", Environment.Is64BitProcess ? "x64": "x86" },
				{ "rc",       this._donwloadRc ? "true": "false" },
			};
			
			foreach(var pair in map) {
				defaultMap[pair.Key] = pair.Value;
			}
			startInfo.Arguments = string.Join(" ", defaultMap.Select(p => string.Format("\"/{0}={1}\"", p.Key, p.Value)));

			return process;
		}
		
		public UpdateInfo Check()
		{
			var lines = new List<string>();
			var map = new Dictionary<string,string>() {
				{ "checkonly", "true" }
			};
			using(var process = CreateProcess(map)) {

				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/check"], process.StartInfo.Arguments);

				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;

				process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
					lock(lines) {
						if(e.Data != null) {
							lines.Add(e.Data);
						}
					}
				};
				process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
					lock(lines) {
						if(e.Data != null) {
							lines.Add(e.Data);
						}
					}
				};

				process.Start();

				process.BeginOutputReadLine();
				process.BeginErrorReadLine();

				process.WaitForExit();
			}

			var info = new UpdateInfo(lines);
			
			if(lines.Count > 0) {
				var s = lines.SingleOrDefault(line => !string.IsNullOrEmpty(line) && line.StartsWith(">> ", StringComparison.OrdinalIgnoreCase));
				
				var v = new string(s.SkipWhile(c => c != ':').Skip(1).ToArray());
				if(s.StartsWith(">> UPDATE", StringComparison.OrdinalIgnoreCase)) {
					var version = new string(v.TakeWhile(c => c != ' ').ToArray());
					var isRc = v.Substring(version.Length + 1) == "RC";
					info.IsUpdate = true;
					info.Version = version;
					info.IsRcVersion = isRc;
				} else if(string.IsNullOrEmpty(s) || !s.StartsWith(">> NONE", StringComparison.OrdinalIgnoreCase)) {
					int r;
					if(int.TryParse(v, out r)) {
						info.ErrorCode = r;
					} else {
						info.ErrorCode = -2;
					}
					info.IsError = true;
				}
			} else {
				info.ErrorCode = -1;
				info.IsError = true;
			}

			return Info = info;
		}
		
		/// <summary>
		/// 更新処理実行。
		/// </summary>
		/// <returns></returns>
		public bool Execute()
		{
			var eventName = "pe-event";

			var lines = new List<string>();
			var map = new Dictionary<string,string>() {
				{ "download",       this._downloadPath },
				{ "expand",         Literal.ApplicationRootDirectoryPath },
				{ "wait",           "true" },
				{ "no-wait-update", "true" },
				{ "event",           eventName },
				{ "script",          Path.Combine(Literal.ApplicationScriptDirPath, "Updater", "UpdaterScript.cs") },
			};
			FileUtility.MakeFileParentDirectory(this._downloadPath);
			if(!Directory.Exists(this._downloadPath)) {
				Directory.CreateDirectory(this._downloadPath);
			}
			// #158
			AppUtility.RotateFile(this._downloadPath, "*.zip", Literal.updateArchiveCount, this._commonData.Logger);

			//var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In);
			var waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);

			using(var process = CreateProcess(map)) {
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/exec"], process.StartInfo.Arguments);

				var result = false;

				process.Start();
				var processEvent = new EventWaitHandle(false, EventResetMode.AutoReset) {
					SafeWaitHandle = new SafeWaitHandle(process.Handle, false),
				};
				var handles = new[] { waitEvent, processEvent };
				var waitResult = WaitHandle.WaitAny(handles, TimeSpan.FromMinutes(3));
				this._commonData.Logger.PutsDebug("WaitHandle.WaitAny", () => waitResult);
				if(0 <= waitResult && waitResult < handles.Length) {
					if(handles[waitResult] == waitEvent) {
						// イベントが立てられたので終了
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/exit"], process.StartInfo.Arguments);
						result = true;
					} else if(handles[waitResult] == processEvent) {
						// Updaterがイベント立てる前に死んだ
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/error-process"], process.ExitCode);
					}
				} else {
					// タイムアウト
					if(!process.HasExited) {
						// まだ生きてるなら強制的に殺す
						process.Kill();
					}
					this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/error-timeout"], process.ExitCode);
				}

				return result;
			}
		}


	}
}
