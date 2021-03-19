using System;
using System.Linq;
using System.Windows;
using ExampleForGoldenMaster.Model;
using System.Collections.ObjectModel;

namespace ExampleForGoldenMaster.ViewModel
{
    class ViewModelMainWindow : ViewModelCommon
    {
        private ObservableCollection<Service> services;
        private Visibility isAdmin = Visibility.Hidden;
        public Visibility IsAdmin { get => isAdmin; 
            set
            {
                isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            } 
        }

        public ViewModelMainWindow()
        {
            GetServices = model.Services.Local;
        }

        public ObservableCollection<Service> GetServices 
        {
            get 
            {
                services.Select(x => x.FullImagePath = Environment.CurrentDirectory + x.MainImagePath);
                services.Select(x => x.HaveDiscont = x.Discount != 0 ? Visibility.Visible : Visibility.Hidden);
                services.Select(x => x.TotalCost = (int)(x.Cost - (x.Cost * (decimal)x.Discount)));
                return services;
            }
            
            set 
            {
                services = value;
                OnPropertyChanged(nameof(GetServices));
            } 
        }
    }
}