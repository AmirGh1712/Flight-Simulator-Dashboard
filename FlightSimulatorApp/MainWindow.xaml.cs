using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = (Application.Current as App).MainViewModel;
        }

        private void ControllersPanel_ValueChanged(object sender, EventArgs e)
        {
            (Application.Current as App).MainViewModel.Update(Panel.JoystickX, Panel.JoystickY, Panel.HorizontalSlider, Panel.VerticalSlider);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (Application.Current as App).MainViewModel.Cancel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).CVM.Reconnect(IP.Text, Port.Text);
        }
    }
}
