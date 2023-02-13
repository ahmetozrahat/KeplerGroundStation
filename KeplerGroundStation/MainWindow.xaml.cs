using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;
using System.IO.Ports;
using KeplerGroundStation.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;

namespace KeplerGroundStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        SerialPort serialPort = new SerialPort();

        private int _totalPackageNumber;
        public int TotalPackageNumber
        {
            get { return _totalPackageNumber; }
            set
            {
                _totalPackageNumber = value;
                OnPropertyChanged(nameof(TotalPackageNumber));
            }
        }

        private string _currentTime;
        /// <summary>
        /// Current time information.
        /// </summary>
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        private string _currentDate;
        /// <summary>
        /// Current date information.
        /// </summary>
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }
        
        /// <summary>
        /// An observable collection for holding the altitude data.
        /// </summary>
        private readonly ObservableCollection<ObservableValue> _altitudeData;
        public ObservableCollection<ISeries> AltitudeSeries { get; set; }

        /// <summary>
        /// An observable collection for holding the pressure data.
        /// </summary>
        private readonly ObservableCollection<ObservableValue> _pressureData;
        public ObservableCollection<ISeries> PressureSeries { get; set; }

        /// <summary>
        /// An observable collection for holding the pressure data.
        /// </summary>
        private readonly ObservableCollection<ObservableValue> _temperatureData;
        public ObservableCollection<ISeries> TemperatureSeries { get; set; }

        private ObservableCollection<string> _serialPorts;
        public ObservableCollection<string> SerialPorts
        {
            get { return _serialPorts; }
            set
            {
                _serialPorts = value;
                OnPropertyChanged(nameof(SerialPorts));
            }
        }

        private string _selectedSerialPort1;
        public string SelectedSerialPort1
        {
            get { return _selectedSerialPort1; }
            set
            {
                _selectedSerialPort1 = value;
                OnPropertyChanged(nameof(SelectedSerialPort1));
            }
        }

        private string _selectedSerialPort2;
        public string SelectedSerialPort2
        {
            get { return _selectedSerialPort2; }
            set
            {
                _selectedSerialPort2 = value;
                OnPropertyChanged(nameof(SelectedSerialPort2));
            }
        }

        /// <summary>
        /// List of all available baud rates to use it in Serial communication.
        /// </summary>
        private ObservableCollection<int> _baudRates;
        public ObservableCollection<int> BaudRates
        {
            get { return _baudRates; }
            set
            {
                _baudRates = value;
                OnPropertyChanged(nameof(BaudRates));
            }
        }

        private int _selectedBaudRate1;
        public int SelectedBaudRate1
        {
            get { return _selectedBaudRate1; }
            set
            {
                _selectedBaudRate1 = value;
                OnPropertyChanged(nameof(SelectedBaudRate1));
            }
        }

        private int _selectedBaudRate2;
        public int SelectedBaudRate2
        {
            get { return _selectedBaudRate2; }
            set
            {
                _selectedBaudRate2 = value;
                OnPropertyChanged(nameof(SelectedBaudRate2));
            }
        }

        /// <summary>
        /// List of all available parities to use it in Serial communication.
        /// </summary>
        private ObservableCollection<Parity> _parities;
        public ObservableCollection<Parity> Parities
        {
            get { return _parities; }
            set
            {
                _parities = value;
                OnPropertyChanged(nameof(Parities));
            }
        }

        private Parity _selectedParity1;
        public Parity SelectedParity1
        {
            get { return _selectedParity1; }
            set
            {
                _selectedParity1 = value;
                OnPropertyChanged(nameof(SelectedParity1));
            }
        }

        private Parity _selectedParity2;
        public Parity SelectedParity2
        {
            get { return _selectedParity2; }
            set
            {
                _selectedParity2 = value;
                OnPropertyChanged(nameof(SelectedParity2));
            }
        }

        private ObservableCollection<int> _dataBits;
        public ObservableCollection<int> DataBits
        {
            get { return _dataBits; }
            set
            {
                _dataBits = value;
                OnPropertyChanged(nameof(DataBits));
            }
        }

        private int _selectedDataBits1;
        public int SelectedDataBits1
        {
            get { return _selectedDataBits1; }
            set
            {
                _selectedDataBits1 = value;
                OnPropertyChanged(nameof(SelectedDataBits1));
            }
        }

        private int _selectedDataBits2;
        public int SelectedDataBits2
        {
            get { return _selectedDataBits2; }
            set
            {
                _selectedDataBits2 = value;
                OnPropertyChanged(nameof(SelectedDataBits2));
            }
        }

        private ObservableCollection<StopBits> _stopBits;
        public ObservableCollection<StopBits> StopBits
        {
            get { return _stopBits; }
            set
            {
                _stopBits = value;
                OnPropertyChanged(nameof(StopBits));
            }
        }

        private StopBits _selectedStopBits1;
        public StopBits SelectedStopBits1
        {
            get { return _selectedStopBits1; }
            set
            {
                _selectedStopBits1 = value;
                OnPropertyChanged(nameof(SelectedStopBits1));
            }
        }

        private StopBits _selectedStopBits2;
        public StopBits SelectedStopBits2
        {
            get { return _selectedStopBits2; }
            set
            {
                _selectedStopBits2 = value;
                OnPropertyChanged(nameof(SelectedStopBits2));
            }
        }

        private bool _isFlightComputerConnected;
        public bool IsFlightComputerConnected
        {
            get { return _isFlightComputerConnected;}
            set
            {
                _isFlightComputerConnected = value;
                OnPropertyChanged(nameof(IsFlightComputerConnected));
            }
        }

        private bool _isRefereeComputerConnected;
        public bool IsRefereeComputerConnected
        {
            get { return _isRefereeComputerConnected; }
            set
            {
                _isRefereeComputerConnected = value;
                OnPropertyChanged(nameof(IsRefereeComputerConnected));
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

        private string _flightComputerStateString;
        public string FlightComputerStateString
        {
            get { return _flightComputerStateString; }
            set
            {
                _flightComputerStateString = value;
                OnPropertyChanged(nameof(FlightComputerStateString));
            }
        }

        private SolidColorBrush _flightComputerStateColor;
        public SolidColorBrush FlightComputerStateColor
        {
            get { return _flightComputerStateColor; }
            set
            {
                _flightComputerStateColor = value;
                OnPropertyChanged(nameof(FlightComputerStateColor));
            }
        }

        private string _refereeComputerStateString;
        public string RefereeComputerStateString
        {
            get { return _refereeComputerStateString; }
            set
            {
                _refereeComputerStateString = value;
                OnPropertyChanged(nameof(RefereeComputerStateString));
            }
        }

        private SolidColorBrush _refereeComputerStateColor;
        public SolidColorBrush RefereeComputerStateColor
        {
            get { return _refereeComputerStateColor; }
            set
            {
                _refereeComputerStateColor = value;
                OnPropertyChanged(nameof(RefereeComputerStateColor));
            }
        }

        // ----- Acceleration Data Variables for Rocket Computer -----

        private double _accelerationX;
        public double AccelerationX
        {
            get { return _accelerationX; }
            set
            {
                _accelerationX = value;
                OnPropertyChanged(nameof(AccelerationX));
            }
        }

        private ObservableValue _accelerationY;
        public ObservableValue AccelerationY
        {
            get { return _accelerationY; }
            set
            {
                _accelerationY = value;
                OnPropertyChanged(nameof(AccelerationY));
            }
        }

        private ObservableValue _accelerationZ;
        public ObservableValue AccelerationZ
        {
            get { return _accelerationZ; }
            set
            {
                _accelerationZ = value;
                OnPropertyChanged(nameof(AccelerationZ));
            }
        }

        // ---------------------------------------------------------

        // XAxes for altitude chart.
        public Axis[] XAxesAltitude { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 12,
                    SeparatorsPaint = null
                }
            };

        // YAxes for altitude chart.
        public Axis[] YAxesAltitude { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 12,
                    MinLimit = 900,
                    MaxLimit = 3000,
                    SeparatorsPaint = null
                }
            };

        // XAxes for pressure chart.
        public Axis[] XAxesPressure { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 12,
                    SeparatorsPaint = null
                }
            };

        // YAxes for pressure chart.
        public Axis[] YAxesPressure { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 12,
                    MinLimit = 300,
                    MaxLimit = 1100,
                    SeparatorsPaint = null
                }
            };

        // XAxes for temperature chart.
        public Axis[] XAxesTemperature { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 12,
                    SeparatorsPaint = null
                }
            };

        // YAxes for temperature chart.
        public Axis[] YAxesTemperature { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 12,
                    MinLimit = -40,
                    MaxLimit = 85,
                    SeparatorsPaint = null
                }
            };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            TotalPackageNumber = 0;

            FlightComputerStateString = "Bağlı Değil";
            FlightComputerStateColor = new SolidColorBrush(Color.FromRgb(237, 67, 55));
            IsFlightComputerConnected = false;

            RefereeComputerStateString = "Bağlı Değil";
            RefereeComputerStateColor = new SolidColorBrush(Color.FromRgb(237, 67, 55));
            IsRefereeComputerConnected = false;

            _connectFlightComputerClicked = new RelayCommand(HandleConnectFlightComputerClicked);
            _disconnectFlightComputerClicked = new RelayCommand(HandleDisconnectFlightComputerClicked);

            // Get all available comm ports.
            SerialPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            BaudRates = new ObservableCollection<int>(KeplerBaudRate.BaudRates);
            Parities = new ObservableCollection<Parity>((Parity[]) Enum.GetValues(typeof(Parity)));
            DataBits = new ObservableCollection<int>(KeplerDataBits.DataBits);
            StopBits = new ObservableCollection<StopBits>((StopBits[])Enum.GetValues(typeof(StopBits)));

            if (SerialPorts.Count > 0)
            {
                _selectedSerialPort1 = SerialPorts[0];
                _selectedSerialPort2 = SerialPorts[0];
            }
            _selectedBaudRate1 = BaudRates[5];
            _selectedBaudRate2 = BaudRates[5];
            _selectedParity1 = Parities[0];
            _selectedParity2 = Parities[0];
            _selectedDataBits1 = DataBits[DataBits.Count-1];
            _selectedDataBits2 = DataBits[DataBits.Count-1];
            _selectedStopBits1 = StopBits[1];
            _selectedStopBits2 = StopBits[1];

            // Create altitude chart.
            _altitudeData = new ObservableCollection<ObservableValue>();
            AltitudeSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = _altitudeData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = 2 },
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };

            // Create pressure chart.
            _pressureData = new ObservableCollection<ObservableValue>();
            PressureSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = _pressureData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = 2 },
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };

            // Create temperature chart.
            _temperatureData = new ObservableCollection<ObservableValue>();
            TemperatureSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = _temperatureData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = 2 },
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };

            // Setup timer for updating the UI.
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Updates the UI with randomly generated values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs args)
        {
            // Set time information.
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            CurrentDate = DateTime.Now.ToString("d MMMM yyyy");

            FlightComputerStateString = IsFlightComputerConnected ? "Bağlandı" : "Bağlı Değil";
            FlightComputerStateColor = IsFlightComputerConnected ?
                new SolidColorBrush(Color.FromRgb(50, 205, 50)) :
                new SolidColorBrush(Color.FromRgb(237, 67, 55));
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

        private void HandleConnectFlightComputerClicked()
        {
            string portName = _selectedSerialPort1;
            int baudRate = _selectedBaudRate1;
            Parity parity = _selectedParity1;
            int dataBits = _selectedDataBits1;
            StopBits stopBits = _selectedStopBits1;
            

            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.Parity = parity;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = stopBits;

            try
            {
                serialPort.Open();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                IsFlightComputerConnected = true;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //MessageBox.Show("Port Name: " + portName + "\n" + "Baud Rate: " + baudRate + "\n" + "Parity: " + parity + "\n" + "Data Bits: " + dataBits + "\n" + "Stop Bits: " + stopBits);
        }

        private void HandleDisconnectFlightComputerClicked()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    IsFlightComputerConnected = false;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            
            try
            {
                string indata = sp.ReadLine();
                string[] data = indata.Split(",");
                int packageId = int.Parse(data[0]);
                double altitude = double.Parse(data[1], CultureInfo.InvariantCulture);
                double pressure = double.Parse(data[2], CultureInfo.InvariantCulture) / 100.0;
                double temperature = double.Parse(data[3], CultureInfo.InvariantCulture);
                double gpsLat = double.Parse(data[4], CultureInfo.InvariantCulture);
                double gpsLong = double.Parse(data[5], CultureInfo.InvariantCulture);
                double accelx = double.Parse(data[6], CultureInfo.InvariantCulture);
                double accely = double.Parse(data[7], CultureInfo.InvariantCulture);
                double accelz = double.Parse(data[8], CultureInfo.InvariantCulture);
                double gyrox = double.Parse(data[9], CultureInfo.InvariantCulture);
                double gyroy = double.Parse(data[10], CultureInfo.InvariantCulture);
                double gyroz = double.Parse(data[11], CultureInfo.InvariantCulture);

                _altitudeData.Add(new(altitude));
                if (_altitudeData.Count > 50) { _altitudeData.RemoveAt(0); }

                _pressureData.Add(new(pressure));
                if (_pressureData.Count > 50) { _pressureData.RemoveAt(0); }

                _temperatureData.Add(new(temperature));
                if (_temperatureData.Count > 50) { _temperatureData.RemoveAt(0); }

                this.Dispatcher.Invoke(() =>
                {
                    TotalPackageCount.Content = ++TotalPackageNumber;
                    PackageId.Content = packageId;
                    AltitudeDesc.Content = altitude + " metre";
                    PressureDesc.Content = pressure + " hPa";
                    TemperatureDesc.Content = temperature + " °C";
                    RocketGPSLat.Content = gpsLat;
                    RocketGPSLong.Content = gpsLong;
                    KeplerMap.Center = new Location(gpsLat, gpsLong);
                    KeplerMap.ZoomLevel = 17;
                    Pushpin pin = new Pushpin();
                    pin.Location = new Location(gpsLat, gpsLong);
                    KeplerMap.Children.Clear();
                    KeplerMap.Children.Add(pin);
                    AccelXLabel.Content = accelx + " m/s";
                    AccelYLabel.Content = accely + " m/s";
                    AccelZLabel.Content = accelz + " m/s";
                    GyroXLabel.Content = gyrox + " rad/s";
                    GyroYLabel.Content = gyroy + " rad/s";
                    GyroZLabel.Content = gyroz + " rad/s";

                    DateTime dateTime = DateTime.Now;

                    SerialMonitor.AppendText("[" + dateTime.ToString("HH:mm:ss") + "]: " + indata + "\n");
                    SerialMonitor.ScrollToEnd();
                }
                );
            } 
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
