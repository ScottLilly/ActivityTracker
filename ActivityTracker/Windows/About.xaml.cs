using System.Windows;

namespace ActivityTracker.Windows
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}