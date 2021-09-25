using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using ActivityTracker.ViewModels;

namespace ActivityTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new TaskSwitchLog();

            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);
        }

        private void OnFocusChangedHandler(object sender, AutomationFocusChangedEventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                (DataContext as TaskSwitchLog).RecordTaskSwitch();
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                (DataContext as TaskSwitchLog).EndCurrentTask();
            });

            // TODO: Add entries to "database"

            base.OnClosing(e);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}