﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Data;
using PeMain.Logic;
using PeMain.Logic.DB;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_functions.
	/// </summary>
	public partial class Pe
	{
		#if DEBUG
		public void DebugProcess()
		{
			/*
			var db = this._commonData.Database;
			
			using(var tran = db.BeginTransaction()) {
				var entity = new PeMain.Data.DB.MNoteEntity();
				db.ExecuteDelete(new [] { entity } );
				db.ExecuteInsert(new [] { entity } );
				tran.Commit();
			}
			
			using(var reader = db.ExecuteReader("select * from M_NOTE")) {
				while(reader.Read()) {
					for(var i=0; i < reader.FieldCount;i ++) {
						var name = reader.GetName(i);
						var value= reader[name];
						Debug.WriteLine("{0} = {1}", name, value);
					}
				}
			}
			using(var tran = db.BeginTransaction()) {
				var entity = new PeMain.Data.DB.MNoteEntity();
				entity.Title = "a'b--c";
				db.ExecuteUpdate(new [] { entity } );
				tran.Commit();
			}
			using(var tran = db.BeginTransaction()) {
				var entity = new PeMain.Data.DB.MNoteEntity();
				entity.Id = 1;
				db.ExecuteInsert(new [] { entity } );
				tran.Rollback();
			}
			using(var reader = db.ExecuteReader("select * from M_NOTE")) {
				while(reader.Read()) {
					for(var i=0; i < reader.FieldCount;i ++) {
						var name = reader.GetName(i);
						var value= reader[name];
						Debug.WriteLine("{0} = {1}", name, value);
					}
				}
			}
			var e2 = new PeMain.Data.DB.MNoteEntity();
			var e3 = db.GetEntity(e2);
			Debug.WriteLine(e2.Title);
			Debug.WriteLine(e3.Title);
			//*/
			
			/*
			var note = new NoteForm();
			note.SetCommonData(this._commonData);
			note.Show();
			//*/
			
			/*
			var info = new PeInformation();
			foreach(var g in info.Get()) {
				Debug.WriteLine(string.Format("[ {0} ]============", g.Title));
				foreach(var item in g.Items) {
					Debug.WriteLine(string.Format("{0} = {1}", item.Key, item.Value));
				}
			}
			//*/
		}
		#endif
		
		IEnumerable<Form> GetWindows()
		{
			var result = new List<Form>();
			result.AddRange(this._toolbarForms.Values);
			result.Add(this._logForm);
			
			foreach(var f in this._toolbarForms.Values.Where(f => f.OwnedForms.Length > 0)) {
				result.AddRange(f.OwnedForms);
			}
			
			result.AddRange(this._noteWindowList);

			return result;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="func">ウィンドウ再構築を独自に行う場合は処理を返す。</param>
		void PauseOthers(Func<Action> func)
		{
			var windowVisible = new Dictionary<Form, bool>();
			foreach(var window in GetWindows()) {
				windowVisible[window] = window.Visible;
				window.Visible = false;
			}
			this._notifyIcon.Visible = false;
			
			this._pause = true;
			var action = func();
			this._pause = false;

			if(action != null) {
				action();
			} else {
				foreach(var pair in windowVisible) {
					pair.Key.Visible = pair.Value;
				}
			}
			
			this._notifyIcon.Visible = true;
		}
		
		void BackupSetting(IEnumerable<string> targetFiles, string saveDirPath, int count)
		{
			var enabledFiles = targetFiles.Where(s => File.Exists(s));
			if(enabledFiles.Count() == 0) {
				return;
			}
			
			// バックアップ世代交代
			if(Directory.Exists(saveDirPath)) {
				foreach(var path in Directory.GetFileSystemEntries(saveDirPath).OrderByDescending(s => Path.GetFileName(s)).Skip(count - 1)) {
					try {
						File.Delete(path);
					} catch(Exception ex) {
						this._commonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
			
			var fileName = Literal.NowTimestampFileName + ".zip";
			var saveFilePath = Path.Combine(saveDirPath, fileName);
			FileUtility.MakeFileParentDirectory(saveFilePath);
			
			// zip
			using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
				foreach(var filePath in enabledFiles) {
					var entry = zip.CreateEntry(Path.GetFileName(filePath));
					using(var entryStream = new BinaryWriter(entry.Open())) {
						var buffer = FileUtility.ToBinary(filePath);
						/*
						using(var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
							var buffer = new byte[Literal.fileTempBufferLength];
							int readLength;
							while((readLength = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
								entryStream.Write(buffer, 0, readLength);
							}
						}
						 */
						entryStream.Write(buffer);
					}
				}
			}

		}
		
		void SaveSetting()
		{
			// バックアップ
			var backupFiles = new [] {
				Literal.UserMainSettingPath,
				Literal.UserLauncherItemsPath,
				Literal.UserDBPath,
			};
			BackupSetting(backupFiles, Literal.UserBackupDirPath, Literal.backupCount);
			
			// 保存開始
			// メインデータ
			SaveSerialize(this._commonData.MainSetting, Literal.UserMainSettingPath);
			//ランチャーデータ
			var sortedSet = new HashSet<LauncherItem>();
			foreach(var item in this._commonData.MainSetting.Launcher.Items.OrderBy(item => item.Name)) {
				sortedSet.Add(item);
			}
			SaveSerialize(sortedSet, Literal.UserLauncherItemsPath);
		}
		
		
		static T LoadDeserialize<T>(string path, bool failToNew)
			where T: new()
		{
			if(File.Exists(path)) {
				var serializer = new XmlSerializer(typeof(T));
				using(var stream = new FileStream(path, FileMode.Open)) {
					return (T)serializer.Deserialize(stream);
				}
			}
			if(failToNew) {
				return new T();
			} else {
				return default(T);
			}
		}

		static void SaveSerialize<T>(T saveData, string savePath)
		{
			Debug.Assert(saveData != null);
			FileUtility.MakeFileParentDirectory(savePath);

			using(var stream = new FileStream(savePath, FileMode.Create)) {
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stream, saveData);
			}
			
		}
		
		public void CloseApplication(bool save)
		{
			if(save) {
				SaveSetting();
				if(this._commonData.Database != null) {
					this._commonData.Database.Close();
				}
			}
			Application.Exit();
		}
		
		Action OpenSetting()
		{
			using(var settingForm = new SettingForm(this._commonData.Language, this._commonData.MainSetting, this._commonData.Database)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					foreach(var note in this._noteWindowList) {
						note.Close();
						note.Dispose();
					}
					this._noteWindowList.Clear();
					
					var mainSetting = settingForm.MainSetting;
					this._commonData.MainSetting = mainSetting;
					settingForm.SaveDB(this._commonData.Database);
					SaveSetting();
					InitializeLanguage(null, null);
					ApplyLanguage();
					
					return delegate() {
						this._logForm.SetCommonData(this._commonData);
						this._messageWindow.SetCommonData(this._commonData);
						foreach(var toolbar in this._toolbarForms.Values) {
							//toolbar.SetCommonData(this._commonData);
							toolbar.Dispose();
						}
						this._toolbarForms.Clear();
						InitializeToolbarForm(null, null);
						
						InitializeNoteForm(null, null);
					};
				}
			}
			return null;
		}
		
		void ChangeShowSysEnv(Func<bool> nowValueDg, Action<bool> changeValueDg, string messageTitleName, string showMessageName, string hiddenMessageName, string errorMessageName)
		{
			var prevValue = nowValueDg();
			changeValueDg(!prevValue);
			var nowValue = nowValueDg();
			SystemEnv.RefreshShell();
			
			ToolTipIcon icon;
			string messageName;
			if(prevValue != nowValue) {
				if(nowValue) {
					messageName = showMessageName;
				} else {
					messageName = hiddenMessageName;
				}
				icon = ToolTipIcon.Info;
			} else {
				messageName = errorMessageName;
				icon = ToolTipIcon.Error;
			}
			var title = this._commonData.Language[messageTitleName];
			var message = this._commonData.Language[messageName];
			if(icon == ToolTipIcon.Error) {
				this._commonData.Logger.Puts(LogType.Error, title, message);
			}
			ShowBalloon(icon, title, message);
			
		}
		
		public void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key)
		{
			if(this._pause) {
				return;
			}
			
			switch(hotKeyId) {
				case HotKeyId.HiddenFile:
					ChangeShowSysEnv(SystemEnv.IsHiddenFileShow, SystemEnv.SetHiddenFileShow, "balloon/hidden-file/title", "balloon/hidden-file/show", "balloon/hidden-file/hide", "balloon/hidden-file/error");
					break;
					
				case HotKeyId.Extension:
					ChangeShowSysEnv(SystemEnv.IsExtensionShow, SystemEnv.SetExtensionShow, "balloon/extension/title", "balloon/extension/show", "balloon/extension/hide", "balloon/extension/error");
					break;
					
				case HotKeyId.CreateNote:
					CreateNote(Point.Empty);
					break;
				case HotKeyId.HiddenNote:
					HiddenNote();
					break;
				case HotKeyId.CompactNote:
					CompactNote();
					break;
					
				default:
					break;
			}
		}

		void CreateNote(Point point)
		{
			// アイテムをデータ設定
			var item = new NoteItem();
			item.Title = DateTime.Now.ToString();
			if(point.IsEmpty) {
				item.Location = Cursor.Position;
			} else {
				item.Location = point;
			}
			var noteDB = new NoteDB(this._commonData.Database);
			noteDB.InsertMaster(item);
			
			var noteForm = CreateNote(item);
			noteForm.Activate();
		}
		
		Form CreateNote(NoteItem noteItem)
		{
			var noteForm = new NoteForm();
			noteForm.NoteItem = noteItem;
			noteForm.SetCommonData(this._commonData);
			noteForm.Show();
			noteForm.Closed += delegate(object sender, EventArgs e) {
				if(noteForm.Visible) {
					this._noteWindowList.Remove(noteForm);
				}
			};
			this._noteWindowList.Add(noteForm);
			return noteForm;
		}
		
		void HiddenNote()
		{
			foreach(var note in this._noteWindowList.ToArray()) {
				note.ToClose(false);
			}
		}
		
		void CompactNote()
		{
			foreach(var note in this._noteWindowList.Where(note => !note.NoteItem.Compact)) {
				note.ToCompact();
			}
		}
	}
}