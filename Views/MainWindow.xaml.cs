using System.Windows;


namespace ExampleForGoldenMaster.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext =  new ViewModel.ViewModelMainWindow();
        }
    }
}
