using ExampleForGoldenMaster.Model;
using System.Linq;
using System.Windows;

namespace ExampleForGoldenMaster.ViewModel
{
    class ViewModelAddAndEdit : ViewModelCommon
    {
        private Command.CommonCommand addCommand;
        private Command.CommonCommand backCommand;
        private Service getService;

        public Command.CommonCommand AddCommand
        {
            get
            {
                if (addCommand is null)
                    addCommand = new Command.CommonCommand(x =>
                    {
                        var items = model.Services.Where<Service>(s => s.Title == GetService.Title).Any<Service>();

                        if (GetService.ID == 0)
                        {
                            if (!items)
                                model.Services.Add(GetService);
                            else
                                MessageBox.Show("Данная услуга уже есть");
                        }
                        else
                            model.Entry(GetService).State = System.Data.Entity.EntityState.Modified;
                        model.SaveChanges();
                        OnPropertyChanged("GetServices");
                    });
                return addCommand;
            }
        }

        public Command.CommonCommand BackCommand
        {
            get
            {
                if (backCommand is null)
                {
                    backCommand = new Command.CommonCommand(x =>
                    {

                    });
                }
                return backCommand;
            }
        }

        public Model.Service GetService { get => getService; 
            set 
            { 
                getService = value;
                OnPropertyChanged(nameof(GetService));
            }
        }

        public ViewModelAddAndEdit()
        {
            GetService = Service;
        }
    }
}
