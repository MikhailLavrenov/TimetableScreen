using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Media;

namespace TimetableScreen.Infrastructure
{
    public class ColorSelectorBehavior : Behavior<FrameworkElement>
    {
        int brushIndex = 0;
        int gradientIndex = 0;

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(ColorPalette.colors300[brushIndex]);
            Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(ColorPalette.colors400[brushIndex]);
            Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(ColorPalette.colors500[brushIndex]);

            var color1 = ColorPalette.gradientsColors[gradientIndex].Item1;
            var color2 = ColorPalette.gradientsColors[gradientIndex].Item2;

            Application.Current.Resources["PrimaryGradient"] = new LinearGradientBrush(color1, color2, 45);

            brushIndex = brushIndex == ColorPalette.ColorsCount - 1 ? 0 : brushIndex + 1;
            gradientIndex = gradientIndex == ColorPalette.GradientsCount - 1 ? 0 : gradientIndex + 1;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= Handler;
        }

    }
}
