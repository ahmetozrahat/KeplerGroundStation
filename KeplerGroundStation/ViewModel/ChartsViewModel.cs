﻿using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.ComponentModel;

namespace KeplerGroundStation.ViewModel
{
    public class ChartsViewModel: INotifyPropertyChanged
    {
        // XAxes for altitude chart.
        public Axis[] XAxesAltitude { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
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
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
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
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
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
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
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
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
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
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    MinLimit = -40,
                    MaxLimit = 85,
                    SeparatorsPaint = null
                }
            };

        // --------------------------------------------------------------------

        // XAxes for payload pressure chart.
        public Axis[] XAxesPayloadPressure { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    SeparatorsPaint = null
                }
            };

        // YAxes for payload pressure chart.
        public Axis[] YAxesPayloadPressure { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    MinLimit = 300,
                    MaxLimit = 1100,
                    SeparatorsPaint = null
                }
            };

        // XAxes for humidity chart.
        public Axis[] XAxesHumidity { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    SeparatorsPaint = null
                }
            };

        // YAxes for humidity chart.
        public Axis[] YAxesHumidity { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    MinLimit = 0,
                    MaxLimit = 100,
                    SeparatorsPaint = null
                }
            };

        // XAxes for payload temperature chart.
        public Axis[] XAxesPayloadTemperature { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    SeparatorsPaint = null
                }
            };

        // YAxes for payload temperature chart.
        public Axis[] YAxesPayloadTemperature { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    TextSize = 12,
                    MinLimit = -40,
                    MaxLimit = 85,
                    SeparatorsPaint = null
                }
            };

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
