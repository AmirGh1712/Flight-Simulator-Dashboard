using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ViewModel MainViewModel { get; internal set; }
        public ConnectVM CVM { get; internal set; }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            Airplane model = new Airplane();
            MainViewModel = new ViewModel(model);
            CVM = new ConnectVM(model);
        }
    }
}
