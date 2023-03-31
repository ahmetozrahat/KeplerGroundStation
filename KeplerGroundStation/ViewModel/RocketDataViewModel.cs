using LiveChartsCore.Defaults;
using LiveChartsCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;

namespace KeplerGroundStation.ViewModel
{
    public class RocketDataViewModel: INotifyPropertyChanged
    {

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

        /// <summary>
        /// An observable collection for holding the altitude data.
        /// </summary>
        private ObservableCollection<ObservableValue> _altitudeData;

        /// <summary>
        /// An observable collection for holding the pressure data.
        /// </summary>
        private ObservableCollection<ObservableValue> _pressureData;

        /// <summary>
        /// An observable collection for holding the pressure data.
        /// </summary>
        private ObservableCollection<ObservableValue> _temperatureData;

        public ObservableCollection<ISeries> AltitudeSeries { get; set; }

        public ObservableCollection<ISeries> PressureSeries { get; set; }

        public ObservableCollection<ISeries> TemperatureSeries { get; set; }

        public RocketDataViewModel()
        {
            _totalPackageNumber = 0;

            // Create altitude chart.
            _altitudeData = new ObservableCollection<ObservableValue>();
            AltitudeSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Name = "İrtifa",
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
                    Name = "Basınç",
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
                    Name = "Sıcaklık",
                    Values = _temperatureData,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = 2 },
                    DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0)
                }
            };
        }

        public int IncrementTotalPackageNumber()
        {
            return ++_totalPackageNumber;
        }

        /// <summary>
        /// Add altitude data and trim if the data exceeds the limits.
        /// </summary>
        /// <param name="altitude"></param>
        public void AddAltitudeData(double altitude)
        {
            _altitudeData.Add(new(altitude));
            if (_altitudeData.Count > 50)
                _altitudeData.RemoveAt(0);
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
