using System;
using System.Globalization;
using System.Windows.Data;

namespace F1Telemetry.WPF.Converter
{
    public class ToLapTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = TimeSpan.FromSeconds(double.Parse(value.ToString()));

            return $"{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds:000}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}