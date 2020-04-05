using System.Windows;
using System.Windows.Controls;

namespace WpfProject
{
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new ApplicationViewModel();
        }

        // Asimple event to empty the search bar when selected.
        private void TextBox_Gotfocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox) sender;
            box.Text = string.Empty;
        }

        
    }
}