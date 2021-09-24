using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ActivityTracker.Models
{
    public class TaskSwitch : INotifyPropertyChanged
    {
        private readonly Process _process;
        private long _endTicks;

        public int Id => _process.Id;
        public string ApplicationName => _process.ProcessName;
        public string WindowTitle => _process.MainWindowTitle;
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

        public TaskSwitch(Process process)
        {
            _process = process;
            StartTicks = DateTime.UtcNow.Ticks;
        }

        public void EndTaskFocus()
        {
            EndTicks = DateTime.UtcNow.Ticks;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}