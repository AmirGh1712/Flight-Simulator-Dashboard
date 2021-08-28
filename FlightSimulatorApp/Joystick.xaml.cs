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
using System.Windows.Media.Animation;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        private bool isTracking;
        private Storyboard s;
        public double X { get; private set; }
        public double Y { get; private set; }
        //Event when the joystick moves.
        public event EventHandler JoystickMove;
        //Constructor.
        public Joystick()
        {
            InitializeComponent();
            this.DataContext = this;
            isTracking = false;
            s = (Storyboard)Knob.TryFindResource("CenterKnob");
            X = 0;
            Y = 0;
            if (JoystickMove != null)
            {
                JoystickMove(this, EventArgs.Empty);
            }
        }
        //Stop the animation when it's done.
        private void centerKnob_Completed(object sender, EventArgs e)
        {
            s.Stop();
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }

        //Start Capturing the mouse.
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isTracking = true;
            (Knob).CaptureMouse();
        }

        //Move the joystick according to the mouse.
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            double radius = Base.Width / 2 - KnobBase.Width / 2;
            Point pos = new Point();
            Point mousePosBase = e.GetPosition(Base);
            Point mousePos = new Point(mousePosBase.X - Base.Width / 2, mousePosBase.Y - Base.Height / 2);
            if (distance(mousePos, new Point()) > radius)
            {
                double angle = Math.Atan(mousePos.Y / mousePos.X);
                if (mousePos.X < 0)
                {
                    angle -= Math.PI;
                }
                pos.Y = radius * Math.Sin(angle);
                pos.X = radius * Math.Cos(angle);
            }
            else
            {
                pos = mousePos;
            }

            if (isTracking)
            {
                knobPosition.X = pos.X;
                knobPosition.Y = pos.Y;
                X = pos.X / radius;
                Y = -pos.Y / radius;
                if (JoystickMove != null)
                {
                    JoystickMove(this, EventArgs.Empty);
                }
            }
        }
        //Release the mouse and move the joystick to 0,0.as
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isTracking = false;
            (Knob).ReleaseMouseCapture();
            X = 0;
            Y = 0;
            if (JoystickMove != null)
            {
                JoystickMove(this, EventArgs.Empty);
            }
            s.Begin();
        }
        //Calculate distance between two points.
        private double distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
