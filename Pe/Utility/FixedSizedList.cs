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

		public int LimitSize { get; set; }

		public new void Add(T item)
		{
			if(Count >= LimitSize) {
				RemoveAt(0);
			} 

			base.Add(item);
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
		}
	}
}
