using ExampleForGoldenMaster.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ExampleForGoldenMaster.ViewModel.Command;
using System.Data.Entity;
using System.Windows.Controls;

namespace ExampleForGoldenMaster.ViewModel
{
    class ViewModelMainWindow : ViewModelCommon
    {
        private ObservableCollection<Service> services;
        private Visibility isAdmin = Visibility.Hidden;
        private Discount discount;
        private string name = "";
        private string password = "";

        private CommonCommand addCommand;
        private CommonCommand editCommand;
        private CommonCommand deleteCommand;
        private CommonCommand backCommand;

        public Visibility IsAdmin { get => isAdmin; 
            set 
            { 
                isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            } 
        }

        public Service GetWriteOrder
        {
            set 
            {
                Service = value;
                Views.AddOrderWindow window = new Views.AddOrderWindow();
                window.ShowDialog();
            }
        }

        public CommonCommand BackCommand 
        {
            get
            {
                if (backCommand is null)
                {
                    backCommand = new CommonCommand(obj => 
                    {
                        IsAdmin = Visibility.Hidden;
                        OnPropertyChanged(nameof(GetServices));
                    });
                }
                return backCommand;
            }
        }

        public CommonCommand EditCommand 
        { 
            get 
            {
                if (editCommand is null)
                    editCommand = new CommonCommand(obj => 
                    {
                        var content = obj as ContentPresenter;
                        var item = content.DataContext as Service;
                        Service = item;
                        Views.EditAndAddWindow window = new Views.EditAndAddWindow();
                        window.ShowDialog();
                    });
                return editCommand;
            } 
        }

        public CommonCommand DeleteCommand 
        { 
            get
            {
                if (deleteCommand is null)
                    deleteCommand = new CommonCommand(obj =>
                    {
                        if (MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            var content = obj as ContentPresenter;
                            var item = content.DataContext as Service;
                            model.ClientServices.Load();
                            if (!model.ClientServices.Local.Where(x => x.ServiceID == item.ID).Any())
                            {
                                model.Services.Remove(item);
                                model.SaveChanges();
                            }
                            else
                                MessageBox.Show("На данную услугу сущетсвует запись");
                        }
                    });
                return deleteCommand;
            } 
        }
        public CommonCommand AddCommand
        {
            get
            {
                if (addCommand is null)
                    addCommand = new CommonCommand(obj => 
                    {
                        Service = new Service();
                        Views.EditAndAddWindow window = new Views.EditAndAddWindow();
                        window.ShowDialog();
                    });
                return addCommand;
            }
        }

        public ViewModelMainWindow()
        {
            GetServices = model.Services.Local;
            Discounts = new List<Discount>
            {
                new Discount{NameValue = "Все", minValue = 0, maxValue = 0},
                new Discount{NameValue = "от 0 до 5%", minValue = 0, maxValue = 5},
                new Discount{NameValue = "от 5 до 15%", minValue = 5, maxValue = 15},
                new Discount{NameValue = "от 30 до 70%", minValue = 30, maxValue = 70},
                new Discount{NameValue = "от 70 до 100%", minValue = 70, maxValue = 100},
            };
            discount = Discounts[0];
        }

        public List<Discount> Discounts { get; set; }
        
        public string GetName 
        {
            get => name; 
            set 
            {
                name = value;
                FilterServices();
            }
        }

        public string GetPassword
        {
            get => password;
            set
            {
                if (value == "0000")
                {
                    IsAdmin = Visibility.Visible;
                    OnPropertyChanged(nameof(GetServices));
                }
                password = value;
            }
        }

        public Discount GetDiscount 
        {
            get => discount; 
            set 
            {
                discount = value; 
                FilterServices();
            }
        }

        private void FilterServices()
        {
            var item = model.Services.Local.ToList();
            
            if(!string.IsNullOrEmpty(name))
                item = item.Where(x => x.Title.Contains(name)).ToList();

            if (discount.maxValue != 0)
                item = item.Where(x => discount.minValue <= x.Discount && x.Discount < discount.maxValue).ToList();

            GetServices = new ObservableCollection<Service>(item);
        }

        public ObservableCollection<Service> GetServices 
        {
            get 
            {
                foreach (var item in services)
                {                    
                    item.VisibilityButtonForEdit = isAdmin;
                    item.HaveDiscont = item.Discount != 0 ? Visibility.Visible : Visibility.Hidden;
                    item.TotalCost = (int)(item.Cost - (item.Cost * (decimal)item.Discount / 100));
                    item.DurationInMinutes = item.DurationInSeconds / 60;
                    item.ColorBackground = item.Discount != 0 ? "LightGreen" : "Transponent";
                }
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