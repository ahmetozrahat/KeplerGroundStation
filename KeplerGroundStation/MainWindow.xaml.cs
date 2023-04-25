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
using KeplerGroundStation.Helpers;
using KeplerGroundStation.ViewModel;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Timers;
using Microsoft.Maps.MapControl.WPF;
using System.Management;

namespace KeplerGroundStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private SerialPortViewModel _flightComputerSerialPort;

        /// <summary>
        /// Serial port view model for receiving flight computer data.
        /// </summary>
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

        /// <summary>
        /// Serial port view model for transmitting referee computer data.
        /// </summary>
        public SerialPortViewModel RefereeComputerSerialPort
        {
            get { return _refereeComputerSerialPort; }
            set
            {
                _refereeComputerSerialPort = value;
                OnPropertyChanged(nameof(RefereeComputerSerialPort));
            }
        }

        private RocketDataViewModel _flightComputerData;

        /// <summary>
        /// View model for holding the Flight computer data. 
        /// </summary>
        public RocketDataViewModel FlightComputerData
        {
            get { return _flightComputerData; }
            set
            {
                _flightComputerData = value;
                OnPropertyChanged(nameof(FlightComputerData));
            }
        }

        private PayloadDataViewModel _payloadComputerData;
        /// <summary>
        /// View model for holding the Payload computer data.
        /// </summary>
        public PayloadDataViewModel PayloadComputerData
        {
            get { return _payloadComputerData; }
            set
            {
                _payloadComputerData = value;
                OnPropertyChanged(nameof(PayloadComputerData));
            }
        }

        private ChartsViewModel _charts;
        /// <summary>
        /// View model for holding the charts data.
        /// </summary>
        public ChartsViewModel Charts
        {
            get { return _charts; }
            set
            {
                _charts = value;
                OnPropertyChanged(nameof(Charts));
            }
        }

        private LocationViewModel _locations;
        /// <summary>
        /// View model for holding the location related data.
        /// </summary>
        public LocationViewModel Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }

        private ICommand _connectFlightComputerClicked;
        /// <summary>
        /// Click event for Flight computer not clicked state.
        /// </summary>
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
        /// <summary>
        /// Click event for Flight computer clicked state.
        /// </summary>
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
        /// <summary>
        /// Click event for Referee computer not clicked state.
        /// </summary>
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
        /// <summary>
        /// Click event for Referee computer clicked state.
        /// </summary>
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
        /// <summary>
        /// Click event for Update Location button clicked state.
        /// </summary>
        public ICommand UpdateLocationClicked
        {
            get { return _updateLocationClicked; }
            set
            {
                _updateLocationClicked = value;
                OnPropertyChanged(nameof(UpdateLocationClicked));
            }
        }

        private FlightComputerPayloadData _flightComputerPayload;
        /// <summary>
        /// Payload data for holding the data coming from the Flight Computer.
        /// </summary>
        public FlightComputerPayloadData FlightComputerPayload
        {
            get { return _flightComputerPayload; }
            set
            {
                _flightComputerPayload = value;
                OnPropertyChanged(nameof(FlightComputerPayload));
            }
        }

        private BackupComputerPayloadData _backupComputerPayload;
        /// <summary>
        /// Payload data for holding the data coming from the Backup Computer.
        /// </summary>
        public BackupComputerPayloadData BackupComputerPayload
        {
            get { return _backupComputerPayload; }
            set
            {
                _backupComputerPayload = value;
                OnPropertyChanged(nameof(BackupComputerPayload));
            }
        }

        private PayloadComputerPayloadData _payloadComputerPayload;
        /// <summary>
        /// Payload data for holding the data coming from the Payload Computer.
        /// </summary>
        public PayloadComputerPayloadData PayloadComputerPayload
        {
            get { return _payloadComputerPayload; }
            set
            {
                _payloadComputerPayload = value;
                OnPropertyChanged(nameof(PayloadComputerPayload));
            }
        }

        private Timer _refereeComputerTimer;
        /// <summary>
        /// Timer for creating pulses to send data for Referee Computer.
        /// </summary>
        public Timer RefereeComputerTimer
        {
            get { return _refereeComputerTimer; }
            set
            {
                _refereeComputerTimer = value;
                OnPropertyChanged(nameof(RefereeComputerTimer));
            }
        }

        private List<byte> _serialData;
        /// <summary>
        /// Object for holding the incoming data from Serial Port.
        /// </summary>
        public List<byte> SerialData
        {
            get { return _serialData; }
            set
            {
                _serialData = value;
                OnPropertyChanged(nameof(SerialData));
            }
        }

        private RefereePayloadGenerator _payloadGenerator;
        /// <summary>
        /// Object for generating Payloads for Referee Computer.
        /// </summary>
        public RefereePayloadGenerator PayloadGenerator
        {
            get { return _payloadGenerator; }
            set
            {
                _payloadGenerator = value;
                OnPropertyChanged(nameof(PayloadGenerator));
            }
        }

        private ManagementEventWatcher watcher;

        public MainWindow()
        {
            InitializeComponent();
            StartDeviceWatcher();
            DataContext = this;
            
            InitializeRefereeComputer();
            InitializeFlightComputer();
            InitializeObjects();
            InitializeMap();
            SetupClickListeners();
            SetupUpdateGUITimer();
        }

        /// <summary>
        /// Initializes the Referee Computer related stuff.
        /// </summary>
        private void InitializeRefereeComputer()
        {
            RefereeComputerTimer = new();
            RefereeComputerTimer.Interval = 200;
            RefereeComputerTimer.Elapsed += RefereeComputerTimer_Tick;

            RefereeComputerStateString.Content = "Bağlı Değil";
            RefereeComputerStateColor.Fill = KeplerColors.Red;

            RefereeComputerSerialPort = new();
            PayloadGenerator = new();
        }

        /// <summary>
        /// Initializes the Flight Computer related stuff.
        /// </summary>
        private void InitializeFlightComputer()
        {
            KeplerFlightComputerStateString.Content = "Bağlı Değil";
            KeplerFlightComputerStateColor.Fill = KeplerColors.Red;

            FlightComputerSerialPort = new SerialPortViewModel(FlightComputerDataReceivedHandler);
        }

        /// <summary>
        /// Initializes objects by creating new instances.
        /// </summary>
        private void InitializeObjects()
        {
            SerialData = new();
            Charts = new();
            FlightComputerData = new();
            PayloadComputerData = new();
            Locations = new();
        }

        /// <summary>
        /// Initializes the map for showing flight computers inside the map.
        /// </summary>
        private void InitializeMap()
        {
            KeplerMap.ZoomLevel = 15;
            Locations.UpdateGroundStationLocation(38.85150386157974, 33.54593931823632);
            KeplerMap.Children.Add(Locations.GroundStationPin);
        }

        /// <summary>
        /// Sets up the click listeners for buttons in the GUI.
        /// </summary>
        private void SetupClickListeners()
        {
            ConnectFlightComputerClicked = new RelayCommand(HandleConnectFlightComputerClicked);
            DisconnectFlightComputerClicked = new RelayCommand(HandleDisconnectFlightComputerClicked);

            ConnectRefereeComputerClicked = new RelayCommand(HandleConnectRefereeComputerClicked);
            DisconnectRefereeComputerClicked = new RelayCommand(HandleDisconnectRefereeComputerClicked);

            UpdateLocationClicked = new RelayCommand(HandleUpdateLocationClicked);
        }

        /// <summary>
        /// Sets up timer for updating the GUI.
        /// </summary>
        private void SetupUpdateGUITimer()
        {
            DispatcherTimer dispatcherTimer = new();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Prepares and sends data to referee computer every x second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefereeComputerTimer_Tick(object sender, ElapsedEventArgs e)
        {
            byte[] refereePayload = PayloadGenerator.GeneratePayload(FlightComputerPayload, BackupComputerPayload, PayloadComputerPayload);
            RefereeComputerSerialPort.WriteBytes(refereePayload);
        }

        /// <summary>
        /// Updates the GUI by setting the time information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs args)
        {
            // Set time information.
            KeplerCurrentDate.Content = DateTime.Now.ToString("d MMMM yyyy");
            KeplerCurrentTime.Content = DateTime.Now.ToString("HH:mm:ss");

            KeplerMap.Children.Clear();

            var locations = Locations.GetLocations();

            if (Locations.FlightComputerLocation != null)
            {
                Pushpin flightComputerPin = Locations.FlightComputerPin;
                flightComputerPin.Location = Locations.FlightComputerLocation;
                flightComputerPin.Content = 1;
                KeplerMap.Children.Add(flightComputerPin);
            }

            if (Locations.BackupComputerLocation != null)
            {
                Pushpin backupComputerPin = Locations.BackupComputerPin;
                backupComputerPin.Location = Locations.BackupComputerLocation;
                backupComputerPin.Content = 2;
                KeplerMap.Children.Add(backupComputerPin);
            }

            if (Locations.PayloadComputerLocation != null)
            {
                Pushpin payloadComputerPin = Locations.PayloadComputerPin;
                payloadComputerPin.Location = Locations.PayloadComputerLocation;
                payloadComputerPin.Content = 3;
                KeplerMap.Children.Add(payloadComputerPin);
            }

            if (Locations.GroundStationLocation != null)
            {
                Pushpin groundStationPin = Locations.GroundStationPin;
                groundStationPin.Location = Locations.GroundStationLocation;
                groundStationPin.Content = 4;
                KeplerMap.Children.Add(groundStationPin);
            }

            KeplerMap.SetView(locations, new Thickness(50, 50, 50, 50), 0);
            Trace.WriteLine("\n");
        }

        public void StartDeviceWatcher()
        {
            WqlEventQuery query = new("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 or EventType = 3");

            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += new EventArrivedEventHandler(DeviceChangeEvent);
            watcher.Start();
        }

        private void DeviceChangeEvent(object sender, EventArrivedEventArgs e)
        {
            FlightComputerSerialPort.UpdateSerialPorts();
        }

        /// ----- Flight Computer Connection Section -----

        private void HandleConnectFlightComputerClicked()
        {
            try
            {
                FlightComputerSerialPort.Open();
                KeplerFlightComputerStateString.Content = "Bağlandı";
                KeplerFlightComputerStateColor.Fill = KeplerColors.Green;
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
                KeplerFlightComputerStateColor.Fill = KeplerColors.Red;
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
                RefereeComputerStateColor.Fill = KeplerColors.Green;
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
                RefereeComputerStateColor.Fill = KeplerColors.Red;
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
                double groundLat = double.Parse(GroundStationLat.Text, CultureInfo.InvariantCulture);
                double groundLon = double.Parse(GroundStationLng.Text, CultureInfo.InvariantCulture);

                // Clear the pins and add the new ones.
                if (KeplerMap.Children.Contains(Locations.GroundStationPin))
                    KeplerMap.Children.Remove(Locations.GroundStationPin);

                Locations.UpdateGroundStationLocation(groundLat, groundLon);

                KeplerMap.Children.Add(Locations.GroundStationPin);

                var locations = Locations.GetLocations();

                // Add the polygon to the map and set the view to zoom properly.
                KeplerMap.SetView(locations, new Thickness(50, 50, 50, 50), 0);
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

            for (int i = 0; i < buffer.Length; i++)
            {
                SerialData.Add(buffer[i]);
            }

            if (SerialData.Count > 0 && SerialData[0] != 0x7f && SerialData[1] != 0x7d)
            {
                SerialData.Clear();
            }

            if (SerialData.Count >= 4 && SerialData[0] == 0x7f && SerialData[1] == 0x7d && SerialData[^2] == 0x7d && SerialData[^1] == 0x7f)
            {
                try
                {
                    byte[] receivedPayload = _serialData.ToArray();
                    short deviceId = BitConverter.ToInt16(receivedPayload, 2); // Device Id

                    switch (deviceId)
                    {
                        case 1:
                            // Main flight computer
                            FlightComputerPayload = PayloadParser.ParseFlightComputerPayloadData(receivedPayload);
                            FlightComputerDataReceived(receivedPayload);
                            break;
                        case 2:
                            // Backup flight computer
                            BackupComputerPayload = PayloadParser.ParseBackupComputerPayloadData(receivedPayload);
                            BackupComputerDataReceived(receivedPayload);
                            break;
                        case 3:
                            // Payload flight computer
                            PayloadComputerPayload = PayloadParser.ParsePayloadComputerPayloadData(receivedPayload);
                            PayloadComputerDataReceived(receivedPayload);
                            break;
                    }
                    SerialData.Clear();
                } catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    SerialData.Clear();
                }

            }
        }

        // ------------------------------------------------------------

        /// <summary>
        /// Data received event handler for Flight Computer Data arrives.
        /// Updates the GUI accordingly with the incoming data.
        /// </summary>
        /// <param name="payload"></param>
        private void FlightComputerDataReceived(byte[] payload)
        {
            Dispatcher.Invoke(() =>
            {
                // Update the Flight Status field.
                FlightStatus.Content = FlightStatusHelper.GetFlightStatusString(FlightComputerPayload.FlightStatus);

                // Update the Package Number field.
                RocketPackageNumber.Content = FlightComputerPayload.PackageId;
                RocketTotalPackageCount.Content = FlightComputerData.IncrementFlightComputerTotalPackageNumber();

                // Update the Temperature field.
                TemperatureDesc.Content = DataFormatter.FormatTemperature(FlightComputerPayload.Temperature);
                FlightComputerData.AddTemperatureData(FlightComputerPayload.Temperature);

                // Update the Pressure field.
                PressureDesc.Content = DataFormatter.FormatPressure(FlightComputerPayload.Pressure);
                FlightComputerData.AddPressureData(FlightComputerPayload.Pressure);

                // Update the Altitude field.
                AltitudeDesc.Content = DataFormatter.FormatDistanceMeters(FlightComputerPayload.Altitude);
                FlightComputerData.AddAltitudeData(FlightComputerPayload.Altitude);

                // Update the Acceleration fields.
                AccelXLabel.Content = DataFormatter.FormatAcceleration(FlightComputerPayload.AccelerationX);
                AccelYLabel.Content = DataFormatter.FormatAcceleration(FlightComputerPayload.AccelerationY);
                AccelZLabel.Content = DataFormatter.FormatAcceleration(FlightComputerPayload.AccelerationZ);
                GyroXLabel.Content = DataFormatter.FormatGyro(FlightComputerPayload.GyroX);
                GyroYLabel.Content = DataFormatter.FormatGyro(FlightComputerPayload.GyroY);
                GyroZLabel.Content = DataFormatter.FormatGyro(FlightComputerPayload.GyroZ);

                float tiltAngle = AngleCalculator.CalculateTiltAngle(FlightComputerPayload.AccelerationY, FlightComputerPayload.AccelerationZ);
                RocketAngle.Content = DataFormatter.FormatAngle(tiltAngle);

                // Update the Map.
                UpdateFlightComputerMap();

                // Add raw data to the console.
                AddRawDataToConsole(payload);
            });
        }

        /// <summary>
        /// Data received event handler for Backup Computer Data arrives.
        /// Updates the GUI accordingly with the incoming data.
        /// </summary>
        /// <param name="payload"></param>
        private void BackupComputerDataReceived(byte[] payload)
        {
            Dispatcher.Invoke(() =>
            {
                // Update the Package Number field.
                RocketBackupPackageNumber.Content = BackupComputerPayload.PackageId;
                RocketBackupTotalPackageCount.Content = FlightComputerData.IncrementBackupComputerTotalPackageNumber();

                // Update the Map.
                UpdateBackupComputerMap();

                // Add raw data to the console.
                AddRawDataToConsole(payload);
            });
        }

        /// <summary>
        /// Data received event handler for Payload Computer Data arrives.
        /// Updates the GUI accordingly with the incoming data.
        /// </summary>
        /// <param name="payload"></param>
        private void PayloadComputerDataReceived(byte[] payload)
        {
            Dispatcher.Invoke(() => 
            {
                // Update the Package Number field.
                PayloadPackageNumber.Content = PayloadComputerPayload.PackageId;
                PayloadTotalPackageCount.Content = PayloadComputerData.IncrementTotalPackageNumber();

                // Update the Pressure field.
                PayloadPressureDesc.Content = DataFormatter.FormatPressure(PayloadComputerPayload.Pressure);
                PayloadComputerData.AddPressureData(PayloadComputerPayload.Pressure);

                // Update the Humidity field.
                PayloadHumidityDesc.Content = DataFormatter.FormatHumidity(PayloadComputerPayload.Humidity);
                PayloadComputerData.AddHumidityData(PayloadComputerPayload.Humidity);

                // Update the Temperature Field.
                PayloadTemperatureDesc.Content = DataFormatter.FormatTemperature(PayloadComputerPayload.Temperature);
                PayloadComputerData.AddTemperatureData(PayloadComputerPayload.Temperature);

                // Update the Map.
                UpdatePayloadComputerMap();

                // Add raw data to the console.
                AddRawDataToConsole(payload);
            });
        }

        /// <summary>
        /// Updates the map with given coordinates in the PayloadData object.
        /// Also updates the GPS Lat and GPS Long in the relative fields of GUI.
        /// </summary>
        private void UpdateFlightComputerMap()
        {
            if (FlightComputerPayload.GpsLat == 0 && FlightComputerPayload.GpsLng == 0)
            {
                return;
            }

            RocketGPSLat.Content = DataFormatter.FormatLocation(FlightComputerPayload.GpsLat);
            RocketGPSLong.Content = DataFormatter.FormatLocation(FlightComputerPayload.GpsLng);
            RocketGPSAlt.Content = DataFormatter.FormatDistanceMeters(FlightComputerPayload.GpsAlt);

            Locations.UpdateFlightComputerLocation(FlightComputerPayload.GpsLat, FlightComputerPayload.GpsLng);

            // Calculate the distance between two coordinates and update it in the GUI.
            double distance = DistanceCalculator.GetDistanceDifference(
                Locations.GroundStationLocation.Latitude,
                Locations.GroundStationLocation.Longitude,
                Locations.FlightComputerLocation.Latitude,
                Locations.FlightComputerLocation.Longitude
                );
            RocketGPSDistance.Content = DataFormatter.FormatDistance(distance);
        }

        private void UpdateBackupComputerMap()
        {
            if (BackupComputerPayload.GpsLat == 0 && BackupComputerPayload.GpsLng == 0)
            {
                return;
            }

            RocketBackupGPSLat.Content = DataFormatter.FormatLocation(BackupComputerPayload.GpsLat);
            RocketBackupGPSLng.Content = DataFormatter.FormatLocation(BackupComputerPayload.GpsLng);
            RocketBackupGPSAlt.Content = DataFormatter.FormatDistanceMeters(BackupComputerPayload.GpsAlt);

            Locations.UpdateBackupComputerLocation(BackupComputerPayload.GpsLat, BackupComputerPayload.GpsLng);

            // Calculate the distance between two coordinates and update it in the GUI.
            double distance = DistanceCalculator.GetDistanceDifference(
                Locations.GroundStationLocation.Latitude,
                Locations.GroundStationLocation.Longitude,
                Locations.BackupComputerLocation.Latitude,
                Locations.BackupComputerLocation.Longitude
                );
            RocketBackupGPSDistance.Content = DataFormatter.FormatDistance(distance);
        }

        private void UpdatePayloadComputerMap()
        {
            if (PayloadComputerPayload.GpsLat == 0 && PayloadComputerPayload.GpsLng == 0)
            {
                return;
            }

            PayloadGPSLat.Content = DataFormatter.FormatLocation(PayloadComputerPayload.GpsLat);
            PayloadGPSLng.Content = DataFormatter.FormatLocation(PayloadComputerPayload.GpsLng);
            PayloadGPSAlt.Content = DataFormatter.FormatDistanceMeters(PayloadComputerPayload.GpsAlt);

            Locations.UpdatePayloadComputerLocation(PayloadComputerPayload.GpsLat, PayloadComputerPayload.GpsLng);

            // Calculate the distance between two coordinates and update it in the GUI.
            double distance = DistanceCalculator.GetDistanceDifference(
                Locations.GroundStationLocation.Latitude,
                Locations.GroundStationLocation.Longitude,
                Locations.PayloadComputerLocation.Latitude,
                Locations.PayloadComputerLocation.Longitude
                );
            PayloadGPSDistance.Content = DataFormatter.FormatDistance(distance);
        }

        /// <summary>
        /// Prints the raw incoming data to the console with formatted output.
        /// </summary>
        /// <param name="data">The incoming data from serial port.</param>
        private void AddRawDataToConsole(byte[] data)
        {
            if (SerialMonitor.Document.Blocks.Count > 50)
                SerialMonitor.Document.Blocks.Remove(SerialMonitor.Document.Blocks.FirstBlock);

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
