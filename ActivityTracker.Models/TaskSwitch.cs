using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ActivityTracker.Models
{
    public class TaskSwitch : INotifyPropertyChanged
    {
        private long _endTicks;

        public int ProcessId { get; }
        public string ApplicationName { get; }
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

        public TaskSwitch(int processId, string applicationName, string windowTitle)
        {
            ProcessId = processId;
            ApplicationName = applicationName;
            WindowTitle = windowTitle;

            StartTicks = DateTime.UtcNow.Ticks;
        }

        public void EndTaskFocus()
        {
            if(EndTicks == 0)
            {
                EndTicks = DateTime.UtcNow.Ticks;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}