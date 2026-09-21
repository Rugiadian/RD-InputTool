using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RD_Tools
{
    public static class InputSimulator
    {
        public static void ClickAt(int x, int y)
        {
            NativeMethods.SetCursorPos(x, y);
            Thread.Sleep(30);
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(30);
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public static void SendUnicodeText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var inputs = new NativeMethods.INPUT[text.Length * 2];
            int idx = 0;

            foreach (char c in text)
            {
                if (c == '\r') continue; // Skip CR, handle LF
                if (c == '\n')
                {
                    // Enter key
                    inputs[idx++] = new NativeMethods.INPUT
                    {
                        type = NativeMethods.INPUT_KEYBOARD,
                        u = new NativeMethods.InputUnion
                        {
                            ki = new NativeMethods.KEYBDINPUT
                            {
                                wVk = NativeMethods.VK_RETURN,
                                dwFlags = NativeMethods.KEYEVENTF_KEYDOWN
                            }
                        }
                    };
                    inputs[idx++] = new NativeMethods.INPUT
                    {
                        type = NativeMethods.INPUT_KEYBOARD,
                        u = new NativeMethods.InputUnion
                        {
                            ki = new NativeMethods.KEYBDINPUT
                            {
                                wVk = NativeMethods.VK_RETURN,
                                dwFlags = NativeMethods.KEYEVENTF_KEYUP
                            }
                        }
                    };
                    continue;
                }

                inputs[idx++] = new NativeMethods.INPUT
                {
                    type = NativeMethods.INPUT_KEYBOARD,
                    u = new NativeMethods.InputUnion
                    {
                        ki = new NativeMethods.KEYBDINPUT
                        {
                            wScan = c,
                            dwFlags = NativeMethods.KEYEVENTF_UNICODE | NativeMethods.KEYEVENTF_KEYDOWN
                        }
                    }
                };

                inputs[idx++] = new NativeMethods.INPUT
                {
                    type = NativeMethods.INPUT_KEYBOARD,
                    u = new NativeMethods.InputUnion
                    {
                        ki = new NativeMethods.KEYBDINPUT
                        {
                            wScan = c,
                            dwFlags = NativeMethods.KEYEVENTF_UNICODE | NativeMethods.KEYEVENTF_KEYUP
                        }
                    }
                };
            }

            if (idx > 0)
            {
                if (idx < inputs.Length)
                {
                    Array.Resize(ref inputs, idx);
                }
                NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<NativeMethods.INPUT>());
            }
        }

        public static void SendClipboardPaste(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            // Set clipboard in STA thread
            var thread = new Thread(() =>
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch { }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Thread.Sleep(50);

            // Press Ctrl+V
            NativeMethods.keybd_event(NativeMethods.VK_CONTROL, 0, NativeMethods.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            NativeMethods.keybd_event(NativeMethods.VK_V, 0, NativeMethods.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            Thread.Sleep(20);
            NativeMethods.keybd_event(NativeMethods.VK_V, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
            NativeMethods.keybd_event(NativeMethods.VK_CONTROL, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void SendEnter()
        {
            NativeMethods.keybd_event(NativeMethods.VK_RETURN, 0, NativeMethods.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            Thread.Sleep(20);
            NativeMethods.keybd_event(NativeMethods.VK_RETURN, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void ReleaseStuckKeys()
        {
            try
            {
                NativeMethods.keybd_event(NativeMethods.VK_CONTROL, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
                NativeMethods.keybd_event(NativeMethods.VK_V, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
                NativeMethods.keybd_event(NativeMethods.VK_RETURN, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
                NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            }
            catch { }
        }
    }
}
