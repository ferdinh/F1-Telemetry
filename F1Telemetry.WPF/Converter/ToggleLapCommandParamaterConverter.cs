using System;
using System.Globalization;
using System.Windows.Data;

namespace F1Telemetry.WPF.Converter
{
    public class ToggleLapCommandParamaterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)values[0], (int)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}