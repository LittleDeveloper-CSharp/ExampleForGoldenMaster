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
    /// <summary>
    /// Класс, который передается в DataContext MainWindow
    /// Типа пасхалка : https://cdn.idaprikol.ru/images/09ad214b47d3d119c0857946da4ff87665c6d511930ad9bc2be3b744dc986885_1.jpg
    /// </summary>
    class ViewModelMainWindow : ViewModelCommon
    {
        private ObservableCollection<Service> services;
        private Visibility isAdmin = Visibility.Hidden;
        private Discount discount;
        private string name = "";
        private string password = "";

        /// <summary>
        /// Команды для кнопок
        /// </summary>
        private CommonCommand addCommand;
        private CommonCommand editCommand;
        private CommonCommand deleteCommand;
        private CommonCommand backCommand;

        /// <summary>
        /// Когда пароль правильный, открывается панель одмена
        /// </summary>
        public Visibility IsAdmin { get => isAdmin; 
            set 
            { 
                isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            } 
        }

        /// <summary>
        /// Когда ты нажимаешь на элемент ListView, то срабатывает это свойство 
        /// </summary>
        public Service GetWriteOrder
        {
            set 
            {
                //А вот и передача в родительский класс свойство
                Service = value;
                Views.AddOrderWindow window = new Views.AddOrderWindow();
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Простая реализация команды для кнопки выключения режима админа
        /// </summary>
        public CommonCommand BackCommand 
        {
            get
            {
                if (backCommand is null)
                {
                    backCommand = new CommonCommand(obj => 
                    {
                        IsAdmin = Visibility.Hidden;
                        GetPassword = "";
                        OnPropertyChanged(nameof(GetServices));
                    });
                }
                return backCommand;
            }
        }

        /// <summary>
        /// Команда для редактирования
        /// </summary>
        public CommonCommand EditCommand 
        { 
            get 
            {
                if (editCommand is null)
                    editCommand = new CommonCommand(obj => 
                    {
                        // А тут самое интересное, короче, тут мы получаем то к чему у нас относиться объект, то есть контейнер
                        var content = obj as ContentPresenter;
                        var item = content.DataContext as Service;
                        // Опять передача в родительский)
                        Service = item;
                        Views.EditAndAddWindow window = new Views.EditAndAddWindow();
                        window.ShowDialog();
                    });
                return editCommand;
            } 
        }

        /// <summary>
        /// Команда на удаление объекта
        /// </summary>
        public CommonCommand DeleteCommand 
        { 
            get
            {
                if (deleteCommand is null)
                    deleteCommand = new CommonCommand(obj =>
                    {
                        if (MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            //Та же схеме, что и в предыдущем
                            var content = obj as ContentPresenter;
                            var item = content.DataContext as Service;
                            //А вот и использование модели из родительского класса, так как у нас модель помеча static, то эта фигня у нас в единственным экземпляре)
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
        /// <summary>
        /// Команда на добавление объекта
        /// </summary>
        public CommonCommand AddCommand
        {
            get
            {
                if (addCommand is null)
                    addCommand = new CommonCommand(obj => 
                    {
                        //Тут просто новый объект передаём
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
            //Типа класс скидка, с мыслью, а почему бы и нет (Мне так было проще) ))))
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

        /// <summary>
        /// Все, что ниже мне лень расписывать)
        /// </summary>
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

        /// <summary>
        /// Все кроме этого, здесь получается мы просто фильтруем)
        /// </summary>
        private void FilterServices()
        {
            var item = model.Services.Local.ToList();
            
            if(!string.IsNullOrEmpty(name))
                item = item.Where(x => x.Title.Contains(name)).ToList();

            if (discount.maxValue != 0)
                item = item.Where(x => discount.minValue <= x.Discount && x.Discount < discount.maxValue).ToList();

            GetServices = new ObservableCollection<Service>(item);
        }

        /// <summary>
        /// А тут мы просто выводи инфу (опять)
        /// </summary>
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