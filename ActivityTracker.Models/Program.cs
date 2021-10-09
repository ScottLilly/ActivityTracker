namespace ActivityTracker.Models
{
    public class Program
    {
        public string ProcessName { get; }
        public string DisplayName { get; }
        public bool ShouldLog { get; }

        public Program(string processName, string displayName, bool shouldLog = true)
        {
            ProcessName = processName;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? processName : displayName;
            ShouldLog = shouldLog;
        }
    }
}