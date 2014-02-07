﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	public partial class StreamForm
	{
		public void SetParameter(Process process, LauncherItem launcherItem)
		{
			Process = process;
			LauncherItem = launcherItem;
			
			Process.EnableRaisingEvents = true;
			Process.Exited += new EventHandler(Process_Exited);
		}

		public void SetSettingData(Language language, MainSetting mainSetting, ISkin skin)
		{
			Language = language;
			MainSetting = mainSetting;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(Process != null);
			
			ApplyLanguage();
			
			this.propertyProcess.SelectedObject = Process;
			this.propertyProperty.SelectedObject = Process.StartInfo;

			Process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
			Process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
		}
		
		void OutputStreamReceived(string line, bool stdOutput)
		{
			this.viewOutput.BeginInvoke(
				(MethodInvoker)delegate() {
					this.viewOutput.Text += line + Environment.NewLine;
				}
			);
		}
		
		void RefreshProperty()
		{
			// TODO: ???
			this.propertyProcess.SelectedObject = null;
			this.propertyProcess.SelectedObject = Process;
		}
		
		void ExitedProcess()
		{
			this.toolStream_kill.Enabled = false;
			this.toolStream_clear.Enabled = false;
			this.toolStream_refresh.Enabled = false;
			RefreshProperty();
			
			Text += String.Format(": {0}", Process.ExitCode);
		}
		
		void KillProcess()
		{
			if(Process.HasExited) {
				return;
			}
			try {
				Process.Kill();
			} catch(Exception ex) {
				Logger.Puts(LogType.Error, ex.Message, ex);
			}
		}
	}
}
