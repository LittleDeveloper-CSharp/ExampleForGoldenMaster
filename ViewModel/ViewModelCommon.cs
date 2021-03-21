using ExampleForGoldenMaster.Model;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace ExampleForGoldenMaster.ViewModel
{
    /// <summary>
    /// Это класс нужен для хранения и передачи значений свойст (в частности услугу) между другими классами
    /// Так же для реализации интерфейса (INotifyPropertyChanged) на измение свойст 
    /// А ну и для единственного контекста данных)))))
    /// </summary>
    class ViewModelCommon : INotifyPropertyChanged
    {
        private static Service service;
        protected Service Service { get => service; set => service = value; }
        public ViewModelCommon()
        {
            model.Services.Load();
        }

        protected static readonly ContextModel model = new ContextModel();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name = "") 
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}