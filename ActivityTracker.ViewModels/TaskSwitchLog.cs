using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ActivityTracker.Engine;
using ActivityTracker.Models;

namespace ActivityTracker.ViewModels
{
    public class TaskSwitchLog
    {
        public ObservableCollection<TaskSwitch> TaskSwitchLogEntries { get; } =
            new ObservableCollection<TaskSwitch>();

        private TaskSwitch LatestTaskSwitch => TaskSwitchLogEntries.LastOrDefault();

        public void RecordTaskSwitch()
        {
            Process process = ProcessService.GetActiveProcess();

            // Don't add an entry if we are "switching" to the same process
            // or switching to a program with no title (window)
            if (LatestTaskSwitch?.Process.Id == process.Id ||
                string.IsNullOrWhiteSpace(process.MainWindowTitle))
            {
                return;
            }

            LatestTaskSwitch?.EndTaskFocus();

            TaskSwitchLogEntries.Add(new TaskSwitch(process));
        }
    }
}