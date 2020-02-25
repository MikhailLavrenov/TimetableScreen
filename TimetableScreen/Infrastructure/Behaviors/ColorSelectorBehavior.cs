using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Media;

namespace TimetableScreen.Infrastructure
{
    public class ColorSelectorBehavior : Behavior<FrameworkElement>
    {
        int brushIndex = 0;

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(ColorPalette.colors300[brushIndex]);
            Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(ColorPalette.colors400[brushIndex]);
            Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(ColorPalette.colors500[brushIndex]);

            brushIndex = brushIndex == ColorPalette.Count - 1 ? 0 : brushIndex + 1;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= Handler;
        }

    }
}
