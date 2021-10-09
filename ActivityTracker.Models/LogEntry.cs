using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ActivityTracker.Models
{
    public sealed class LogEntry : INotifyPropertyChanged
    {
        private long _endTicks;
        private string _displayName;

        public int ProcessId { get; }
        public string ApplicationName { get; }
        public string DisplayName
        {
            get => _displayName;
            private set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }
        public string WindowTitle { get; }
        public long StartTicks { get; }
        public long EndTicks
        {
            get => _endTicks;
            private set
            {
                _endTicks = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public TimeSpan? Duration => EndTicks == 0 ? null : new TimeSpan(EndTicks - StartTicks);

        public event PropertyChangedEventHandler PropertyChanged;

        public LogEntry(int processId, string processName, string windowTitle, string displayName = "")
        {
            ProcessId = processId;
            ApplicationName = processName;
            WindowTitle = windowTitle;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? processName : displayName;

            StartTicks = DateTime.UtcNow.Ticks;
        }

        public void EndProgramFocus()
        {
            if(EndTicks == 0)
            {
                EndTicks = DateTime.UtcNow.Ticks;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}