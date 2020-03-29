using System.Windows;

namespace WpfProject
{
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new ApplicationViewModel();
        }
    }
}