using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class RedoParameter
    {
        public RedoParameter(ILauncherExecutePathParameter path, ILauncherExecuteCustomParameter custom, IReadOnlyCollection<LauncherEnvironmentVariableData> environmentVariableItems, IReadOnlyLauncherRedoData redoData, IScreen screen)
        {
            Path = path;
            Custom = custom;
            EnvironmentVariableItems = environmentVariableItems;
            RedoData = redoData;
            Screen = screen;
        }

        #region property

        public ILauncherExecutePathParameter Path { get; }
        public ILauncherExecuteCustomParameter Custom { get; }
        public IReadOnlyCollection<LauncherEnvironmentVariableData> EnvironmentVariableItems { get; }
        public IReadOnlyLauncherRedoData RedoData { get; }
        public IScreen Screen { get; }

        #endregion
    }

    public class RedoExecutor: DisposerBase
    {
        #region event

        public event EventHandler? Exited;

        #endregion

        public RedoExecutor(LauncherExecutor executor, ILauncherExecuteResult firstResult, RedoParameter parameter, INotifyManager notifyManager, ILoggerFactory loggerFactory)
        {
            if(firstResult.Process == null) {
                throw new ArgumentException($"{nameof(firstResult)}.{nameof(firstResult.Process)}");
            }
            if(parameter.RedoData.RedoWait == RedoWait.None) {
                throw new ArgumentException($"{nameof(parameter)}.{nameof(parameter.RedoData)}.{nameof(parameter.RedoData.RedoWait)}");
            }

            Logger = loggerFactory.CreateLogger(GetType());

            Executor = executor;
            FirstResult = firstResult;
            Parameter = parameter;
            NotifyManager = notifyManager;

            if(Parameter.RedoData.RedoWait == RedoWait.Timeout || Parameter.RedoData.RedoWait == RedoWait.TimeoutAndCount) {
                Stopwatch = Stopwatch.StartNew();
                WaitEndTimer = new Timer() {
                    Interval = (int)(Parameter.RedoData.WaitTime + Stopwatch.Elapsed).TotalMilliseconds,
                    AutoReset = false,
                };
                WaitEndTimer.Elapsed += WaitEndTimer_Elapsed;
                WaitEndTimer.Start();
            }

            Watching(FirstResult.Process, false);
        }


        #region property

        ILogger Logger { get; }

        Guid NotifyLogId { get; set; }

        ILauncherExecuteResult FirstResult { get; }
        LauncherExecutor Executor { get; }
        RedoParameter Parameter { get; }

        INotifyManager NotifyManager { get; }

        Stopwatch? Stopwatch { get; }

        Process? CurrentProcess { get; set; }
        Timer? WaitEndTimer { get; }

        int RetryCount { get; set; }

        public bool IsExited { get; private set; }

        #endregion

        #region function

        bool IsTimeout() => Stopwatch != null && Parameter.RedoData.WaitTime < Stopwatch.Elapsed;
        bool IsMaxRetry() => Parameter.RedoData.RetryCount <= RetryCount;

        void OnExited()
        {
            IsExited = true;
            Exited?.Invoke(this, EventArgs.Empty);

            NotifyManager.FadeoutLog(NotifyLogId);

            Dispose();
        }

        void Watching(Process process, bool isContinue)
        {
            CurrentProcess = process;
            CurrentProcess.EnableRaisingEvents = true;

            if(isContinue && CurrentProcess.HasExited) {
                if(Check(CurrentProcess)) {
                    Execute();
                } else {
                    OnExited();
                }
            } else {
                CurrentProcess.Exited += Process_Exited;
            }
        }

        /// <summary>
        /// 再試行が可能か。
        /// </summary>
        /// <param name="process"></param>
        /// <returns>真: 再試行が可能。</returns>
        bool Check(Process process)
        {
            if(!process.HasExited) {
                Logger.LogWarning("到達不可");
                return false;
            }

            if(Parameter.RedoData.SuccessExitCodes.Any(i => i == process.ExitCode)) {
                Logger.LogInformation("正常終了コードのため再試行不要: {0}", process.ExitCode);
                if(NotifyLogId != Guid.Empty) {
                    NotifyManager.ReplaceLog(NotifyLogId, "正常終了");
                }

                return false;
            }

            switch(Parameter.RedoData.RedoWait) {
                case RedoWait.Timeout:
                    if(IsTimeout()) {
                        NotifyManager.ReplaceLog(NotifyLogId, "タイムアウト");
                        Logger.LogInformation("タイムアウト");
                        return false;
                    }
                    break;

                case RedoWait.Count:
                    if(IsMaxRetry()) {
                        NotifyManager.ReplaceLog(NotifyLogId, "試行回数超過");
                        Logger.LogInformation("試行回数超過");
                        return false;
                    }
                    break;

                case RedoWait.TimeoutAndCount:
                    if(IsTimeout() || IsMaxRetry()) {
                        NotifyManager.ReplaceLog(NotifyLogId, "タイムアウト/試行回数超過");
                        Logger.LogInformation("タイムアウト/試行回数超過");
                        return false;
                    }
                    break;

                case RedoWait.None:
                default:
                    throw new NotImplementedException();
            }

            Logger.LogTrace("再実施可能");

            return true;
        }

        string CreateRedoNotifyLogMessage()
        {
            return "@再実施";
        }

        void Execute()
        {
            if(NotifyLogId == Guid.Empty) {
                NotifyLogId = NotifyManager.AppendLog(new NotifyMessage(NotifyLogKind.Topmost, Parameter.Custom.Caption, new NotifyLogContent(CreateRedoNotifyLogMessage())));
            } else {
                NotifyManager.ReplaceLog(NotifyLogId, CreateRedoNotifyLogMessage());
            }

            var result = Executor.Execute(FirstResult.Kind, Parameter.Path, Parameter.Custom, Parameter.EnvironmentVariableItems, LauncherRedoData.GetDisable(), Parameter.Screen);
            RetryCount += 1;
            Watching(result.Process!, true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(WaitEndTimer != null) {
                    WaitEndTimer.Elapsed -= WaitEndTimer_Elapsed;
                    if(disposing) {
                        WaitEndTimer.Dispose();
                    }
                }

                if(CurrentProcess != null) {
                    CurrentProcess.Exited -= Process_Exited;
                }

            }

            base.Dispose(disposing);
        }

        #endregion

        private void Process_Exited(object? sender, EventArgs e)
        {
            Debug.Assert(CurrentProcess != null);

            CurrentProcess.Exited -= Process_Exited;

            if(Check(CurrentProcess)) {
                Execute();
            } else {
                OnExited();
            }
        }

        private void WaitEndTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Assert(CurrentProcess != null);
            Debug.Assert(WaitEndTimer != null);

            if(NotifyLogId != Guid.Empty) {
                NotifyManager.ReplaceLog(NotifyLogId, "監視終了");
            }
            OnExited();
        }
    }
}
