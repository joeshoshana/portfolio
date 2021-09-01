using Shkila.ScaleReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Basalt_v2.Converters
{
    public class TypeToVisibilty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null)
                return Visibility.Collapsed;

            ConnectionType val;
            if(Enum.TryParse(value.ToString(), out val))
            {
                ConnectionType val2;
                if (Enum.TryParse(parameter.ToString(), out val2))
                {
                    if (val == val2)
                    {
                        return Visibility.Visible;
                    }
                }

                return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
