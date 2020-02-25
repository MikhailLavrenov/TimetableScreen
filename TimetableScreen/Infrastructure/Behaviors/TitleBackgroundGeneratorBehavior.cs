using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimetableScreen.Infrastructure
{
    public class TitleBackgroundGeneratorBehavior : Behavior<FrameworkElement>
    {
        private Border element;

        static SolidColorBrush[] brushes = new SolidColorBrush[]
        {
            GetBrush("#f44336"),
            GetBrush("#E91E63"),
            GetBrush("#9C27B0"),
            GetBrush("#673AB7"),
            GetBrush("#3F51B5"),
            GetBrush("#2196F3"),
            GetBrush("#03A9F4"),
            GetBrush("#00BCD4"),
            GetBrush("#009688"),
            GetBrush("#4CAF50"),
            GetBrush("#8BC34A"),
            GetBrush("#CDDC39"),
            //GetBrush("#FFEB3B"),
            GetBrush("#FFC107"),
            GetBrush("#FF9800"),
            GetBrush("#FF5722"),
            GetBrush("#795548"),
            //GetBrush("#9E9E9E"),
            GetBrush("#607D8B"),
        };

        int brushIndex = 0;

        protected override void OnAttached()
        {
            element = (Border)AssociatedObject;

            AssociatedObject.Loaded += Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            element.Background = brushes[brushIndex];

            //Application.Current.Resources["HeaderBackground"] = brushes[brushIndex];

            brushIndex = brushIndex == brushes.Length - 1 ? 0 : brushIndex + 1;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= Handler;
        }

        private static SolidColorBrush GetBrush(string hex)
        {
            var dColor = System.Drawing.ColorTranslator.FromHtml(hex);
            var mColor = Color.FromArgb(dColor.A, dColor.R, dColor.G, dColor.B);            

            return new SolidColorBrush(mColor) { Opacity=0.9};
        }
    }
}
