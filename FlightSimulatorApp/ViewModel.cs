using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Maps.MapControl;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        public Airplane Model { get; private set; }

        private string _IndicatedHeadingDeg;
        public string IndicatedHeadingDeg
        {
            get => _IndicatedHeadingDeg;
            private set
            {
                _IndicatedHeadingDeg = value;
                NotifyPropertyChanged(nameof(IndicatedHeadingDeg));
            }
        }

        private string _GPSIndicatedVerticalSpeed;
        public string GPSIndicatedVerticalSpeed
        {
            get => _GPSIndicatedVerticalSpeed;
            private set
            {
                _GPSIndicatedVerticalSpeed = value;
                NotifyPropertyChanged(nameof(GPSIndicatedVerticalSpeed));
            }
        }

        private string _GPSIndicatedGroundSpeedKt;
        public string GPSIndicatedGroundSpeedKt
        {
            get => _GPSIndicatedGroundSpeedKt;
            private set
            {
                _GPSIndicatedGroundSpeedKt = value;
                NotifyPropertyChanged(nameof(GPSIndicatedGroundSpeedKt));
            }
        }

        private string _AirspeedIndicatorIndicatedSpeedKt;
        public string AirspeedIndicatorIndicatedSpeedKt
        {
            get => _AirspeedIndicatorIndicatedSpeedKt;
            private set
            {
                _AirspeedIndicatorIndicatedSpeedKt = value;
                NotifyPropertyChanged(nameof(AirspeedIndicatorIndicatedSpeedKt));
            }
        }

        private string _GPSIndicatedAltitudeFt;
        public string GPSIndicatedAltitudeFt
        {
            get => _GPSIndicatedAltitudeFt;
            private set
            {
                _GPSIndicatedAltitudeFt = value;
                NotifyPropertyChanged(nameof(GPSIndicatedAltitudeFt));
            }
        }

        private string _AttitudeIndicatorInternalRollDeg;
        public string AttitudeIndicatorInternalRollDeg
        {
            get => _AttitudeIndicatorInternalRollDeg;
            private set
            {
                _AttitudeIndicatorInternalRollDeg = value;
                NotifyPropertyChanged(nameof(AttitudeIndicatorInternalRollDeg));
            }
        }

        private string _AttitudeIndicatorInternalPitchDeg;
        public string AttitudeIndicatorInternalPitchDeg
        {
            get => _AttitudeIndicatorInternalPitchDeg;
            private set
            {
                _AttitudeIndicatorInternalPitchDeg = value;
                NotifyPropertyChanged(nameof(AttitudeIndicatorInternalPitchDeg));
            }
        }

        private string _AltimeterIndicatedAltitudeFt;
        public string AltimeterIndicatedAltitudeFt
        {
            get => _AltimeterIndicatedAltitudeFt;
            private set
            {
                _AltimeterIndicatedAltitudeFt = value;
                NotifyPropertyChanged(nameof(AltimeterIndicatedAltitudeFt));
            }
        }
        private string _Error;
        public string Error
        {
            get => _Error;
            private set
            {
                _Error = value;
                NotifyPropertyChanged(nameof(Error));
            }
        }
        private Location _Pos;
        public Location Pos
        {
            get => _Pos;
            private set
            {
                _Pos = value;
                NotifyPropertyChanged(nameof(Pos));
            }
        }

        private string _Visible;
        public string Visible
        {
            get => _Visible;
            private set
            {
                _Visible = value;
                NotifyPropertyChanged(nameof(Visible));
            }
        }
        private void SetPos(double latitude, double longitude)
        {
            Pos.Longitude = longitude;
            Pos.Latitude = latitude;
                NotifyPropertyChanged(nameof(Pos));
        }
        public string IP { get; set; }
        public string Port { get; set; }
        //Constructor.
        public ViewModel(Airplane model)
        {
            Model = model;
            model.PropertyChanged += Model_PropertyChanged;
            IP = ConfigurationManager.AppSettings.Get("IP");
            Port = ConfigurationManager.AppSettings.Get("Port");
            Pos = new Location(0, 0);
            Visible = "Hidden";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        //Change the VM when the models' propertys change;
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IndicatedHeadingDeg":
                    IndicatedHeadingDeg = Model.IndicatedHeadingDeg;
                    break;
                case "GPSIndicatedVerticalSpeed":
                    GPSIndicatedVerticalSpeed = Model.GPSIndicatedVerticalSpeed;
                    break;
                case "GPSIndicatedGroundSpeedKt":
                    GPSIndicatedGroundSpeedKt = Model.GPSIndicatedGroundSpeedKt;
                    break;
                case "AirspeedIndicatorIndicatedSpeedKt":
                    AirspeedIndicatorIndicatedSpeedKt = Model.AirspeedIndicatorIndicatedSpeedKt;
                    break;
                case "GPSIndicatedAltitudeFt":
                    GPSIndicatedAltitudeFt = Model.GPSIndicatedAltitudeFt;
                    break;
                case "AttitudeIndicatorInternalRollDeg":
                    AttitudeIndicatorInternalRollDeg = Model.AttitudeIndicatorInternalRollDeg;
                    break;
                case "AttitudeIndicatorInternalPitchDeg":
                    AttitudeIndicatorInternalPitchDeg = Model.AttitudeIndicatorInternalPitchDeg;
                    break;
                case "AltimeterIndicatedAltitudeFt":
                    AltimeterIndicatedAltitudeFt = Model.AltimeterIndicatedAltitudeFt;
                    break;
                case "Latitude":
                case "Longitude":
                    {
                        double lon = 0, lat = 0;
                        if (!double.TryParse(Model.Latitude, out lat))
                            Error = "Invalid latitude from the server.";
                        if (lat < -90 || lat > 90)
                        {
                            Error = "Invalid latitude from the server.";
                            lat = 0;
                        }
                        if (!double.TryParse(Model.Longitude, out lon))
                            Error = "Invalid longitude from the server.";
                        if (lon < -180 || lon > 180)
                        {
                            Error = "Invalid longitude from the server.";
                            lon = 0;
                        }
                        SetPos(lat, lon);
                        break;
                    }
                case "Error":
                    Error = Model.Error;
                    break;
                case "IsConnected":
                    {
                        if (Model.IsConnected)
                            Visible = "Visible";
                        else
                            Visible = "Hidden";
                        break;
                    }
                default:
                    break;
            }
        }
        //Update the model.
        public void Update(double rudder, double elevator, double aileron, double throttle)
        {
            Model.SetRudder(rudder.ToString());
            Model.SetElevator(elevator.ToString());
            Model.SetAileron(aileron.ToString());
            Model.SetThrottle(throttle.ToString());
        }
        //Cancel the model.
        public void Cancel()
        {
            Model.Cancel();
        }
    }
}
