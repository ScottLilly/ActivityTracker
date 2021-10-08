using System;
using System.Globalization;
using System.Windows.Data;

namespace ActivityTracker.CustomConverters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TimeSpan duration)
            {
                try
                {
                    return string.Format(GetFormat(duration), duration);
                }
                catch(Exception exc)
                {
                    var msg = exc.Message;
                    return "null";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetFormat(TimeSpan duration)
        {
            return duration switch
            {
                { Days: > 0 } => "{0:dd}day(s) {0:hh}h {0:mm}min {0:ss}sec",
                { Hours: > 0 } => "{0:hh}h {0:mm}min {0:ss}sec",
                { Minutes: > 0 } => "{0:mm}min {0:ss}sec",
                //_ => "{0:ss} sec"
                _ => "{0:hh}h {0:mm}min {0:ss}sec"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
