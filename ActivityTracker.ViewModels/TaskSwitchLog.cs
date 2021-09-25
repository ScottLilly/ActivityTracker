using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ActivityTracker.Engine;
using ActivityTracker.Models;
using ActivityTracker.Repository;

namespace ActivityTracker.ViewModels
{
    public class TaskSwitchLog
    {
        private readonly DataRepository _dataRepository =
            DataRepository.GetInstance();

        public ObservableCollection<LogEntry> LogEntries { get; } =
            new ObservableCollection<LogEntry>();

        private LogEntry LatestFocusSwitch =>
            LogEntries.LastOrDefault();

        public void RecordTaskSwitch()
        {
            using Process process = ProcessService.GetActiveProcess();

            if(process == null)
            {
                EndCurrentProgramFocus();
                return;
            }

            // Don't record time for the ActivityTracker app
            // TODO: Maybe make this a configurable setting
#if DEBUG
            if(process.Id == Environment.ProcessId)
            {
                return;
            }
#endif

            // Don't add an entry if we are "switching" to the same process
            // or switching to a program with no title (window)
            if(LatestFocusSwitch?.ProcessId == process.Id ||
               string.IsNullOrWhiteSpace(process.MainWindowTitle))
            {
                return;
            }

            EndCurrentProgramFocus();

            LogEntries.Add(new LogEntry(process.Id, process.ProcessName, process.MainWindowTitle, GetProcessDisplayName(process)));
        }

        public void EndCurrentProgramFocus()
        {
            LatestFocusSwitch?.EndProgramFocus();
        }

        private string GetProcessDisplayName(Process process)
        {
            Program program =
                _dataRepository.Programs.FirstOrDefault(p => p.ProcessName.Equals(process.ProcessName, StringComparison.OrdinalIgnoreCase));

            return program?.DisplayName ?? process.ProcessName;
        }
    }
}