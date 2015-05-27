﻿namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Model
{
	using System.Collections.Generic;
	using System.Reflection;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.Logic;

	/// <summary>
	/// モデルの基盤。
	/// <para>データ保持を生きがいにする。</para>
	/// </summary>
	public abstract class ModelBase: IGetMembers
	{
		#region variable

		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		#region override

		public override string ToString()
		{
			return ReflectionUtility.GetObjectString(this);
		}

		#endregion

		#region IGetMembers
		
		public IEnumerable<PropertyInfo> PropertyInfos
		{
			get
			{
				if(this._propertyInfos == null) {
					this._propertyInfos = GetType().GetProperties();
				}

				return this._propertyInfos;
			}
		}

		public IEnumerable<string> GetNameValueList()
		{
			var nameValueMap = ReflectionUtility.GetMembers(this, PropertyInfos);
			return ReflectionUtility.GetNameValueStrings(nameValueMap);
		}

		#endregion

		#region function
		#endregion
	}
}
