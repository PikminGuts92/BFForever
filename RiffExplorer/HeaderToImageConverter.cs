using System;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using static System.IO.Path;
using BFForever.Riff;

// What a nice guy! :)
// http://www.codeproject.com/Articles/21248/A-Simple-WPF-Explorer-Tree 

namespace RiffExplorer
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = "pack://application:,,,/Assets/";
            
            if (value is Index2 || value is FEnvironment)
            {
                // Root
                path += "box.png";
            }
            else if (value is Index2Entry)
            {
                // Index2 entry
                Index2Entry entry = value as Index2Entry;

                if (entry.IsZObject())
                {
                    path += "page_white_text.png";
                    // TODO: Add a pretty switch statement
                }
                else
                {
                    path += "page_white.png";
                }
            }
            else
            {
                // Assume folder
                path += "folder_closed.png";
            }
            
            // Returns assigned icon
            return new BitmapImage(new Uri(path));
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
