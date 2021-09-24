using ActivityTracker.ViewModels;
using System;
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
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent,
                AutomationElement.RootElement, TreeScope.Children, OnWindowOpened);
        }

        private void OnFocusChangedHandler(object sender, AutomationFocusChangedEventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                (DataContext as TaskSwitchLog).RecordTaskSwitch();
            });
        }


        private static void OnWindowOpened(object sender, AutomationEventArgs automationEventArgs)
        {
            try
            {
                var element = sender as AutomationElement;
                if (element != null)
                {
                    Console.WriteLine("New Window opened: {0}", element.Current.Name);
                }
            }
            catch (ElementNotAvailableException)
            {
            }
        }
    }
}