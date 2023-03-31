using KeplerGroundStation.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace KeplerGroundStation.ViewModel
{
    public class SerialPortViewModel : INotifyPropertyChanged
    {
        private SerialPort _serialPort;

        private string _selectedSerialPort;
        public string SelectedSerialPort
        {
            get { return _selectedSerialPort; }
            set
            {
                _selectedSerialPort = value;
                OnPropertyChanged(nameof(SelectedSerialPort));
            }
        }

        private int _selectedBaudRate;
        public int SelectedBaudRate
        {
            get { return _selectedBaudRate; }
            set
            {
                _selectedBaudRate = value;
                OnPropertyChanged(nameof(SelectedBaudRate));
            }
        }

        private Parity _selectedParity;
        public Parity SelectedParity
        {
            get { return _selectedParity; }
            set
            {
                _selectedParity = value;
                OnPropertyChanged(nameof(SelectedParity));
            }
        }

        private int _selectedDataBits;
        public int SelectedDataBits
        {
            get { return _selectedDataBits; }
            set
            {
                _selectedDataBits = value;
                OnPropertyChanged(nameof(SelectedDataBits));
            }
        }

        private StopBits _selectedStopBits;
        public StopBits SelectedStopBits
        {
            get { return _selectedStopBits; }
            set
            {
                _selectedStopBits = value;
                OnPropertyChanged(nameof(SelectedStopBits));
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private SerialDataReceivedEventHandler _serialDataReceivedEventHandler;
        public SerialDataReceivedEventHandler SerialDataReceivedEventHandler
        {
            get { return _serialDataReceivedEventHandler; }
            set
            {
                _serialDataReceivedEventHandler = value;
                OnPropertyChanged(nameof(SerialDataReceivedEventHandler));
            }
        }

        public ObservableCollection<string> SerialPorts { get; } = new ObservableCollection<string>();

        public ObservableCollection<int> BaudRates { get; } = new ObservableCollection<int>();

        public ObservableCollection<Parity> Parities { get; } = new ObservableCollection<Parity>();

        public ObservableCollection<int> DataBits { get; } = new ObservableCollection<int>();

        public ObservableCollection<StopBits> StopBits { get; } = new ObservableCollection<StopBits>();

        public SerialPortViewModel()
        {
            SerialPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            BaudRates = new ObservableCollection<int>(KeplerBaudRate.BaudRates);
            Parities = new ObservableCollection<Parity>((Parity[])Enum.GetValues(typeof(Parity)));
            DataBits = new ObservableCollection<int>(KeplerDataBits.DataBits);
            StopBits = new ObservableCollection<StopBits>((StopBits[])Enum.GetValues(typeof(StopBits)));

            // Set default values
            SelectedSerialPort = SerialPorts.FirstOrDefault();
            SelectedBaudRate = BaudRates[6];
            SelectedParity = Parities.FirstOrDefault();
            SelectedDataBits = DataBits.LastOrDefault();
            SelectedStopBits = StopBits[1];

            IsConnected = false;
        }

        public SerialPortViewModel(SerialDataReceivedEventHandler serialDataReceivedEventHandler)
        {
            SerialPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            BaudRates = new ObservableCollection<int>(KeplerBaudRate.BaudRates);
            Parities = new ObservableCollection<Parity>((Parity[])Enum.GetValues(typeof(Parity)));
            DataBits = new ObservableCollection<int>(KeplerDataBits.DataBits);
            StopBits = new ObservableCollection<StopBits>((StopBits[])Enum.GetValues(typeof(StopBits)));

            // Set default values
            SelectedSerialPort = SerialPorts.FirstOrDefault();
            SelectedBaudRate = BaudRates[6];
            SelectedParity = Parities.FirstOrDefault();
            SelectedDataBits = DataBits.LastOrDefault();
            SelectedStopBits = StopBits[1];

            _serialDataReceivedEventHandler = serialDataReceivedEventHandler;

            IsConnected = false;
        }

        public void Open()
        {
            _serialPort = new SerialPort(SelectedSerialPort, SelectedBaudRate, SelectedParity, SelectedDataBits, SelectedStopBits);
            try
            {
                _serialPort.Open();
                IsConnected = true;

                if (_serialDataReceivedEventHandler != null)
                {
                    _serialPort.DataReceived += _serialDataReceivedEventHandler;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    IsConnected = false;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void WriteBytes(byte[] bytes)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
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
