using ExampleForGoldenMaster.Model;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace ExampleForGoldenMaster.ViewModel
{
    class ViewModelCommon : INotifyPropertyChanged
    {
        private static Service service;
        protected Service Service { get => service; set => service = value; }
        public ViewModelCommon()
        {
            model.Services.Load();
        }

        protected readonly ContextModel model = new ContextModel();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name = "") 
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}