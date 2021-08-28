using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Net.Sockets;
using System.Configuration;
using System.IO;

namespace FlightSimulatorApp
{
    public class Airplane : INotifyPropertyChanged
    {
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

        private string _Rudder;
        public string Rudder
        {
            get => _Rudder;
            private set
            {
                _Rudder = value;
                NotifyPropertyChanged(nameof(Rudder));
            }
        }

        private string _Elevator;
        public string Elevator
        {
            get => _Elevator;
            private set
            {
                _Elevator = value;
                NotifyPropertyChanged(nameof(Elevator));
            }
        }

        private string _Aileron;
        public string Aileron
        {
            get => _Aileron;
            private set
            {
                _Aileron = value;
                NotifyPropertyChanged(nameof(Aileron));
            }
        }

        private string _Throttle;
        public string Throttle
        {
            get => _Throttle;
            private set
            {
                _Throttle = value;
                NotifyPropertyChanged(nameof(Throttle));
            }
        }

        private string _Latitude;
        public string Latitude
        {
            get => _Latitude;
            private set
            {
                _Latitude = value;
                NotifyPropertyChanged(nameof(Latitude));
            }
        }

        private string _Longitude;
        public string Longitude
        {
            get => _Longitude;
            private set
            {
                _Longitude = value;
                NotifyPropertyChanged(nameof(Longitude));
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

        private bool _IsConnected;
        public bool IsConnected
        {
            get => _IsConnected;
            private set
            {
                _IsConnected = value;
                NotifyPropertyChanged(nameof(IsConnected));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private TcpClient client;
        private Thread thread;
        private bool cancel;
        private bool ElevatorChanged;
        private bool RudderChanged;
        private bool AileronChanged;
        private bool ThrottleChanged;
        //Constructor.
        public Airplane()
        {
            cancel = false;
            IsConnected = false;
            thread = new Thread(() => Connect(ConfigurationManager.AppSettings.Get("IP"), ConfigurationManager.AppSettings.Get("Port")));
            thread.Start();
            ElevatorChanged = false;
            RudderChanged = false;
            AileronChanged = false;
            ThrottleChanged = false;
        }

        //Update the rudder.
        public void SetRudder(string rudder)
        {
            Rudder = rudder;
            RudderChanged = true;
        }
        //Update the elevator.
        public void SetElevator(string elevator)
        {
            Elevator = elevator;
            ElevatorChanged = true;
        }
        //Update the aileron.
        public void SetAileron(string aileron)
        {
            Aileron = aileron;
            AileronChanged = true;
        }
        //Update the throttle.
        public void SetThrottle(string throttle)
        {
            Throttle = throttle;
            ThrottleChanged = true;
        }
        void NotifyPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        //Cancel the connection to the server.
        public void Cancel()
        {
            cancel = true;
            if (thread.IsAlive)
            {
                thread.Join();
            }
        }

        //Reconnect to the server.
        public void Reconnect(string ip, string port)
        {
            Error = "";
            Cancel();
            cancel = false;
            thread = new Thread(() => Connect(ip, port));
            thread.Start();
        }

        //Connect to the server, sending and reciving data.
        private void Connect(string ip, string port)
        {
            NetworkStream ns = null;
            int p;
            if (!int.TryParse(port, out p))
            {
                Error = "Inavlid port.";
                return;
            }
            try
            {
                //Connect to the server.
                client = new TcpClient(ip, p);
                try
                {
                    IsConnected = true;
                    while (!cancel)
                    {
                        String responseData;

                        ns = client.GetStream();
                        ns.ReadTimeout = 10000;
                        ns.WriteTimeout = 10000;

                        /*****Sending set Commands*******/

                        if (ElevatorChanged)
                        {
                            if (SetCommand(ns, "/controls/flight/elevator", Elevator))
                                ElevatorChanged = false;
                            else
                                continue;
                        }

                        if (RudderChanged)
                        {
                            if (SetCommand(ns, "/controls/flight/rudder", Rudder))
                                RudderChanged = false;
                            else
                                continue;
                        }

                        if (AileronChanged)
                        {
                            if (SetCommand(ns, "/controls/flight/aileron", Aileron))
                                AileronChanged = false;
                            else
                                continue;
                        }

                        if (ThrottleChanged)
                        {
                            if (SetCommand(ns, "/controls/engines/current-engine/throttle", Throttle))
                                ThrottleChanged = false;
                            else
                                continue;
                        }

                        /*****Sending get Commands*******/
                        responseData = GetCommand(ns, "/instrumentation/heading-indicator/indicated-heading-deg");
                        if (responseData != null)
                            IndicatedHeadingDeg = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/gps/indicated-vertical-speed");
                        if (responseData != null)
                            GPSIndicatedVerticalSpeed = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/gps/indicated-ground-speed-kt");
                        if (responseData != null)
                            GPSIndicatedGroundSpeedKt = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/airspeed-indicator/indicated-speed-kt");
                        if (responseData != null)
                            AirspeedIndicatorIndicatedSpeedKt = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/gps/indicated-altitude-ft");
                        if (responseData != null)
                            GPSIndicatedAltitudeFt = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/attitude-indicator/internal-roll-deg");
                        if (responseData != null)
                            AttitudeIndicatorInternalRollDeg = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/attitude-indicator/internal-pitch-deg");
                        if (responseData != null)
                            AttitudeIndicatorInternalPitchDeg = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/instrumentation/altimeter/indicated-altitude-ft");
                        if (responseData != null)
                            AltimeterIndicatedAltitudeFt = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/position/latitude-deg");
                        if (responseData != null)
                            Latitude = responseData;
                        else
                            continue;

                        responseData = GetCommand(ns, "/position/longitude-deg");
                        if (responseData != null)
                            Longitude = responseData;
                        else
                            continue;

                        Thread.Sleep(200);
                        Error = "";
                    }
                }
                catch
                {
                    IsConnected = false;
                    Error = "Couldn't execute a command. Try connecting to the server again";
                }
                finally
                {
                    ns?.Close();
                }
            }
            catch
            {
                IsConnected = false;
                Error = "Couldn't connect to the server. Try again.";
            }
            finally
            {
                IsConnected = false;
                client?.Close();
            }


        }
        private string GetCommand(NetworkStream ns, string path)
        {
            try
            {
                Byte[] data;
                String responseData;
                Int32 bytes;
                data = Encoding.ASCII.GetBytes(string.Format("get {0}\n", path));
                ns.Write(data, 0, data.Length);
                data = new Byte[256];
                bytes = ns.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                if (responseData != "ERR\n")
                    return responseData;
                Error = "Get command error.\n";
                return null;
            }
            catch (IOException)
            {
                Error = "Timeout in get command.\n";
                return null;
            }
        }
        private bool SetCommand(NetworkStream ns, string path, string value)
        {
            try
            {
                Byte[] data;
                String responseData;
                Int32 bytes;
                string val = value;
                string message = string.Format("set {0} {1}\n", path, value);
                val += '\n';
                data = Encoding.ASCII.GetBytes(message);
                ns.Write(data, 0, data.Length);
                data = new Byte[256];
                bytes = ns.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                if (val != responseData && responseData != "0.0\n" && responseData != "1.0\n" && responseData != "-1.0\n")
                {
                    Error = "Set command error.";
                    return false;
                }
                return true;
            }
            catch (IOException)
            {
                Error = "Timeout in set command.\n";
                return false;
            }
        }
    }
}
