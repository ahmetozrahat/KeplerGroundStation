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

        private PayloadData _payloadData;
        private System.Timers.Timer _refereeComputerTimer;

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

        private void FlightComputerDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            
            try
            {
                string indata = sp.ReadLine();
                _payloadData = PayloadParser.ParsePayloadData(indata);

                // Perform the UI updates in a seperate Thread.
                Dispatcher.Invoke(() =>
                {
                    UpdatePackageInfo();
                    UpdateTemperature();
                    UpdatePressure();
                    UpdateAltitude();
                    UpdateMap();
                    UpdateAccelerationData();
                    AddRowDataToConsole(indata);
                }
                );
            } 
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
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
            TemperatureDesc.Content = DataFormatter.formatTemperature(_payloadData.Temperature);
            RocketData.AddTemperatureData(_payloadData.Temperature);
        }

        /// <summary>
        /// Updates the Pressure chart by adding a new observable value to the series.
        /// It also updates the relative fields of pressure in the GUI.
        /// </summary>
        private void UpdatePressure()
        {
            PressureDesc.Content = DataFormatter.formatPressure(_payloadData.Pressure);
            RocketData.AddPressureData(_payloadData.Pressure);
        }

        /// <summary>
        /// Updates the Altitude chart by adding a new observable value to the series.
        /// It also updates the relative fields of altitude in the GUI.
        /// </summary>
        private void UpdateAltitude()
        {
            AltitudeDesc.Content = DataFormatter.formatDistanceMeters(_payloadData.Altitude);
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

            RocketGPSLat.Content = _payloadData.GpsLat;
            RocketGPSLong.Content = _payloadData.GpsLong;

            double groundLat = 38.769628; // Teknopark // 38.769628; // Mimarsinanbahcelievler
            double groundLon = 35.583048; // 35.583048;

            double dummyLat = 38.643833;  // Mühendislik // 38.643833; // Malatya yolu
            double dummyLon = 35.701681; // 35.701681;

            // Create push pin for ground station.
            Pushpin pinGround = new()
            {
                Location = new Location(groundLat, groundLon),
                Background = new SolidColorBrush(Color.FromArgb(164, 226, 255, 100))
            };

            // Create push pin for flight computer.
            Pushpin pinRocket = new()
            {
                Location = new Location(dummyLat, dummyLon),
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
                new Location(dummyLat, dummyLon)
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
            double distance = DistanceCalculator.GetDistanceDifference(groundLat, groundLon, dummyLat, dummyLon);
            DistanceDesc.Content = ValueFormatter.FormatDistance(distance);
        }

        /// <summary>
        /// Updates the acceleration and gyro data in the respective fields in GUI.
        /// Gets the values from the given PayloadData object.
        /// </summary>
        private void UpdateAccelerationData()
        {
            AccelXLabel.Content = DataFormatter.formatAcceleration(_payloadData.AccelerationX);
            AccelYLabel.Content = DataFormatter.formatAcceleration(_payloadData.AccelerationY);
            AccelZLabel.Content = DataFormatter.formatAcceleration(_payloadData.AccelerationZ);
            GyroXLabel.Content = DataFormatter.formatGyro(_payloadData.GyroX);
            GyroYLabel.Content = DataFormatter.formatGyro(_payloadData.GyroY);
            GyroZLabel.Content = DataFormatter.formatGyro(_payloadData.GyroZ);

            int tiltAngle = AngleCalculator.CalculateTiltAngle(_payloadData.AccelerationY, _payloadData.AccelerationZ);
            RocketAngle.Content = DataFormatter.formatAngle(tiltAngle);
        }


        /// <summary>
        /// Prints the raw incoming data to the console with formatted output.
        /// </summary>
        /// <param name="data">The incoming data from serial port.</param>
        private void AddRowDataToConsole(string data)
        {
            DateTime dateTime = DateTime.Now;

            Run timeStampRun = new("[" + dateTime.ToString("HH:mm:ss") + "]: ")
            {
                Foreground = new SolidColorBrush(Colors.Aqua)
            };

            Run dataRun = new(data)
            {
                Foreground = new SolidColorBrush(Colors.White)
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
