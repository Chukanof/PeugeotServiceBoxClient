using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfClient.Shell
{
    class BoolToVisibilityConverter : IValueConverter
    {
        static BoolToVisibilityConverter()
        {
            Instance = new BoolToVisibilityConverter();
        }

        public static BoolToVisibilityConverter Instance { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;

            if (val)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
