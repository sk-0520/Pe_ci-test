using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public class FileSystemSelectDialogRequestParameter : FileDialogRequestParameter
    {
        #region peoperty

        /// <summary>
        /// ファイルを選択するか。
        /// </summary>
        public FileSystemDialogMode FileSystemDialogMode { get; set; }

        #endregion
    }

    public enum FileSystemDialogMode
    {
        FileOpen,
        FileSave,
        Directory,
    }

    public class FileSystemSelectDialogRequestResponse : FileDialogRequestResponse
    {
        #region peoperty
        #endregion
    }

    public class IconSelectDialogRequestParameter : RequestParameter
    {
        #region peoperty

        public string FileName { get; set; } = string.Empty;
        public int IconIndex { get; set; }

        #endregion
    }

    public class IconSelectDialogRequestResponse : CancelResponse
    {
        #region peoperty

        public string FileName { get; set; } = string.Empty;
        public int IconIndex { get; set; }


        #endregion
    }

    public class DialogRequester
    {
        public DialogRequester(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        public string ExpandPath(string? path) => Environment.ExpandEnvironmentVariables(path ?? string.Empty);

        public DialogFilterItem CreateAllFilter() => new DialogFilterItem("all", string.Empty, "*");

        private void SelectFileSystem(IRequestSender requestSender, string path, FileSystemDialogMode fileSystemDialogMode, IEnumerable<DialogFilterItem> filters, Action<FileSystemSelectDialogRequestResponse> response)
        {
            var parameter = new FileSystemSelectDialogRequestParameter() {
                FilePath = path,
                FileSystemDialogMode = fileSystemDialogMode,
            };

            if(filters.Any()) {
                parameter.Filter.SetRange(filters);
            }

            requestSender.Send<FileSystemSelectDialogRequestResponse>(parameter, r => {
                if(r.ResponseIsCancel) {
                    Logger.LogTrace("cancel");
                    return;
                }
                response(r);
            });
        }

        /// <summary>
        /// ファイル選択。
        /// </summary>
        /// <param name="requestSender"></param>
        /// <param name="path"></param>
        /// <param name="filters"></param>
        /// <param name="response"><see cref="FileSystemSelectDialogRequestResponse.ResponseIsCancel"/>は真。</param>
        public void SelectFile(IRequestSender requestSender, string path, bool isOpen, IEnumerable<DialogFilterItem> filters, Action<FileSystemSelectDialogRequestResponse> response)
        {
            var mode = isOpen ? FileSystemDialogMode.FileOpen : FileSystemDialogMode.FileSave;
            SelectFileSystem(requestSender, path, mode, filters, response);
        }
        public void SelectDirectory(IRequestSender requestSender, string path, Action<FileSystemSelectDialogRequestResponse> response)
        {
            SelectFileSystem(requestSender, path, FileSystemDialogMode.Directory, Enumerable.Empty<DialogFilterItem>(), response);
        }


        public void SelectIcon(IRequestSender requestSender, string path, int index, Action<IconSelectDialogRequestResponse> response)
        {
            var parameter = new IconSelectDialogRequestParameter() {
                FileName = path,
                IconIndex = index,
            };
            requestSender.Send< IconSelectDialogRequestResponse>(parameter, r => {
                if(r.ResponseIsCancel) {
                    Logger.LogTrace("cancel");
                    return;
                }
                response(r);
            });

        }

        #endregion
    }
}
