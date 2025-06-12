//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rul.services
//{
//    internal class DiscountToBackgroundConverter
//    {
//    }
//}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Rul
{
    public class DiscountToBackgroundConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value is int ProductDiscountAmount  && ProductDiscountAmount > 15)
        //    {
        //        return Brushes.Red; // Красный фон для скидки > 15
        //    }
        //    return Brushes.Transparent; // Прозрачный фон для остальных
        //}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int ProductDiscountAmount && ProductDiscountAmount > 15)
            {
                return Brushes.Red;
            }
            return Brushes.Transparent;
        }
    }
}
