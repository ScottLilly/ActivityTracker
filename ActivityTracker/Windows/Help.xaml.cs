using System.Windows;

namespace ActivityTracker.Windows
{
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}