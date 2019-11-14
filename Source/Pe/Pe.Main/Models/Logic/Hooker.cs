using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public abstract class HookerBase: DisposerBase
    {
        public HookerBase(ILoggerFactory loggerFactory)
        {
            HookProc = HookProcedure;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IntPtr HookHandle { get; private set; }
        HookProc HookProc { get; }
        protected ILogger Logger { get; }

        public bool IsEnabled => HookHandle != IntPtr.Zero;

        #endregion

        #region function

        protected abstract IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle);

        public void Register()
        {
            var moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
            var moduleHandle = NativeMethods.GetModuleHandle(moduleName);
            HookHandle = RegisterImpl(HookProc, moduleHandle);
        }

        public void Unregister()
        {
            NativeMethods.UnhookWindowsHookEx(HookHandle);
            HookHandle = IntPtr.Zero;
        }

        protected virtual IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            Logger.LogTrace("code = {0}, wParam = {1}, lParam = {2}", code, wParam, lParam);
            return NativeMethods.CallNextHookEx(HookHandle, code, wParam, lParam);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(IsEnabled) {
                    Unregister();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public enum KeyboardHookChainMode
    {
        /// <summary>
        /// 通常処理。
        /// </summary>
        Next,
        /// <summary>
        /// 置き換え。
        /// </summary>
        Replace,
        /// <summary>
        /// 無効化。
        /// </summary>
        Cancel,
    }

    public class KeyboardHookEventArgs: EventArgs
    {
        #region proeprty

        public KBDLLHOOKSTRUCT kbdll;

        #endregion

        public KeyboardHookEventArgs(bool isUp, IntPtr lParam)
        {
            this.kbdll = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT))!;

            Key = KeyInterop.KeyFromVirtualKey((int)this.kbdll.vkCode);
        }

        #region property

        public Key Key { get; }

        public KeyboardHookChainMode ChainMode { get; set; }

        #endregion

    }

    public class KeyboradHooker : HookerBase
    {
        #region event

        public event EventHandler<KeyboardHookEventArgs>? KeyDown;
        public event EventHandler<KeyboardHookEventArgs>? KeyUp;

        #endregion

        public KeyboradHooker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        #endregion

        #region HookerBase

        protected override IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle)
        {
            return NativeMethods.SetWindowsHookEx(WH.WH_KEYBOARD_LL, hookProc, moduleHandle, 0);
        }

        protected override IntPtr HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            if(code <= 0) {
                var message = wParam.ToInt32();
                var isDown = message == (int)WM.WM_KEYUP || message == (int)WM.WM_SYSKEYUP;
                var isUp = message == (int)WM.WM_KEYDOWN || message == (int)WM.WM_SYSKEYDOWN;
                if(isUp || isDown) {
                    var e = new KeyboardHookEventArgs(isUp, lParam);
                    if(isUp) {
                        KeyUp?.Invoke(this, e);
                    } else {
                        KeyDown?.Invoke(this, e);
                    }

                    if(e.ChainMode == KeyboardHookChainMode.Cancel) {
                        return new IntPtr(1);
                    }
                }

            }
            return NativeMethods.CallNextHookEx(HookHandle, code, wParam, lParam);
        }

        #endregion
    }

    public class MouseHooker: HookerBase
    {
        public MouseHooker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region HookerBase

        protected override IntPtr RegisterImpl(HookProc hookProc, IntPtr moduleHandle)
        {
            return NativeMethods.SetWindowsHookEx(WH.WH_MOUSE_LL, hookProc, moduleHandle, 0);
        }

        #endregion
    }
}
