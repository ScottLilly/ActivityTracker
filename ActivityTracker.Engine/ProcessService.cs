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

            // This happens if a focused program is minimized or closed
            if (process == null ||
               (process.ProcessName.Equals("explorer", StringComparison.InvariantCulture) &&
                string.IsNullOrWhiteSpace(process.MainWindowTitle)))
            {
                EndCurrentProgramFocus();
                return;
            }

            // This happens when a non-focused program is minimized or closed
            if (string.IsNullOrWhiteSpace(process.MainWindowTitle))
            {
                return;
            }

            // Don't add a new log entry if we are "switching" to the same process/program,
            // unless the window title changed (a new tab/option was selected in the program)
            if (ProcessMatchesCurrentProgramWithFocus(process))
            {
                return;
            }

            AddNewLogEntry(process);
        }

        private static void EndCurrentProgramFocus()
        {
            s_dataRepository.LogEntries.LastOrDefault()?.EndProgramFocus();
        }

        private static bool ProcessMatchesCurrentProgramWithFocus(Process process)
        {
            LogEntry lastLogEntry = s_dataRepository.LogEntries.LastOrDefault();

            return lastLogEntry?.ProcessId == process.Id &&
                lastLogEntry?.ApplicationName == process.ProcessName &&
                lastLogEntry?.WindowTitle == process.MainWindowTitle &&
                lastLogEntry?.EndTicks == 0;
        }

        private static void AddNewLogEntry(Process process)
        {
            EndCurrentProgramFocus();

            string displayName = GetProcessDisplayName(process);

            s_dataRepository.LogEntries.Add(new LogEntry(process.Id, process.ProcessName, process.MainWindowTitle, displayName));
        }

        private static string GetProcessDisplayName(Process process)
        {
            string displayName = 
                s_dataRepository.GetProgramByProcessName(process.ProcessName)?.DisplayName ?? "";

            return string.IsNullOrWhiteSpace(displayName) ? process.ProcessName : displayName;
        }
    }
}