using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Jdh.Home.Mobile.Converters
{
    public class WifiImageConverter : IValueConverter
    {

        private readonly IEnumerable<WifiRange> _ranges = new HashSet<WifiRange>
        {
            new WifiRange(int.MinValue, 25, "WifiEmpty.png"),
            new WifiRange(26, 50, "WifiPoor.png"),
            new WifiRange(51, 75, "WifiMedium.png"),
            new WifiRange(76, int.MaxValue, "WifiFull.png")
        };
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) 
                throw new InvalidOperationException("Value must be int.");

            if (targetType != typeof(string)) 
                throw new InvalidOperationException("Target must be string.");

            return _ranges.First(r => r.Applies((int) value)).Image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private class WifiRange
        {
            public WifiRange(int lower, int upper, string image)
            {
                Lower = lower;
                Upper = upper;
                Image = image;
            }

            public int Lower { get; }
            public int Upper { get; }
            public string Image { get; }

            public bool Applies(int level) => 
                level >= Lower && level <= Upper;
        }
    }
}
