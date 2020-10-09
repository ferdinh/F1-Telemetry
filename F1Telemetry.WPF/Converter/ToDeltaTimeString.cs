using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace F1Telemetry.WPF.Converter
{
    public class ToDeltaTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = TimeSpan.FromSeconds(double.Parse(value.ToString()));
            var deltaTimeStringBuilder = new StringBuilder();

            if (time.TotalSeconds > 0)
            {
                deltaTimeStringBuilder.Append("+");
            }

            if (time.Minutes > 0)
            {
                deltaTimeStringBuilder.Append($"{time.Minutes:00}:");
            }

            deltaTimeStringBuilder.Append($"{time.Seconds:00}.{Math.Abs(time.Milliseconds):000}");

            return deltaTimeStringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}