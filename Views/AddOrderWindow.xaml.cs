using System.Windows;

namespace ExampleForGoldenMaster.Views
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.ViewModelAddOrder();
        }
    }
}
