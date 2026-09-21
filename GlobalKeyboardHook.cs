using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RD_Tools
{
    public class HookKeyEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool Alt { get; }
        public bool Control { get; }
        public bool Shift { get; }
        public bool Handled { get; set; }

        public HookKeyEventArgs(Keys key, bool alt, bool control, bool shift)
        {
            Key = key;
            Alt = alt;
            Control = control;
            Shift = shift;
            Handled = false;
        }
    }

    public class GlobalKeyboardHook : IDisposable
    {
        private IntPtr _hookId = IntPtr.Zero;
        private NativeMethods.LowLevelKeyboardProc _proc;

        public event EventHandler<HookKeyEventArgs> KeyDown;

        public GlobalKeyboardHook()
        {
            _proc = HookCallback;
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                _hookId = NativeMethods.SetWindowsHookEx(
                    NativeMethods.WH_KEYBOARD_LL,
                    _proc,
                    NativeMethods.GetModuleHandle(curModule?.ModuleName ?? string.Empty),
                    0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();
                if (msg == NativeMethods.WM_KEYDOWN || msg == NativeMethods.WM_SYSKEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    var key = (Keys)vkCode;

                    bool alt = (NativeMethods.GetAsyncKeyState(NativeMethods.VK_MENU) & 0x8000) != 0;
                    bool ctrl = (NativeMethods.GetAsyncKeyState(NativeMethods.VK_CONTROL) & 0x8000) != 0;
                    bool shift = (NativeMethods.GetAsyncKeyState(NativeMethods.VK_SHIFT) & 0x8000) != 0;

                    var args = new HookKeyEventArgs(key, alt, ctrl, shift);
                    KeyDown?.Invoke(this, args);

                    if (args.Handled)
                    {
                        return (IntPtr)1;
                    }
                }
            }

            return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            if (_hookId != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }
    }
}
