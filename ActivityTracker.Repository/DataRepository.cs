using System;
using System.Collections.ObjectModel;
using System.Linq;
using ActivityTracker.Models;
using Microsoft.Extensions.Configuration;

namespace ActivityTracker.Repository
{
    public class DataRepository
    {
        private static IConfiguration _configuration;

        private static readonly DataRepository s_dataRepository =
            new DataRepository();

        public ObservableCollection<LogEntry> LogEntries { get; } =
            new ObservableCollection<LogEntry>();

        private ObservableCollection<Program> Programs { get; } =
            new ObservableCollection<Program>();

        private DataRepository()
        {
            IConfigurationBuilder configurationBuilder =
                new ConfigurationBuilder().AddJsonFile("Configuration.json");

            _configuration = configurationBuilder.Build();

            LoadPrograms();
        }

        public static DataRepository GetInstance()
        {
            return s_dataRepository;
        }

        public Program GetProgramByProcessName(string processName)
        {
            return Programs.FirstOrDefault(p => p.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase));
        }

        private void LoadPrograms()
        {
            Programs.Clear();

            IConfigurationSection programsSection =
                _configuration.GetChildren().FirstOrDefault(s => s.Key.Equals("Programs"));

            if (programsSection == null)
            {
                return;
            }

            foreach (IConfigurationSection program in programsSection.GetChildren())
            {
                string processName = program["ProcessName"] ?? "";
                string displayName = program["DisplayName"] ?? "";
                bool shouldLog = Convert.ToBoolean(program["ShouldLog"] ?? "true");

                if (!string.IsNullOrWhiteSpace(processName))
                {
                    Programs.Add(new Program(processName, displayName, shouldLog));
                }
            }
        }
    }
}