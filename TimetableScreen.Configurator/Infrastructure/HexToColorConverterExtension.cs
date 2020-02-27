using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.Infrastructure
{
    [ValueConversion(typeof(ColorsPair), typeof(LinearGradientBrush))]
    public class HexToColorConverterExtension : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorsPair = value as ColorsPair;

            if (colorsPair == default) return null;

            return new LinearGradientBrush(colorsPair.Brush1,  colorsPair.Brush2, 0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
