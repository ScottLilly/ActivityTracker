using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using ActivityTracker.ViewModels;
using ActivityTracker.Windows;

namespace ActivityTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ProgramFocusSwitchLog();

            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);
        }

        private void OnFocusChangedHandler(object sender, AutomationFocusChangedEventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                (DataContext as ProgramFocusSwitchLog).RecordProgramFocusSwitch();
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                (DataContext as ProgramFocusSwitchLog).RecordProgramFocusSwitch();
            });

            // TODO: Add entries to "database"

            base.OnClosing(e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.Owner = this;
            help.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
        }
    }
}