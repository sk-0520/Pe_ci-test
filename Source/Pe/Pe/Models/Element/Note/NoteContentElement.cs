using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteContentElement : ElementBase, IFlushable
    {
        #region

        public event EventHandler? LinkContentChanged;

        #endregion

        #region variable
        #endregion

        public NoteContentElement(Guid noteId, NoteContentKind contentKind, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            ContentKind = contentKind;

            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            DispatcherWapper = dispatcherWapper;

            MainDatabaseLazyWriter = mainDatabaseLazyWriter;

            LinkContentLazyChanger = new LazyAction(nameof(LinkContentLazyChanger), TimeSpan.FromSeconds(5), LoggerFactory);

        }

        #region property

        public Guid NoteId { get; }
        public NoteContentKind ContentKind { get; }

        public bool IsLink { get; private set; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IDispatcherWapper DispatcherWapper { get; }
        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        [Obsolete]
        NoteLinkContentWatcher? LinkWatcher { get; set; }
        NoteLinkWatcher? LinkWatcher2 { get; set; }
        LazyAction LinkContentLazyChanger { get; }

        #endregion

        #region function

        public bool Exists()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectExistsContent(NoteId);
            }
        }

        void CreateNewContent(string content)
        {
            Logger.LogInformation("ノート空コンテンツ生成: {0}, {1}", NoteId, ContentKind);
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = ContentKind,
                    Content = content,
                    //TODO: あとあとかんがえよ
                    IsLink = false,
                    FilePath = string.Empty,
                    Encoding = Encoding.UTF8,
                    DelayTime = TimeSpan.Zero,
                    BufferSize = 0,
                    RefreshTime = TimeSpan.Zero,
                    IsEnabledRefresh = true,
                };
                dao.InsertNewContent(data, DatabaseCommonStatus.CreateCurrentAccount());
                commander.Commit();
            }
        }

        public string LoadRawContent()
        {
            if(IsLink) {
                return LoadLinkContent();
            }

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectFullContent(NoteId);
            }
        }

        public string LoadPlainContent()
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

            if(!Exists()) {
                // 存在しなければこのタイミングで生成
                var factory = new NoteContentFactory();
                var content = factory.CreatePlain();
                CreateNewContent(content);
                // 作ったやつを返すだけなので別に。
                return content;
            }

            return LoadRawContent();
        }

        public string LoadRichTextContent()
        {
            if(ContentKind != NoteContentKind.RichText) {
                throw new InvalidOperationException();
            }

            if(!Exists()) {
                var factory = new NoteContentFactory();
                var content = factory.CreateRichText();
                CreateNewContent(content);
                return content;
            }

            return LoadRawContent();
        }

        [Obsolete]
        public NoteLinkContentData LoadLinkSetting()
        {
            if(ContentKind != NoteContentKind.Link) {
                throw new InvalidOperationException();
            }

            var converter = new NoteContentConverter(LoggerFactory);

            if(!Exists()) {
                var factory = new NoteContentFactory();
                var setting = factory.CreateLink();

                var content = converter.ToLinkSettingString(setting);
                CreateNewContent(content);
                return setting;
            }

            var rawSetting = LoadRawContent();
            return converter.ToLinkSetting(rawSetting);
        }

        [Obsolete]
        public string LoadLinkContent(FileInfo file, Encoding encoding)
        {
            using(var stream = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)) {
                var reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
        }

        string LoadLinkContent()
        {
            var parameter = (NoteLinkWatchParameter?)LinkWatcher2?.WatchParameter;
            if(parameter == null) {
                Logger.LogWarning("リンクがおかしい: {0}", NoteId);
                return string.Empty;
            }

            using(var stream = parameter.File!.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                var reader = new StreamReader(stream, parameter.Encoding!);
                return reader.ReadToEnd();
            }
        }

        [Obsolete]
        public NoteLinkContentWatcher StartLinkWatch(NoteLinkContentData linkData)
        {
            if(LinkWatcher == null) {
                LinkWatcher = new NoteLinkContentWatcher(linkData, LoggerFactory);
            }
            LinkWatcher.Start();
            return LinkWatcher;
        }

        public void StartLinkWatch(NoteLinkWatchParameter noteLinkWatchParameter)
        {
            if(LinkWatcher2 == null) {
                LinkWatcher2 = new NoteLinkWatcher(noteLinkWatchParameter, LoggerFactory);
                LinkWatcher2.FileContentChanged += LinkWatcher2_FileContentChanged;
            }
            LinkWatcher2.Start();
        }

        public void StopLinkWatch()
        {
            //if(LinkWatcher != null) {
            //    LinkWatcher.Stop();
            //}
            LinkWatcher2?.Stop();
        }

        void SaveLinkContent(string? content)
        {
            var parameter = (NoteLinkWatchParameter?)LinkWatcher2?.WatchParameter;
            if(parameter == null) {
                Logger.LogWarning("リンクがおかしい: {0}", NoteId);
                return;
            }

            StopLinkWatch();
            using(new ActionDisposer(() => StartLinkWatch(parameter))) {
                using(var stream = parameter.File!.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)) {
                    var writer = new StreamWriter(stream, parameter.Encoding!);
                    writer.Write(content);
                }
            }
        }

        void ChangeRawContent(NoteContentKind contentKind, string? content, object stockKey)
        {
            if(IsLink) {
                SaveLinkContent(content);
                return;
            }

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new NoteContentsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = contentKind,
                    Content = content,
                };
                dao.UpdateContent(data, DatabaseCommonStatus.CreateCurrentAccount());
            }, stockKey);
        }

        public void ChangePlainContent(string? content)
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

            ChangeRawContent(ContentKind, content, UniqueKeyPool.Get());
        }

        public void ChangeRichTextContent(string content)
        {
            if(ContentKind != NoteContentKind.RichText) {
                throw new InvalidOperationException();
            }

            ChangeRawContent(ContentKind, content, UniqueKeyPool.Get());
        }

        [Obsolete]
        void DelayLinkContentChange(string content)
        {
            LinkContentLazyChanger.DelayAction(() => {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                LinkWatcher.Stop();
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

                using(var stream = new FileStream(LinkWatcher.File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)) {
                    using(var writer = new StreamWriter(stream, LinkWatcher.Encoding)) {
                        writer.Write(content);
                    }
                }

                LinkWatcher.Start();
            });
        }

        [Obsolete]
        public void ChangeLinkContent(string content)
        {
            if(ContentKind != NoteContentKind.Link) {
                throw new InvalidOperationException();
            }
            if(LinkWatcher == null) {
                throw new InvalidOperationException();
            }

            DelayLinkContentChange(content);
        }

        void DisposeLinkWatcher()
        {
            //if(LinkWatcher != null) {
            //    LinkWatcher.Dispose();
            //}
            if(LinkWatcher2!= null) {
                LinkWatcher2.FileContentChanged -= LinkWatcher2_FileContentChanged;
                LinkWatcher2.Dispose();
            }
        }

        public void ChangeLink(string filePath, Encoding encoding)
        {
            Flush();
            var content = LoadRawContent();

            var path = Environment.ExpandEnvironmentVariables(filePath);
            using(var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                using(var writer = new StreamWriter(stream, encoding)) {
                    writer.Write(content);
                }
            }

            var noteLinkWatchParameter = new NoteLinkWatchParameter() {
                File = new FileInfo(path),
                Encoding = encoding,
            };

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                // 環境変数込みで書き込み
                dao.UpdateLinkEnabled(NoteId, filePath, encoding, noteLinkWatchParameter, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            IsLink = true;
            StartLinkWatch(noteLinkWatchParameter);
        }

        (bool success, bool isLink, NoteLinkWatchParameter parameter) LoadLinkWatchParameter()
        {
            NoteContentData linkData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                linkData = dao.SelectLinkParameter(NoteId);
            }

            var parameter = new NoteLinkWatchParameter();

            if(!linkData.IsLink) {
                return (true, false, parameter);
            }

            if(string.IsNullOrWhiteSpace(linkData.FilePath)) {
                return (false, false, parameter);
            }

            var path = Environment.ExpandEnvironmentVariables(linkData.FilePath);
            parameter.File = new FileInfo(path);
            parameter.Encoding = linkData.Encoding;
            parameter.BufferSize = linkData.BufferSize;
            parameter.DelayTime = linkData.DelayTime;
            parameter.RefreshTime = linkData.RefreshTime;
            parameter.IsEnabledRefresh = linkData.IsEnabledRefresh;

            return (true, true, parameter);
        }

        #endregion

        #region ElementBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                DisposeLinkWatcher();
                if(disposing) {
                    LinkContentLazyChanger.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            if(!Exists()) {
                return;
            }

            var values = LoadLinkWatchParameter();
            if(!values.success) {
                Logger.LogError("ノード内容初期化失敗: {0}", NoteId);
                return;
            }

            IsLink = values.isLink;
            if(IsLink) {
                StartLinkWatch(values.parameter);
            }
        }

        #endregion

        #region IFlush

        public void Flush()
        {
            LinkContentLazyChanger.SafeFlush();
            MainDatabaseLazyWriter.SafeFlush();
        }

        #endregion

        private void LinkWatcher2_FileContentChanged(object? sender, FileChangedEventArgs e)
        {
            LinkContentChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
