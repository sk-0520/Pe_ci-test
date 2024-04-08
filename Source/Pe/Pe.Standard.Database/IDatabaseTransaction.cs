﻿using System;
using System.Data;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    /// <summary>
    /// データベース実装におけるトランザクション処理。
    /// <para>これが実体化されてればトランザクション中でしょうね。</para>
    /// </summary>
    public interface IDatabaseTransaction: IDatabaseContext, IDatabaseContexts, IDisposable
    {
        #region property

        /// <summary>
        /// CRL上のトランザクション実体。
        /// <para>トランザクションを開始しない場合 <see langword="null" /> となり、扱いは <see cref="IDatabaseTransaction"/> 実装側依存となる。</para>
        /// </summary>
        IDbTransaction? Transaction { get; }

        #endregion

        #region function

        /// <summary>
        /// コミット！
        /// </summary>
        void Commit();

        /// <summary>
        /// なかったことにしたい人生の一部。
        /// </summary>
        void Rollback();

        #endregion
    }
}
