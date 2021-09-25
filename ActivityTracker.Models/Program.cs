namespace ActivityTracker.Models
{
    public class Program
    {
        public string ProcessName { get; set; }
        public string DisplayName { get; set; }
        public bool ShouldLog { get; set; }

        public Program(string processName, string displayName, bool shouldLog = true)
        {
            ProcessName = processName;
            DisplayName = displayName;
            ShouldLog = shouldLog;
        }
    }
}