﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// 最大サイズ制限リスト。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class FixedSizedList<T>: List<T>, ILimitSize
	{
		const int defaultLimit = 64;

		private int _limitSize;

		public event EventHandler ListChanged;

		public FixedSizedList()
			: base()
		{
			LimitSize = defaultLimit;
		}

		public FixedSizedList(IEnumerable<T> collection)
			: base(collection)
		{
			LimitSize = defaultLimit;
		}

		public FixedSizedList(int limitSize)
			: base()
		{
			LimitSize = limitSize;
		}

		public FixedSizedList(int capacity, int limitSize)
			: base(capacity)
		{
			LimitSize = limitSize;
		}

		public int LimitSize 
		{ 
			get { return this._limitSize; } 
			set
			{
				if(this._limitSize > value && Count > value) {
					RemoveRange(value, Count - value);
				}
				this._limitSize = value;
			}
		}

		protected void CallListChangedEvent()
		{
			if(ListChanged != null) {
				ListChanged(this, new EventArgs());
			}
		}

		public new void Add(T item)
		{
			if(Count >= LimitSize) {
				RemoveAt(0);
			} 

			base.Add(item);

			CallListChangedEvent();
		}

		public new void AddRange(IEnumerable<T> collection)
		{
			var collectionCount = collection.Count();
			if(collectionCount >= LimitSize) {
				Clear();
				base.AddRange(collection.Skip(collectionCount - LimitSize));
			} else {
				// TODO: ちょっと後回し
				base.AddRange(collection);
			}

			CallListChangedEvent();
		}

		public new void Insert(int index, T item)
		{
			if(Count >= LimitSize) {
				RemoveAt(Count - 1);
			}
			base.Insert(index, item);

			CallListChangedEvent();
		}

		public new void Clear()
		{
			var hasData = Count > 0;
			if(hasData) {
				base.Clear();
				CallListChangedEvent();
			}
		}

		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
			CallListChangedEvent();
		}

	}
}