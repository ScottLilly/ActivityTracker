using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ActivityTracker.Models
{
    public class TaskSwitch : INotifyPropertyChanged
    {
        private long endTicks;

        public Process Process { get; }
        public long StartTicks { get; }
        public long EndTicks
        {
            get => endTicks;
            private set
            {
                endTicks = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public TimeSpan? Duration => EndTicks == 0 ? null : new TimeSpan(EndTicks - StartTicks);


        public event PropertyChangedEventHandler PropertyChanged;

        public TaskSwitch(Process process)
        {
            Process = process;
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