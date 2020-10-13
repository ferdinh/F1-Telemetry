using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace F1Telemetry.WPF.Converter
{
    public class ToLapTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = TimeSpan.FromSeconds(double.Parse(value.ToString()));
            var lapTimeStringBuilder = new StringBuilder();

            if (time.Minutes > 0)
            {
                lapTimeStringBuilder.Append($"{time.Minutes:00}:");
            }

            lapTimeStringBuilder.Append($"{time.Seconds:00}.{Math.Abs(time.Milliseconds):000}");

            return lapTimeStringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}