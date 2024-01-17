using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public static class ProcessManager
{
    public static List<IntPtr> GetOpenTabsWithText(string searchText)
    {
        List<IntPtr> matchingTabs = new List<IntPtr>();

        Process[] processes = Process.GetProcessesByName("firefox");
        foreach (Process process in processes)
        {
            try
            {
                EnumerateProcessWindows(process.Id, searchText, matchingTabs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking process: {ex.Message}");
            }
        }

        return matchingTabs;
    }

    private static void EnumerateProcessWindows(int processId, string searchText, List<IntPtr> matchingTabs)
    {
        EnumWindows((hwnd, lParam) =>
        {
            int windowProcessId;
            GetWindowThreadProcessId(hwnd, out windowProcessId);

            if (windowProcessId == processId)
            {
                string mainWindowTitle = GetWindowTitle(hwnd);
                if (!string.IsNullOrEmpty(mainWindowTitle) && mainWindowTitle.ToLower().Contains(searchText.ToLower()))
                {
                    matchingTabs.Add(hwnd);
                }
            }

            return true;
        }, IntPtr.Zero);
    }

    private static string GetWindowTitle(IntPtr hWnd)
    {
        const int nChars = 256;
        StringBuilder windowTitle = new StringBuilder(nChars);
        if (GetWindowText(hWnd, windowTitle, nChars) > 0)
        {
            return windowTitle.ToString();
        }
        return null;
    }

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
}
