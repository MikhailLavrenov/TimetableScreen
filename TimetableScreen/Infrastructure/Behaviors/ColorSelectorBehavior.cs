using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Media;

namespace TimetableScreen.Infrastructure
{
    public class ColorSelectorBehavior : Behavior<FrameworkElement>
    {
        //int brushIndex = 0;
        int gradientIndex = 0;
        bool invert = false;

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            //Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(ColorPalette.colors300[brushIndex]);
            //Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(ColorPalette.colors400[brushIndex]);
            //Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(ColorPalette.colors500[brushIndex]);

            var color1 = ColorPalette.gradientsColors[gradientIndex].Item1;
            var color2 = ColorPalette.gradientsColors[gradientIndex].Item2;
            var point1 = new Point(0, 0);
            var point2 = new Point(1, 1);

            if (invert)
                (point1, point2) = (point2, point1);

            Application.Current.Resources["PrimaryGradient"] = new LinearGradientBrush(color1, color2, point1, point2);

            //brushIndex = brushIndex == ColorPalette.ColorsCount - 1 ? 0 : brushIndex + 1;

            if (gradientIndex == ColorPalette.GradientsCount - 1)
            {
                gradientIndex = 0;
                invert = !invert;
            }
            else
                gradientIndex++;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= Handler;
        }

    }
}
