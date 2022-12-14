using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public class NotifyEventArgs: EventArgs
    { }

    public class LauncherItemChangedEventArgs: NotifyEventArgs
    {
        public LauncherItemChangedEventArgs(LauncherItemId launcherItemId)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }

    public class LauncherItemRemoveInLauncherGroupEventArgs: NotifyEventArgs
    {
        public LauncherItemRemoveInLauncherGroupEventArgs(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId, int index)
        {
            LauncherGroupId = launcherGroupId;
            LauncherItemId = launcherItemId;
            Index = index;
        }

        #region property

        public LauncherGroupId LauncherGroupId { get; }
        public LauncherItemId LauncherItemId { get; }
        public int Index { get; }

        #endregion
    }

    public class LauncherItemRegisteredEventArgs: NotifyEventArgs
    {
        public LauncherItemRegisteredEventArgs(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId)
        {
            LauncherGroupId = launcherGroupId;
            LauncherItemId = launcherItemId;
        }

        #region property

        public LauncherGroupId LauncherGroupId { get; }
        public LauncherItemId LauncherItemId { get; }

        #endregion
    }

    public class CustomizeLauncherItemExitedEventArgs: NotifyEventArgs
    {
        public CustomizeLauncherItemExitedEventArgs(LauncherItemId launcherItemId)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }

    public class LauncherGroupItemRegisteredEventArgs: NotifyEventArgs
    {
        public LauncherGroupItemRegisteredEventArgs(LauncherGroupId launcherGroupId)
        {
            LauncherGroupId = launcherGroupId;
        }

        #region property

        public LauncherGroupId LauncherGroupId { get; }

        #endregion
    }

    /// <summary>
    /// ??????????????????????????????
    /// </summary>
    public class FullscreenEventArgs: NotifyEventArgs
    {
        public FullscreenEventArgs(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            Screen = screen;
            IsFullscreen = isFullScreen;
            FullscreenWindowHandle = hWnd;
        }

        #region property

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        public IScreen Screen { get; }
        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        public bool IsFullscreen { get; }
        /// <summary>
        /// ??????????????????????????????????????????????????????????????????
        /// <para><see cref="IsFullscreen"/>????????????????????????????????????????????????</para>
        /// </summary>
        public IntPtr FullscreenWindowHandle { get; }

        #endregion
    }

    public class NotifyLogEventArgs: NotifyEventArgs
    {
        public NotifyLogEventArgs(NotifyEventKind kind, IReadOnlyNotifyMessage message)
        {
            Kind = kind;
            Message = message;
        }

        #region property

        public NotifyEventKind Kind { get; }
        public IReadOnlyNotifyMessage Message { get; }

        #endregion
    }

    /// <summary>
    /// ?????????????????????????????????????????????????????????
    /// </summary>
    public interface INotifyManager
    {
        #region event

        event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        event EventHandler<LauncherItemRemoveInLauncherGroupEventArgs>? LauncherItemRemovedInLauncherGroup;
        event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;
        event EventHandler<LauncherGroupItemRegisteredEventArgs>? LauncherGroupItemRegistered;

        /// <summary>
        /// ???????????????????????????????????????????????????????????????????????????
        /// </summary>
        event EventHandler<NotifyEventArgs>? SettingChanged;


        event EventHandler<FullscreenEventArgs>? FullscreenChanged;

        event EventHandler<NotifyLogEventArgs>? NotifyLogChanged;

        #endregion

        #region property

        ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs { get; }
        ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs { get; }

        #endregion

        #region function

        /// <summary>
        /// ??????????????????????????????????????????
        /// <para>???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????<see cref="IOrderManager.RefreshLauncherItemElement(Guid)"/>?????????????????????????????????</para>
        /// </summary>
        /// <param name="launcherItemId">??????????????????????????????????????????ID???</param>
        void SendLauncherItemChanged(LauncherItemId launcherItemId);
        void SendLauncherItemRegistered(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId);
        /// <summary>
        /// ?????????????????????????????????????????????????????????????????????????????????
        /// </summary>
        /// <param name="launcherGroupId"></param>
        /// <param name="launcherItemId"></param>
        /// <param name="index">?????????<see cref="launcherItemId"/>?????????????????????????????????????????????????????????????????????</param>
        void SendLauncherItemRemoveInLauncherGroup(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId, int index);
        void SendCustomizeLauncherItemExited(LauncherItemId launcherItemId);
        /// <summary>
        /// ???????????????????????????????????????????????????????????????????????????
        /// </summary>
        /// <param name="launcherGroupId"></param>
        void SendLauncherGroupItemRegistered(LauncherGroupId launcherGroupId);
        /// <summary>
        /// ???????????????????????????????????????????????????
        /// </summary>
        void SendSettingChanged();

        void SendFullscreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd);

        /// <summary>
        /// ?????????????????????????????????
        /// </summary>
        /// <param name="notifyLogId"></param>
        /// <returns></returns>
        bool ExistsLog(NotifyLogId notifyLogId);

        /// <summary>
        /// ?????????????????????
        /// </summary>
        /// <param name="notifyMessage"></param>
        /// <returns></returns>
        NotifyLogId AppendLog(NotifyMessage notifyMessage);
        /// <summary>
        ///???????????????????????????
        /// </summary>
        /// <param name="notifyLogId"></param>
        /// <param name="contentMessage"></param>
        void ReplaceLog(NotifyLogId notifyLogId, string contentMessage);
        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="notifyLogId"></param>
        bool ClearLog(NotifyLogId notifyLogId);
        /// <summary>
        /// ????????????????????????????????????
        /// </summary>
        /// <param name="notifyLogId"></param>
        void FadeoutLog(NotifyLogId notifyLogId);

        #endregion
    }

    internal class NotifyManager: ManagerBase, INotifyManager
    {
        #region property

        /// <summary>
        /// ????????????????????????????????????????????????
        /// </summary>
        readonly object _notifyLogsLocker = new object();

        #endregion

        #region event
        #endregion

        public NotifyManager(IDiContainer diContainer, NotifyLogConfiguration notifyLogConfiguration, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            TopmostNotifyLogsImpl = new ObservableCollection<NotifyLogItemElement>();
            StreamNotifyLogsImpl = new ObservableCollection<NotifyLogItemElement>();
            TopmostNotifyLogs = new ReadOnlyObservableCollection<NotifyLogItemElement>(TopmostNotifyLogsImpl);
            StreamNotifyLogs = new ReadOnlyObservableCollection<NotifyLogItemElement>(StreamNotifyLogsImpl);

            NotifyLogLifeTimes = new Dictionary<NotifyLogKind, TimeSpan>() {
                [NotifyLogKind.Normal] = notifyLogConfiguration.NormalLogDisplayTime,
                [NotifyLogKind.Command] = notifyLogConfiguration.CommandLogDisplayTime,
                [NotifyLogKind.Undo] = notifyLogConfiguration.UndoLogDisplayTime,
                [NotifyLogKind.Topmost] = notifyLogConfiguration.FadeoutTime,
                //[NotifyLogKind.Platform] = TimeSpan.Zero,
            };

            StreamTimer = new Timer() {
                Interval = TimeSpan.FromSeconds(1).TotalMilliseconds,
                AutoReset = true,
            };
            StreamTimer.Elapsed += StreamTimer_Elapsed;
            StreamTimer.Start();
        }

        #region property

        Timer StreamTimer { get; }
        IReadOnlyDictionary<NotifyLogKind, TimeSpan> NotifyLogLifeTimes { get; }

        IDispatcherWrapper DispatcherWrapper { get; }
        private ObservableCollection<NotifyLogItemElement> TopmostNotifyLogsImpl { get; }
        private ObservableCollection<NotifyLogItemElement> StreamNotifyLogsImpl { get; }
        private KeyedCollection<NotifyLogId, NotifyLogItemElement> NotifyLogs { get; } = new SimpleKeyedCollection<NotifyLogId, NotifyLogItemElement>(v => v.NotifyLogId);

        private IDictionary<IScreen, bool> FullscreenStatus { get; } = new Dictionary<IScreen, bool>();

        #endregion

        #region function

        [Conditional("DEBUG")]
        private void ThrowIfEmptyGuid(LauncherItemId launcherItemId)
        {
            if(launcherItemId == LauncherItemId.Empty) {
                throw new InvalidOperationException();
            }
        }

        [Conditional("DEBUG")]
        private void ThrowIfEmptyGuid(LauncherGroupId launcherItemId)
        {
            if(launcherItemId == LauncherGroupId.Empty) {
                throw new InvalidOperationException();
            }
        }

        [Conditional("DEBUG")]
        private void ThrowIfEmptyGuid(Guid launcherGroupItemId)
        {
            if(launcherGroupItemId == Guid.Empty) {
                throw new InvalidOperationException();
            }
        }

        [Conditional("DEBUG")]
        private void ThrowIfEmptyLauncherItemId(LauncherItemId launcherItemId) => ThrowIfEmptyGuid(launcherItemId);

        [Conditional("DEBUG")]
        private void ThrowIfEmptyLauncherGroupItemId(LauncherGroupId launcherGroupItemId) => ThrowIfEmptyGuid(launcherGroupItemId);

        private void OnLauncherItemChanged(LauncherItemId launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemChangedEventArgs(launcherItemId);
            LauncherItemChanged?.Invoke(this, e);
        }

        private void OnLauncherItemRegistered(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemRegisteredEventArgs(launcherGroupId, launcherItemId);
            LauncherItemRegistered?.Invoke(this, e);
        }

        private void OnLauncherItemRemovedInGroup(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId, int index)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemRemoveInLauncherGroupEventArgs(launcherGroupId, launcherItemId, index);
            LauncherItemRemovedInLauncherGroup?.Invoke(this, e);
        }

        private void OnCustomizeLauncherItemExited(LauncherItemId launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new CustomizeLauncherItemExitedEventArgs(launcherItemId);
            CustomizeLauncherItemExited?.Invoke(this, e);
        }

        private void OnLauncherGroupItemRegistered(LauncherGroupId launcherGroupItemId)
        {
            ThrowIfEmptyLauncherGroupItemId(launcherGroupItemId);

            var e = new LauncherGroupItemRegisteredEventArgs(launcherGroupItemId);
            LauncherGroupItemRegistered?.Invoke(this, e);
        }

        private void OnSettingChanged()
        {
            var e = new NotifyEventArgs();
            SettingChanged?.Invoke(this, e);
        }

        private void OnFullscreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            var e = new FullscreenEventArgs(screen, isFullScreen, hWnd);
            FullscreenChanged?.Invoke(this, e);
        }

        private void OnNotifyEventChanged(NotifyEventKind kind, IReadOnlyNotifyMessage message)
        {
            var e = new NotifyLogEventArgs(kind, message);
            NotifyLogChanged?.Invoke(this, e);
        }

        public void ClearAllLogs()
        {
            lock(this._notifyLogsLocker) {
                TopmostNotifyLogsImpl.Clear();
                StreamNotifyLogsImpl.Clear();
                NotifyLogs.Clear();
            }
        }

        #endregion

        #region INotifyManager

        public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        public event EventHandler<LauncherItemRemoveInLauncherGroupEventArgs>? LauncherItemRemovedInLauncherGroup;
        public event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        public event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;
        public event EventHandler<LauncherGroupItemRegisteredEventArgs>? LauncherGroupItemRegistered;
        public event EventHandler<NotifyEventArgs>? SettingChanged;

        public event EventHandler<FullscreenEventArgs>? FullscreenChanged;

        public event EventHandler<NotifyLogEventArgs>? NotifyLogChanged;

        public ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs { get; }
        public ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs { get; }

        public void SendLauncherItemChanged(LauncherItemId launcherItemId)
        {
            OnLauncherItemChanged(launcherItemId);
        }
        public void SendLauncherItemRemoveInLauncherGroup(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId, int index)
        {
            OnLauncherItemRemovedInGroup(launcherGroupId, launcherItemId, index);
        }

        public void SendLauncherItemRegistered(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId)
        {
            OnLauncherItemRegistered(launcherGroupId, launcherItemId);
        }

        public void SendCustomizeLauncherItemExited(LauncherItemId launcherItemId)
        {
            OnCustomizeLauncherItemExited(launcherItemId);
        }

        public void SendLauncherGroupItemRegistered(LauncherGroupId launcherGroupItemId)
        {
            OnLauncherGroupItemRegistered(launcherGroupItemId);
        }

        public void SendSettingChanged()
        {
            OnSettingChanged();
        }

        public void SendFullscreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            var fire = false;

            if(FullscreenStatus.TryGetValue(screen, out var currentValue)) {
                if(isFullScreen != currentValue) {
                    Logger.LogTrace("??????: {0}", isFullScreen);
                    FullscreenStatus[screen] = isFullScreen;
                    fire = true;
                }
            } else {
                var existsScreen = FullscreenStatus.Keys.FirstOrDefault(i => i.DeviceName == screen.DeviceName);
                if(existsScreen != null) {
                    if(FullscreenStatus[existsScreen] != isFullScreen) {
                        Logger.LogTrace("???????????????: {0}", isFullScreen);
                        FullscreenStatus[existsScreen] = isFullScreen;
                        fire = true;
                    }
                } else {
                    // ?????????????????????????????????????????????????????????????????????????????????????????????
                    if(isFullScreen) {
                        Logger.LogTrace("?????????: {0}", isFullScreen);
                        FullscreenStatus.Add(screen, isFullScreen);
                        fire = true;
                    }
                }
            }
            if(fire) {
                Logger.LogDebug("?????????????????????????????????: {0}, {1}", screen, isFullScreen);
                OnFullscreenChanged(screen, isFullScreen, hWnd);
            }
        }

        /// <inheritdoc cref="INotifyManager.ExistsLog(NotifyLogId)" />
        public bool ExistsLog(NotifyLogId notifyLogId)
        {
            return NotifyLogs.Contains(notifyLogId);
        }

        /// <inheritdoc cref="INotifyManager.AppendLog(NotifyMessage)" />
        public NotifyLogId AppendLog(NotifyMessage notifyMessage)
        {
            if(notifyMessage == null) {
                throw new ArgumentNullException(nameof(notifyMessage));
            }

            var element = DiContainer.Build<NotifyLogItemElement>(NotifyLogId.NewId(), notifyMessage);
            element.Initialize();

            Logger.LogDebug("[{0}] {1}: {2}, {3}", notifyMessage.Header, notifyMessage.Kind, notifyMessage.Content.Message, element.NotifyLogId);

            DispatcherWrapper.Begin(() => {
                NotifyLogs.Add(element);
                if(element.Kind == NotifyLogKind.Topmost) {
                    TopmostNotifyLogsImpl.Add(element);
                } else {
                    StreamNotifyLogsImpl.Add(element);
                }

                OnNotifyEventChanged(NotifyEventKind.Add, element);
            });

            return element.NotifyLogId;
        }
        /// <inheritdoc cref="INotifyManager.ReplaceLog(NotifyLogId, string)" />
        public void ReplaceLog(NotifyLogId notifyLogId, string contentMessage)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                throw new KeyNotFoundException(notifyLogId.ToString());
            }

            Logger.LogDebug("[{0}] ??????: {1}, {2}", element.Header, contentMessage, element.NotifyLogId);

            DispatcherWrapper.Begin(() => {
                element.ChangeContent(new NotifyLogContent(contentMessage, DateTime.UtcNow));
                OnNotifyEventChanged(NotifyEventKind.Change, element);
            });
        }
        /// <inheritdoc cref="INotifyManager.ClearLog(NotifyLogId)" />
        public bool ClearLog(NotifyLogId notifyLogId)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                return false;
            }

            lock(this._notifyLogsLocker) {
                if(0 < NotifyLogs.Count && NotifyLogs.Remove(notifyLogId)) {
                    DispatcherWrapper.Begin(() => {
                        if(element.Kind == NotifyLogKind.Topmost) {
                            TopmostNotifyLogsImpl.Remove(element);
                        } else {
                            StreamNotifyLogsImpl.Remove(element);
                        }

                        OnNotifyEventChanged(NotifyEventKind.Clear, element);
                        element.Dispose();
                    });

                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc cref="INotifyManager.FadeoutLog(NotifyLogId)" />
        public void FadeoutLog(NotifyLogId notifyLogId)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                return;
            }

            Task.Delay(NotifyLogLifeTimes[element.Kind]).ContinueWith(t => {
                ClearLog(notifyLogId);
            });
        }

        #endregion

        private void StreamTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            var time = DateTime.UtcNow;
            var removeLogs = StreamNotifyLogsImpl
                .Where(i => i.Content.Timestamp + NotifyLogLifeTimes[i.Kind] < time)
                .Select(i => i.NotifyLogId)
                .ToList()
            ;
            if(0 < removeLogs.Count) {
                foreach(var removeLog in removeLogs) {
                    ClearLog(removeLog);
                }
            }
        }
    }
}
