using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MacroMaker.Classes
{
    public class WinHooks
    {
        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;

        private IntPtr mouseHookId = IntPtr.Zero;
        private IntPtr keyboardHookId = IntPtr.Zero;
        private LowLevelMouseProc mouseProc;
        private LowLevelKeyboardProc keyboardProc;

        public event Action<int> MouseButtonDown;
        public event Action<int> KeyDown;

        public List<int> BlockMouseButtonIdsDown { get; set; } = new List<int>();
        public List<int> BlockMouseButtonIdsUp { get; set; } = new List<int>();

        public WinHooks()
        {
            mouseProc = MouseHookCallback;
            mouseHookId = SetHook(WH_MOUSE_LL, mouseProc);

            keyboardProc = KeyboardHookCallback;
            keyboardHookId = SetHook(WH_KEYBOARD_LL, keyboardProc);
        }

        ~WinHooks()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources, if any
            }

            UnhookWindowsHookEx(mouseHookId);
            UnhookWindowsHookEx(keyboardHookId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IntPtr SetHook(int idHook, Delegate proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(idHook, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int parsedWParam = (int)wParam;

            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                // Determine which button was pressed
                int buttonNumber = 0;

                if ((int)hookStruct.mouseData > 0)
                    buttonNumber = (int)hookStruct.mouseData;
                else if (parsedWParam > 0)
                    buttonNumber = parsedWParam;


                if (buttonNumber != 0)
                    MouseButtonDown?.Invoke(buttonNumber);

                // Check if the default event should be altered or blocked
                if (BlockMouseButtonIdsDown.Contains(buttonNumber) || BlockMouseButtonIdsUp.Contains(buttonNumber))
                {
                    if (BlockMouseButtonIdsDown.Contains(buttonNumber))
                        BlockMouseButtonIdsDown.Remove(buttonNumber);
                    else if (BlockMouseButtonIdsUp.Contains(buttonNumber))
                        BlockMouseButtonIdsUp.Remove(buttonNumber);

                    // Modify lParam to indicate that the button is not pressed
                    hookStruct.flags |= 0x1; // Set injected flag (LLMHF_INJECTED) to prevent re-injection
                    hookStruct.mouseData = 0; // Set mouseData to 0 to indicate no button pressed

                    // Convert the modified structure back to IntPtr
                    Marshal.StructureToPtr(hookStruct, lParam, true);

                    // Allow the event to propagate
                    return CallNextHookEx(mouseHookId, -1, wParam, lParam);
                }
            }

            // If no modification is needed, proceed with the default action
            return CallNextHookEx(mouseHookId, nCode, wParam, lParam);
        }


        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (int)wParam == WM_KEYDOWN)
            {
                KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                KeyDown?.Invoke((int)hookStruct.vkCode);
            }

            return CallNextHookEx(keyboardHookId, nCode, wParam, lParam);
        }

        #region WinAPI Functions

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const int WM_KEYDOWN = 0x0100;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
    }
}
