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

        private void TextBox_Gotfocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox) sender;
            box.Text = string.Empty;
            //box.GotFocus -= TextBox_Gotfocus; 
            //throw new System.NotImplementedException();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            /*TextBox box = (TextBox) sender;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = "Enter Pokemons Name";
            }
            box.LostFocus += TextBox_LostFocus;*/
            //throw new System.NotImplementedException();
        }
    }
}