using ActivityTracker.ViewModels;
using System.Windows;
using System.Windows.Automation;

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
    }
}