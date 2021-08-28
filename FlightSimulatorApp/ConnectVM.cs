using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class ConnectVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        public Airplane Model { get; private set; }
        //Constructor.
        public ConnectVM(Airplane theModel)
        {
            Model = theModel;
        }
        //Reconnect the model to a server according to the parameters.
        public void Reconnect(string ip, string port)
        {
            Model.Reconnect(ip, port);
        }
    }
}
