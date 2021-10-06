using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public class ReleaseNoteElement: ElementBase
    {
        public ReleaseNoteElement(NewVersionInfo newVersionInfo, IReadOnlyNewVersionItemData updateItem, bool isCheckOnly, IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NewVersionInfoImpl = newVersionInfo;
            NewVersionItem = updateItem;

            IsCheckOnly = isCheckOnly;

            OrderManager = orderManager;
            UserAgentManager = userAgentManager;
        }

        #region property
        NewVersionInfo NewVersionInfoImpl { get; }
        public IReadOnlyNewVersionInfo NewVersionInfo => NewVersionInfoImpl;

        public IReadOnlyNewVersionItemData NewVersionItem { get; }
        public bool IsCheckOnly { get; private set; }
        IOrderManager OrderManager { get; }
        IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        /// <summary>
        /// リリースノートを取得する。
        /// </summary>
        /// <remarks>ブラウザ機能でダウンロードしちゃう可能性があるので本体機能で落とす。</remarks>
        /// <returns></returns>
        public async Task<string> LoadReleaseNoteDocumentAsync()
        {
            using(var userAgent = UserAgentManager.CreateAppHttpUserAgent()) {
                return await userAgent.GetStringAsync(NewVersionItem.NoteUri);
                //return await userAgent.GetStringAsync(new Uri("https://bitbucket.org/sk_0520/pe/downloads/update-release.html"));
            }
        }

        public void StartDownload()
        {
            OrderManager.StartUpdate(UpdateTarget.Application, UpdateProcess.Download);
            IsCheckOnly = false;
        }

        public void StartUpdate()
        {
            OrderManager.StartUpdate(UpdateTarget.Application, UpdateProcess.Update);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
