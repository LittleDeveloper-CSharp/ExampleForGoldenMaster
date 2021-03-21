using ExampleForGoldenMaster.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleForGoldenMaster.ViewModel
{
    class ViewModelAddOrder : ViewModelCommon
    {
        private Service service;
        private ClientService clientService;
        private Client client;
        private Command.CommonCommand addOrderCommand;
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
                        model.ClientServices.Add(clientService);
                        model.SaveChanges();
                        
                    });
                }
                return addOrderCommand;
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
        
        public ClientService GetClientService
        {
            get => clientService;
            set
            {
                clientService = value;
                OnPropertyChanged(nameof(GetClientService));
            }
        }


        public ViewModelAddOrder()
        {
            GetService = Service;
            GetClient = GetClients[0];
            GetClientService = new ClientService();
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

        private void CancatTimeWithDate()
        {
            var valueTime = time.Split(':', '.').Select(x => int.Parse(x)).ToArray();
            if (valueTime.Length == 2)
            {
                DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, valueTime[0], valueTime[1], 0);
                clientService.StartTime = dateTime;
            }
        }
    }
}