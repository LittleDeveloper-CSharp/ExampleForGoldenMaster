using ExampleForGoldenMaster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExampleForGoldenMaster.ViewModel
{
    /// <summary>
    /// А тут я просто разбил на свойста объект и при добавлении в базу собираю в один
    /// </summary>
    class ViewModelAddOrder : ViewModelCommon
    {
        private Service service;
        private Client client;
        private Command.CommonCommand addOrderCommand;
        private Command.CommonCommand backCommand;
        private DateTime date = DateTime.Parse(DateTime.Now.ToShortDateString());
        private string time = "00:00";

        public Command.CommonCommand AddOrderCommand
        {
            get
            {
                if(addOrderCommand == null)
                {
                    addOrderCommand = new Command.CommonCommand(x =>
                    {
                        clientService.Service = service;
                        clientService.Client = client;
                        Application.Current.Windows[2].Close(); // Эта фигня была найдена в инете, работает только с 2
                        model.SaveChanges();
                    });
                }
                return addOrderCommand;
            }
        }

        public Command.CommonCommand BackCommand
        {
            get
            {
                if(backCommand == null)
                {
                    backCommand = new Command.CommonCommand(x =>
                    {
                        Application.Current.Windows[2].Close(); // Эта фигня была найдена в инете, работает только с 2 (типа у нас новое окно идет под номером 2)
                    });
                }
                return backCommand;
            }
        }

        public Service GetService 
        {
            get => service;
            set 
            {
                service = value;
                OnPropertyChanged(nameof(GetService));
            } 
        }

        private ClientService clientService;


        public ViewModelAddOrder()
        {
            GetService = Service;
            GetClient = GetClients[0];
            clientService = new ClientService();
            model.Entry(clientService).State = System.Data.Entity.EntityState.Added;
        }

        public List<Client> GetClients { get => model.Clients.ToList(); } 
        
        public Client GetClient
        {
            get => client;
            set
            {
                client = value;
                OnPropertyChanged(nameof(GetClient));
            }
        }

        public DateTime GetDateTime 
        {
            get => date; 
            set 
            {
                date = value;
                CancatTimeWithDate();
            }
        }

        public string GetTime 
        { 
            get => time;
            set
            {
                time = value;
                CancatTimeWithDate();
            }
        }

        private string endTime;
        public string EndTime
        {
            get => endTime;
            set
            {
                endTime = value;
                OnPropertyChanged();
            }
        }

        //Какая-то хитрая штука
        private void CancatTimeWithDate()
        {
            var valueTime = time.Split(':', '.').Select(x => int.Parse(x)).ToArray();
            if (valueTime.Length == 2)
            {
                DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, valueTime[0], valueTime[1], 0);
                clientService.StartTime = dateTime;
                dateTime = dateTime.AddMinutes(service.DurationInMinutes);
                EndTime = $"{dateTime.Hour}:{dateTime.Minute}";
            }
        }
    }
}