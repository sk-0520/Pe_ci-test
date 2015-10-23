﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using Utility;

    /// <summary>
    /// なんかをとりあえず通知する。
    /// </summary>
    public class NotifyPropertyChangedObject: DisposeFinalizeBase, INotifyPropertyChanged
    {
        #region function

        /// <summary>
        /// 変数変更用ヘルパ。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">格納する変数。</param>
        /// <param name="value">変更データ。</param>
        /// <param name="propertyName"></param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected bool SetVariableValue<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
        {
            if(!IComparable<T>.Equals(variable, value)) {
                variable = value;
                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// プロパティ変更用ヘルパ。
        /// TODO: キャッシュする。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">変更データ。</param>
        /// <param name="targetMemberName">対象オブジェクトのメンバ名。</param>
        /// <param name="propertyName"></param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected bool SetPropertyValue<T>(object obj, T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string callerPropertyName = "")
        {
            CheckUtility.DebugEnforceNotNull(obj);

            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName);

            var nowValue = (T)propertyInfo.GetValue(obj);

            if(!IComparable<T>.Equals(nowValue, value)) {
                propertyInfo.SetValue(obj, value);
                OnPropertyChanged(callerPropertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 指定メンバが変更された通知する。
        /// </summary>
        /// <param name="propertyName">メンバ1。</param>
        /// <param name="propertyNames">メンバn。</param>
        protected void CallOnPropertyChange(string propertyName, params string[] propertyNames)
        {
            if(propertyNames == null || propertyNames.Length == 0) {
                OnPropertyChanged(propertyName);
            } else {
                var paramList = new List<string>(1 + propertyNames.Length);
                paramList.Add(propertyName);
                paramList.AddRange(propertyNames);
                CallOnPropertyChange(paramList);
            }
        }

        protected void CallOnPropertyChange(IEnumerable<string> propertyNames)
        {
            foreach(var propertyName in propertyNames) {
                OnPropertyChanged(propertyName);
            }
        }

        protected void CallOnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null) {
                PropertyChanged(sender, e);
            }
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
            }

            base.Dispose(disposing);
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// プロパティが変更された際に発生。
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChanged呼び出し。
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            CallOnPropertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
