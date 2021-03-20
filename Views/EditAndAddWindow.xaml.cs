using ExampleForGoldenMaster.ViewModel;
using System.Windows;

namespace ExampleForGoldenMaster.Views
{
    /// <summary>
    /// Логика взаимодействия для EditAndAddWindow.xaml
    /// </summary>
    public partial class EditAndAddWindow : Window
    {
        public EditAndAddWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelAddAndEdit();
        }
    }
}
