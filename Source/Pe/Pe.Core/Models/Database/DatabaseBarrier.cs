using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    internal sealed class DatabaseBarrierTransaction: DisposerBase, IDatabaseTransaction
    {
        public DatabaseBarrierTransaction(IDisposable locker, IDatabaseTransaction transaction, IDatabaseImplementation implementation)
        {
            Locker = locker;
            Transaction = transaction;
            Implementation = implementation;
        }

        #region property

        private IDisposable Locker { get; [Unuse(UnuseKinds.Dispose)]set; }
        public IDatabaseTransaction Transaction { get; [Unuse(UnuseKinds.Dispose)] set; }

        IDbTransaction IDatabaseTransaction.Transaction => Transaction.Transaction;

        public IDatabaseImplementation Implementation { get; }

        #endregion

        #region function
        #endregion

        #region IDatabaseTransaction

        public IDatabaseContext Context => this;

        public int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.Execute(statement, parameter);
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.GetDataTable(statement, parameter);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Transaction.Query<T>(statement, parameter, buffered);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Transaction.Query(statement, parameter, buffered);
        }

        public T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirst<T>(statement, parameter);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirstOrDefault<T>(statement, parameter);
        }

        public T QuerySingle<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QuerySingle<T>(statement, parameter);
        }

        [return: MaybeNull]
        public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QuerySingleOrDefault<T>(statement, parameter);
        }

        public void Commit()
        {
            ThrowIfDisposed();

            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();

            ThrowIfDisposed();
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Transaction.Dispose();
                    Locker.Dispose();
                }
                Transaction = null!;
                Locker = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// ???????????????????????????????????????????????????
    /// <para>NOTE: ??????????????????SQLite????????????????????????</para>
    /// </summary>
    public interface IDatabaseBarrier
    {
        #region function

        /// <summary>
        /// ????????????????????????????????????????????????????????????
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction WaitWrite();
        /// <summary>
        /// ????????????????????????????????????????????????????????????
        /// </summary>
        /// <param name="timeout">???????????????</param>
        /// <returns></returns>
        IDatabaseTransaction WaitWrite(TimeSpan timeout);

        /// <summary>
        /// ????????????????????????????????????????????????????????????
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction WaitRead();
        /// <summary>
        /// ????????????????????????????????????????????????????????????
        /// </summary>
        /// <param name="timeout">???????????????</param>
        /// <returns></returns>
        IDatabaseTransaction WaitRead(TimeSpan timeout);


        #endregion
    }

    public static class IDatabaseBarrierExtensions
    {
        #region function

        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="databaseBarrier"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult ReadData<TResult>(this IDatabaseBarrier databaseBarrier, Func<IDatabaseTransaction, TResult> func)
        {
            using var transaction = databaseBarrier.WaitRead();
            return func(transaction);
        }

        /// <summary>
        /// ????????????????????????
        /// <para>????????????????????????</para>
        /// </summary>
        /// <typeparam name="TArgument"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="databaseBarrier"></param>
        /// <param name="func"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static TResult ReadData<TArgument, TResult>(this IDatabaseBarrier databaseBarrier, Func<IDatabaseTransaction, TArgument, TResult> func, TArgument argument)
        {
            using var transaction = databaseBarrier.WaitRead();
            return func(transaction, argument);
        }

        #endregion
    }

    /// <inheritdoc cref="IDatabaseBarrier" />
    public class DatabaseBarrier: IDatabaseBarrier
    {
        public DatabaseBarrier(IDatabaseAccessor accessor, ReaderWriterLocker locker)
        {
            Accessor = accessor;
            Locker = locker;
        }

        #region property

        protected IDatabaseAccessor Accessor { get; }
        protected ReaderWriterLocker Locker { get; }

        #endregion

        #region IDatabaseBarrier

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite" />
        /// <para><see cref="ReaderWriterLocker.WaitWriteByDefaultTimeout()"/>??????????????????</para>
        /// </summary>
        public virtual IDatabaseTransaction WaitWrite()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite(TimeSpan)" />
        public virtual IDatabaseTransaction WaitWrite(TimeSpan timeout)
        {
            var locker = Locker.WaitWrite(timeout);
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitRead" />
        /// <para><see cref="ReaderWriterLocker.WaitReadByDefaultTimeout()"/>??????????????????</para>
        /// </summary>
        /// <returns></returns>
        public virtual IDatabaseTransaction WaitRead()
        {
            var locker = Locker.WaitReadByDefaultTimeout();
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <inheritdoc cref="IDatabaseBarrier.WaitRead(TimeSpan)" />
        public virtual IDatabaseTransaction WaitRead(TimeSpan timeout)
        {
            var locker = Locker.WaitRead(timeout);
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        #endregion
    }
}
