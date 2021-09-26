using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using ActivityTracker.Models;
using ActivityTracker.Repository;

namespace ActivityTracker.Engine
{
    public static class ProcessService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        private static DataRepository s_dataRepository =
            DataRepository.GetInstance();

        public static Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();

            GetWindowThreadProcessId(hwnd, out uint pid);

            return Process.GetProcessById((int)pid);
        }

        public static void RecordProgramFocusSwitch()
        {
            using Process process = GetActiveProcess();

            if(process == null)
            {
                EndCurrentProgramFocus();
                return;
            }

            // Don't add an entry if we are "switching" to the same process
            // or switching to a program with no title (window)
            if (s_dataRepository.LogEntries.LastOrDefault()?.ProcessId == process.Id ||
               string.IsNullOrWhiteSpace(process.MainWindowTitle))
            {
                return;
            }

            EndCurrentProgramFocus();

            string displayName = GetProcessDisplayName(process);

            s_dataRepository.LogEntries.Add(new LogEntry(process.Id, process.ProcessName, process.MainWindowTitle, displayName));
        }

        private static void EndCurrentProgramFocus()
        {
            s_dataRepository.LogEntries.LastOrDefault()?.EndProgramFocus();
        }

        private static string GetProcessDisplayName(Process process)
        {
            string displayName = 
                s_dataRepository.GetProgramByProcessName(process.ProcessName)?.DisplayName ?? "";

            return string.IsNullOrWhiteSpace(displayName) ? process.ProcessName : displayName;
        }
    }
}