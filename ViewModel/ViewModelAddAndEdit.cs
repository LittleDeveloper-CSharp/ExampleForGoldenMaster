using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExampleForGoldenMaster.ViewModel
{
    class ViewModelAddAndEdit : ViewModelCommon
    {
        public ViewModelAddAndEdit()
        {
            MessageBox.Show(Service.Title);
        }
    }
}
