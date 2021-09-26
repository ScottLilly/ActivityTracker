using System.Collections.ObjectModel;
using ActivityTracker.Engine;
using ActivityTracker.Models;
using ActivityTracker.Repository;

namespace ActivityTracker.ViewModels
{
    public class ProgramFocusSwitchLog
    {
        private DataRepository _dataRepository =
            DataRepository.GetInstance();

        public ObservableCollection<LogEntry> LogEntries =>
            _dataRepository.LogEntries;

        public void RecordProgramFocusSwitch()
        {
            ProcessService.RecordProgramFocusSwitch();
        }
    }
}