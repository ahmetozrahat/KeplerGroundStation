using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.IO.Ports;
using KeplerGroundStation.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Documents;
using KeplerGroundStation.Helpers;
using KeplerGroundStation.ViewModel;
using System.Globalization;

namespace KeplerGroundStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private SerialPortViewModel _flightComputerSerialPort;
        public SerialPortViewModel FlightComputerSerialPort
        {
            get { return _flightComputerSerialPort; }
            set
            {
                _flightComputerSerialPort = value;
                OnPropertyChanged(nameof(FlightComputerSerialPort));
            }
        }

        private SerialPortViewModel _refereeComputerSerialPort;
        public SerialPortViewModel RefereeComputerSerialPort
        {
            get { return _refereeComputerSerialPort; }
            set
            {
                _refereeComputerSerialPort = value;
                OnPropertyChanged(nameof(RefereeComputerSerialPort));
            }
        }

        private RocketDataViewModel _rocketData;
        public RocketDataViewModel RocketData
        {
            get { return _rocketData; }
            set
            {
                _rocketData = value;
                OnPropertyChanged(nameof(RocketData));
            }
        }

        private ChartsViewModel _charts;
        public ChartsViewModel Charts
        {
            get { return _charts; }
            set
            {
                _charts = value;
                OnPropertyChanged(nameof(Charts));
            }
        }

        private ICommand _connectFlightComputerClicked;
        public ICommand ConnectFlightComputerClicked
        {
            get { return _connectFlightComputerClicked;}
            set
            {
                _connectFlightComputerClicked = value;
                OnPropertyChanged(nameof(ConnectFlightComputerClicked));
            }
        }

        private ICommand _disconnectFlightComputerClicked;
        public ICommand DisconnectFlightComputerClicked
        {
            get { return _disconnectFlightComputerClicked; }
            set
            {
                _disconnectFlightComputerClicked = value;
                OnPropertyChanged(nameof(DisconnectFlightComputerClicked));
            }
        }

        private ICommand _connectRefereeComputerClicked;
        public ICommand ConnectRefereeComputerClicked
        {
            get { return _connectRefereeComputerClicked; }
            set
            {
                _connectRefereeComputerClicked = value;
                OnPropertyChanged(nameof(ConnectRefereeComputerClicked));
            }
        }

        private ICommand _disconnectRefereeComputerClicked;
        public ICommand DisconnectRefereeComputerClicked
        {
            get { return _disconnectRefereeComputerClicked; }
            set
            {
                _disconnectRefereeComputerClicked = value;
                OnPropertyChanged(nameof(DisconnectRefereeComputerClicked));
            }
        }

        private ICommand _updateLocationClicked;
        public ICommand UpdateLocationClicked
        {
            get { return _updateLocationClicked; }
            set
            {
                _updateLocationClicked = value;
                OnPropertyChanged(nameof(UpdateLocationClicked));
            }
        }

        private PayloadData _payloadData;
        private System.Timers.Timer _refereeComputerTimer;
        private byte[] receivedBytes = new byte[52];
        private int receivedBytesCount = 0;
        double groundLat = 0;
        double groundLon = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _refereeComputerTimer = new System.Timers.Timer();
            _refereeComputerTimer.Interval = 200;
            _refereeComputerTimer.Elapsed += RefereeComputerTimer_Tick;

            KeplerMap.ZoomLevel = 15;

            KeplerFlightComputerStateString.Content = "Bağlı Değil";
            KeplerFlightComputerStateColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            RefereeComputerStateString.Content = "Bağlı Değil";
            RefereeComputerStateColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            _connectFlightComputerClicked = new RelayCommand(HandleConnectFlightComputerClicked);
            _disconnectFlightComputerClicked = new RelayCommand(HandleDisconnectFlightComputerClicked);

            _connectRefereeComputerClicked = new RelayCommand(HandleConnectRefereeComputerClicked);
            _disconnectRefereeComputerClicked = new RelayCommand(HandleDisconnectRefereeComputerClicked);

            _updateLocationClicked = new RelayCommand(HandleUpdateLocationClicked);

            FlightComputerSerialPort = new SerialPortViewModel(FlightComputerDataReceivedHandler);
            RefereeComputerSerialPort = new SerialPortViewModel();

            Charts = new ChartsViewModel();
            RocketData = new RocketDataViewModel();

            // Setup timer for updating the UI.
            DispatcherTimer dispatcherTimer = new();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Prepares and sends data to referee computer every x second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefereeComputerTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            byte[] refereePayload = RefereePayloadGenerator.GeneratePayload(_payloadData);
            RefereeComputerSerialPort.WriteBytes(refereePayload);
        }

        /// <summary>
        /// Updates the UI with randomly generated values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs args)
        {
            // Set time information.
            KeplerCurrentDate.Content = DateTime.Now.ToString("d MMMM yyyy");
            KeplerCurrentTime.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        /// ----- Flight Computer Connection Section -----

        private void HandleConnectFlightComputerClicked()
        {
            try
            {
                FlightComputerSerialPort.Open();
                KeplerFlightComputerStateString.Content = "Bağlandı";
                KeplerFlightComputerStateColor.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HandleDisconnectFlightComputerClicked()
        {
            try
            {
                FlightComputerSerialPort.Close();
                KeplerFlightComputerStateString.Content = "Bağlı Değil";
                KeplerFlightComputerStateColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // -----------------------------------------------

        // ----- Referee Computer Connection Section -----

        private void HandleConnectRefereeComputerClicked()
        {
            try
            {
                RefereeComputerSerialPort.Open();
                RefereeComputerStateString.Content = "Bağlandı";
                RefereeComputerStateColor.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                _refereeComputerTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HandleDisconnectRefereeComputerClicked()
        {
            try
            {
                RefereeComputerSerialPort.Close();
                RefereeComputerStateString.Content = "Bağlı Değil";
                RefereeComputerStateColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                _refereeComputerTimer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // --------------------------------------------

        // ----- Update Location Section -----

        private void HandleUpdateLocationClicked()
        {
            try
            {
                groundLat = double.Parse(GroundStationLat.Text, CultureInfo.InvariantCulture);
                groundLon = double.Parse(GroundStationLng.Text, CultureInfo.InvariantCulture);
            } catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        // --------------------------------------------

        private void FlightComputerDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            int bytesToRead = sp.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            sp.Read(buffer, 0, bytesToRead);

            try
            {
                _payloadData = PayloadParser.ParsePayloadData(buffer);

                // Perform the UI updates in a seperate Thread.
                Dispatcher.Invoke(() =>
                {
                    UpdateFlightStatus();
                    UpdatePackageInfo();
                    UpdateTemperature();
                    UpdatePressure();
                    UpdateAltitude();
                    UpdateMap();
                    UpdateAccelerationData();
                    AddRowDataToConsole(buffer);
                }
                );
            } 
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates the flight status field in the GUI.
        /// </summary>
        private void UpdateFlightStatus()
        {
            switch (_payloadData.FlightStatus)
            {
                case 1:
                    FlightStatus.Content = "Paraşütler Açılmadı";
                    break;
                case 2:
                    FlightStatus.Content = "Birincil Paraşüt Açıldı, İkincil Paraşüt Açılmadı";
                    break;
                case 3:
                    FlightStatus.Content = "Birincil Paraşüt Açılmadı, İkincil Paraşüt Açıldı";
                    break;
                case 4:
                    FlightStatus.Content = "Paraşütler Açıldı";
                    break;
            }
        }

        /// <summary>
        /// Updates the package info fields in the GUI.
        /// </summary>
        private void UpdatePackageInfo()
        {
            PackageId.Content = _payloadData.PackageId;
            TotalPackageCount.Content = RocketData.IncrementTotalPackageNumber();
        }

        /// <summary>
        /// Updates the Temperature chart by adding a new observable value to the series.
        /// It also updates the relative fields of temperature in the GUI.
        /// </summary>
        private void UpdateTemperature()
        {
            TemperatureDesc.Content = DataFormatter.FormatTemperature(_payloadData.Temperature);
            RocketData.AddTemperatureData(_payloadData.Temperature);
        }

        /// <summary>
        /// Updates the Pressure chart by adding a new observable value to the series.
        /// It also updates the relative fields of pressure in the GUI.
        /// </summary>
        private void UpdatePressure()
        {
            PressureDesc.Content = DataFormatter.FormatPressure(_payloadData.Pressure);
            RocketData.AddPressureData(_payloadData.Pressure);
        }

        /// <summary>
        /// Updates the Altitude chart by adding a new observable value to the series.
        /// It also updates the relative fields of altitude in the GUI.
        /// </summary>
        private void UpdateAltitude()
        {
            AltitudeDesc.Content = DataFormatter.FormatDistanceMeters(_payloadData.Altitude);
            RocketData.AddAltitudeData(_payloadData.Altitude);
        }

        /// <summary>
        /// Updates the map with given coordinates in the PayloadData object.
        /// Also updates the GPS Lat and GPS Long in the relative fields of GUI.
        /// </summary>
        private void UpdateMap()
        {
            if (_payloadData.GpsLat == 1000 && _payloadData.GpsLong == 1000)
            {
                return;
            }

            RocketGPSLat.Content = DataFormatter.FormatLocation(_payloadData.GpsLat);
            RocketGPSLong.Content = DataFormatter.FormatLocation(_payloadData.GpsLong);

            // Create push pin for ground station.
            Pushpin pinGround = new()
            {
                Location = new Location(groundLat, groundLon),
                Background = new SolidColorBrush(Color.FromArgb(164, 226, 255, 100))
            };

            // Create push pin for flight computer.
            Pushpin pinRocket = new()
            {
                Location = new Location(_payloadData.GpsLat, _payloadData.GpsLong),
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 102, 100))
            };

            // Clear the pins and add the new ones.
            KeplerMap.Children.Clear();
            KeplerMap.Children.Add(pinGround);
            KeplerMap.Children.Add(pinRocket);

            // Create a polygon consisting of the locations.
            MapPolygon polygon = new();
            var locations = new LocationCollection()
            {
                new Location(groundLat, groundLon),
                new Location(_payloadData.GpsLat, _payloadData.GpsLong)
            };

            // Style the polygon.
            polygon.Locations = locations;
            polygon.Fill = new SolidColorBrush(Colors.Red);
            polygon.Stroke = new SolidColorBrush(Colors.Red);
            polygon.StrokeThickness = 3;
            polygon.Opacity = 1;
            
            // Add the polygon to the map and set the view to zoom properly.
            KeplerMap.Children.Add(polygon);
            KeplerMap.SetView(locations, new Thickness(50, 50, 50, 50), 0);

            // Calculate the distance between two coordinates and update it in the GUI.
            double distance = DistanceCalculator.GetDistanceDifference(groundLat, groundLon, _payloadData.GpsLat, _payloadData.GpsLong);
            DistanceDesc.Content = DataFormatter.FormatDistance(distance);
        }

        /// <summary>
        /// Updates the acceleration and gyro data in the respective fields in GUI.
        /// Gets the values from the given PayloadData object.
        /// </summary>
        private void UpdateAccelerationData()
        {
            AccelXLabel.Content = DataFormatter.FormatAcceleration(_payloadData.AccelerationX);
            AccelYLabel.Content = DataFormatter.FormatAcceleration(_payloadData.AccelerationY);
            AccelZLabel.Content = DataFormatter.FormatAcceleration(_payloadData.AccelerationZ);
            GyroXLabel.Content = DataFormatter.FormatGyro(_payloadData.GyroX);
            GyroYLabel.Content = DataFormatter.FormatGyro(_payloadData.GyroY);
            GyroZLabel.Content = DataFormatter.FormatGyro(_payloadData.GyroZ);

            float tiltAngle = AngleCalculator.CalculateTiltAngle(_payloadData.AccelerationY, _payloadData.AccelerationZ);
            RocketAngle.Content = DataFormatter.FormatAngle(tiltAngle);
        }


        /// <summary>
        /// Prints the raw incoming data to the console with formatted output.
        /// </summary>
        /// <param name="data">The incoming data from serial port.</param>
        private void AddRowDataToConsole(byte[] data)
        {
            DateTime dateTime = DateTime.Now;

            Run timeStampRun = new("[" + dateTime.ToString("HH:mm:ss") + "]: ")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(70, 130, 180))
            };

            Run dataRun = new(DataFormatter.FormatByteArray(data))
            {
                Foreground = new SolidColorBrush(Colors.DarkGray)
            };

            Paragraph paragraph = new();
            paragraph.Inlines.Add(timeStampRun);
            paragraph.Inlines.Add(dataRun);

            SerialMonitor.Document.Blocks.Add(paragraph);
            SerialMonitor.ScrollToEnd();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
