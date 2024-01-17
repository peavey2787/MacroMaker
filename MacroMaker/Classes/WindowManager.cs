
namespace MacroMaker.Classes
{
    using System;
    using System.Runtime.InteropServices;

    public static class WindowManager
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public static void BringWindowToFront(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                SwitchToThisWindow(hWnd, true);
            }
        }
    }


}
