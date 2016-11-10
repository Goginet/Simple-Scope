using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Simple_Scope.Windows {
    [ValueConversion(typeof(string), typeof(SpaceObject))]
    public class SpaceObjectToStringConverter : IValueConverter {
        public Universe Universe { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            SpaceObject obj = value as SpaceObject;
            if (obj != null) {
                return (value as SpaceObject).Name;
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Universe.GetObjectByName((string)value);
        }
    }
}
