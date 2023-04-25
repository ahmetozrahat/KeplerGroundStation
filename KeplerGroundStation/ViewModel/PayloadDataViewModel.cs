using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KeplerGroundStation.ViewModel
{
    public class PayloadDataViewModel: INotifyPropertyChanged
    {

        private int _totalPackageNumber;

        /// <summary>
        /// Total package number of payload data.
        /// </summary>
        public int TotalPackageNumber
        {
            get { return _totalPackageNumber; }
            set
            {
                _totalPackageNumber = value;
                OnPropertyChanged(nameof(TotalPackageNumber));
            }
        }

        /// <summary>
        /// An observable collection for holding the pressure data.
        /// </summary>
        private ObservableCollection<ObservableValue> _pressureData;

        /// <summary>
        /// An observable collection for holding the humidity data.
        /// </summary>
        private ObservableCollection<ObservableValue> _humidityData;

        /// <summary>
        /// An observable collection for holding the temperature data.
        /// </summary>
        private ObservableCollection<ObservableValue> _temperatureData;

        public ObservableCollection<ISeries> PressureSeries { get; set; }

        public ObservableCollection<ISeries> HumiditySeries { get; set; }

        public ObservableCollection<ISeries> TemperatureSeries { get; set; }

        public PayloadDataViewModel()
        {
            // Create humidity chart.
            _pressureData = new ObservableCollection<ObservableValue>();
            PressureSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Name = "Basınç",
                    Values = _pressureData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Magenta) { StrokeThickness = 2 },
                    Fill = new SolidColorPaint(SKColors.Magenta.WithAlpha(30)),
                    GeometryStroke = new SolidColorPaint(SKColors.Magenta),
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };

            // Create humidity chart.
            _humidityData = new ObservableCollection<ObservableValue>();
            HumiditySeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Name = "Bağıl Nem",
                    Values = _humidityData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Magenta) { StrokeThickness = 2 },
                    Fill = new SolidColorPaint(SKColors.Magenta.WithAlpha(30)),
                    GeometryStroke = new SolidColorPaint(SKColors.Magenta),
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };

            // Create temperature chart.
            _temperatureData = new ObservableCollection<ObservableValue>();
            TemperatureSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Name = "Sıcaklık",
                    Values = _temperatureData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Magenta) { StrokeThickness = 2 },
                    Fill = new SolidColorPaint(SKColors.Magenta.WithAlpha(30)),
                    GeometryStroke = new SolidColorPaint(SKColors.Magenta),
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };
        }

        public int IncrementTotalPackageNumber()
        {
            return ++_totalPackageNumber;
        }

        /// <summary>
        /// Add pressure data and trim if the data exceeds the limits.
        /// </summary>
        /// <param name="pressure"></param>
        public void AddPressureData(double pressure)
        {
            _pressureData.Add(new(pressure));
            if (_pressureData.Count > 50)
                _pressureData.RemoveAt(0);
        }

        /// <summary>
        /// Add humidity data and trim if the data exceeds the limits.
        /// </summary>
        /// <param name="humidity"></param>
        public void AddHumidityData(double humidity)
        {
            _humidityData.Add(new(humidity));
            if (_humidityData.Count > 50)
                _humidityData.RemoveAt(0);
        }

        /// <summary>
        /// Add temperature data and trim if the data exceeds the limits.
        /// </summary>
        /// <param name="temperature"></param>
        public void AddTemperatureData(double temperature)
        {
            _temperatureData.Add(new(temperature));
            if (_temperatureData.Count > 50)
                _temperatureData.RemoveAt(0);
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
