﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.DB
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.DB;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// Description of Note.
	/// </summary>
	public class NoteDB: DBWrapper
	{
		public NoteDB(AppDBManager db): base(db)
		{ }
		
		/// <summary>
		/// TODO: Dataから別のどこかへ委譲。
		/// </summary>
		/// <param name="enabledOnly">有効アイテムのみ取得するか</param>
		/// <returns></returns>
		public IEnumerable<NoteItem> GetNoteItemList(bool enabledOnly)
		{
			using(var query = this.db.CreateQuery()) {
				//var dtoList = this.db.GetResultList<NoteItemDto>(global::PeMain.Properties.SQL.GetNoteItemList);
				var dtoList = query.GetResultList<NoteItemDto>(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_GetNoteItemList);
				if(enabledOnly) {
					dtoList = dtoList.Where(dto => dto.CommonEnabled);
				}
				var count = dtoList.Count();
				if(count > 0) {
					var result = new List<NoteItem>(count);
					foreach(var dto in dtoList) {
						var noteItem = new NoteItem();

						noteItem.NoteId = dto.Id;

						noteItem.Title = dto.Title;
						noteItem.Body = dto.Body;
						noteItem.NoteType = NoteTypeUtility.ToNoteType(dto.RawType);

						noteItem.Visible = dto.Visibled;
						noteItem.Compact = dto.Compact;
						noteItem.Topmost = dto.Topmost;
						noteItem.Locked = dto.Locked;

						noteItem.Location = new Point(dto.X, dto.Y);
						noteItem.Size = new Size(dto.Width, dto.Height);

						noteItem.Style.ForeColor = dto.ForeColor;
						noteItem.Style.BackColor = dto.BackColor;
						if(!string.IsNullOrWhiteSpace(dto.FontFamily) && dto.FontHeight > 0) {
							noteItem.Style.FontSetting.Family = dto.FontFamily;
							noteItem.Style.FontSetting.Height = dto.FontHeight;
							noteItem.Style.FontSetting.Bold = dto.FontBold;
							noteItem.Style.FontSetting.Italic = dto.FontItalic;
						}

						result.Add(noteItem);
					}

					return result;
				} else {
					return new NoteItem[] { };
				}
			}
		}
		
		
		public void ToDisabled(IEnumerable<NoteItem> noteItemList)
		{
			if(noteItemList == null || !noteItemList.Any()) {
				return;
			}

			using(var query = this.db.CreateQuery()) {
				query.Parameter["enabled"] = false;
				query.Parameter["update"] = DateTime.Now;
				var idList = new List<string>(noteItemList.Count());
				var index = 1;
				foreach(var note in noteItemList) {
					var key = string.Format("id_{0}", index++);
					var item = string.Format("NOTE_ID = :{0}", key);
					query.Parameter[key] = note.NoteId;
					idList.Add(item);
				}
				query.SetExpression("ID_LIST", string.Join(" or ", idList));
				query.ExecuteCommand(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_EnabledSwitch);
			}
		}
		
		public void ResistMasterNote(IEnumerable<NoteItem> noteItemList, DateTime timestamp)
		{
			if(noteItemList == null || !noteItemList.Any()) {
				return;
			}
			
			var updateList = new List<MNoteRow>();
			var insertList = new List<MNoteRow>();

			foreach(var item in noteItemList) {
				using(var query = this.db.CreateQuery()) {
					var entity = new MNoteRow();
					entity.Id = item.NoteId;
					var tempEntity = query.GetRow(entity);
					var isUpdate = tempEntity != null;
					if(isUpdate) {
						entity = tempEntity;
					} else {
						entity.CommonCreate = timestamp;
						entity.CommonEnabled = true;
					}
					entity.CommonUpdate = timestamp;

					entity.Title = item.Title;
					entity.RawType = NoteType.Text.ToNumber();
					entity.Title = item.Title;

					if(isUpdate) {
						updateList.Add(entity);
					} else {
						insertList.Add(entity);
					}
				}
			}
			if(updateList.Count > 0) {
				using(var query = this.db.CreateQuery()) {
					query.ExecuteUpdate(updateList);
				}
			}
			if(insertList.Count > 0) {
				using(var query = this.db.CreateQuery()) {
					query.ExecuteInsert(insertList);
				}
			}
		}

		public void ResistTransactionNote(IEnumerable<NoteItem> noteItemList, DateTime timestamp)
		{
			if(noteItemList == null || !noteItemList.Any()) {
				return;
			}
			
			var updateList = new List<TNoteRow>();
			var insertList = new List<TNoteRow>();
			
			foreach(var item in noteItemList) {
				using(var query = this.db.CreateQuery()) {
					var entity = new TNoteRow();
					entity.Id = item.NoteId;
					var tempEntity = query.GetRow(entity);
					var isUpdate = tempEntity != null;
					if(isUpdate) {
						entity = tempEntity;
					} else {
						entity.CommonCreate = timestamp;
					}
					entity.CommonUpdate = timestamp;
					entity.Body = item.Body;

					if(isUpdate) {
						updateList.Add(entity);
					} else {
						insertList.Add(entity);
					}
				}
			}
			
			if(updateList.Count > 0) {
				using(var query = this.db.CreateQuery()) {
					query.ExecuteUpdate(updateList);
				}
			}
			if(insertList.Count > 0) {
				using(var query = this.db.CreateQuery()) {
					query.ExecuteInsert(insertList);
				}
			}
		}
		
		public void ResistTransactionNoteStyle(IEnumerable<NoteItem> noteItemList, DateTime timestamp)
		{
			if(noteItemList == null || !noteItemList.Any()) {
				return;
			}
			
			var updateList = new List<TNoteStyleRow>();
			var insertList = new List<TNoteStyleRow>();
			
			foreach(var item in noteItemList) {
				using(var query = this.db.CreateQuery()) {
					var entity = new TNoteStyleRow();
					entity.Id = item.NoteId;
					var tempEntity = query.GetRow(entity);
					var isUpdate = tempEntity != null;
					if(isUpdate) {
						entity = tempEntity;
					} else {
						entity.CommonCreate = timestamp;
					}
					entity.CommonUpdate = timestamp;

					entity.ForeColor = item.Style.ForeColor;
					entity.BackColor = item.Style.BackColor;
					if(item.Style.FontSetting.IsDefault) {
						entity.FontFamily = string.Empty;
					} else {
						entity.FontFamily = item.Style.FontSetting.Family;
					}
					entity.FontHeight = item.Style.FontSetting.Height;
					entity.FontBold = item.Style.FontSetting.Bold;
					entity.FontItalic = item.Style.FontSetting.Italic;
					entity.Visibled = item.Visible;
					entity.Locked = item.Locked;
					entity.Topmost = item.Topmost;
					entity.Compact = item.Compact;
					entity.Location = item.Location;
					entity.Size = item.Size;

					if(isUpdate) {
						updateList.Add(entity);
					} else {
						insertList.Add(entity);
					}
				}
			}
			
			if(updateList.Count > 0) {
				using(var query = this.db.CreateQuery()) {
					query.ExecuteUpdate(updateList);
				}
			}
			if(insertList.Count > 0) {
				using(var query = this.db.CreateQuery()) {
					query.ExecuteInsert(insertList);
				}
			}
		}
		
		public void Resist(IEnumerable<NoteItem> noteItemList)
		{
			var timestamp = DateTime.Now;
			ResistMasterNote(noteItemList, timestamp);
			ResistTransactionNote(noteItemList, timestamp);
			ResistTransactionNoteStyle(noteItemList, timestamp);
		}
		
		public NoteItem InsertMaster(NoteItem noteItem)
		{
			lock(this.db) {
				using(var tran = this.db.BeginTransaction()) {
					var noteDto = this.db.GetTableId("M_NOTE", "NOTE_ID");
					noteItem.NoteId = noteDto.MaxId + 1;
					ResistMasterNote(new [] { noteItem }, DateTime.Now);
					tran.Commit();
					return noteItem;
				}
			}
		}
		
	}
}
