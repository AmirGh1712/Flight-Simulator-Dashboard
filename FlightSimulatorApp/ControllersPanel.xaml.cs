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
    /// Interaction logic for ControllersPanel.xaml
    /// </summary>
    public partial class ControllersPanel : UserControl
    {
        //Event when one of the controls change.
        public event EventHandler ValueChanged;
        public double JoystickX { get; private set; }
        public double JoystickY { get; private set; }
        public double HorizontalSlider { get; private set; }
        public double VerticalSlider { get; private set; }
        //Constructor.
        public ControllersPanel()
        {
            InitializeComponent();
            this.DataContext = this;
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
        //Notify When the joystick moves.
        private void Joystick_JoystickMove(object sender, EventArgs e)
        {
            JoystickX = Math.Round(Stick.X, 2);
            JoystickY = Math.Round(Stick.Y, 2);
            joy_lbl.Content = string.Format("Elevator: {0} Rudder: {1}", JoystickY, JoystickX);
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
        //Notify When the slider moves.
        private void Slider_Vertical_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VerticalSlider = Math.Round(VertSld.Value, 2);
            th_lbl.Content = string.Format("Throttle: {0}", VerticalSlider);
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
        //Notify When the slider moves.
        private void Slider_Horizontal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HorizontalSlider = Math.Round(HorSld.Value, 2);
            ail_lbl.Content = string.Format("Aileron: {0}", HorizontalSlider);
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
    }
}
