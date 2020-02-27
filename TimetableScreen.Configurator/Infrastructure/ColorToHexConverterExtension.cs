using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace TimetableScreen.Configurator.Infrastructure
{
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToHexConverterExtension : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            var dColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

            return System.Drawing.ColorTranslator.ToHtml(dColor);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hex = (string)value;

            if (string.IsNullOrEmpty(hex) || hex.Length != 7 || hex[0] != '#')
                return Colors.Gray;

            return (Color)ColorConverter.ConvertFromString(hex);
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
