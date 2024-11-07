using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace RecipeMaster
{
    public class FavoriteConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? "Remove from Favorites" : "Add to Favorites";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

}
