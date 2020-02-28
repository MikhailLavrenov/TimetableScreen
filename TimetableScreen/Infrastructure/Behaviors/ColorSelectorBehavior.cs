using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Infrastructure
{
    public class ColorSelectorBehavior : Behavior<FrameworkElement>
    {
        public static DependencyProperty ColorPairsProperty { get; set; }

        static ColorSelectorBehavior()
        {
            ColorPairsProperty = DependencyProperty.Register(
                    "ColorPairs",
                    typeof(ObservableCollection<ColorPair>),
                    typeof(ColorSelectorBehavior));
        }

        public ObservableCollection<ColorPair> ColorPairs { get => (ObservableCollection<ColorPair>)GetValue(ColorPairsProperty); set => SetValue(ColorPairsProperty, value); }

        int index = 0;
        Point point1 = new Point(0, 0);
        Point point2 = new Point(1, 1);

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += Handler;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            if (ColorPairs?.Count <= 0)
                return;

            var color1 = ColorPairs[index].Color1;
            var color2 = ColorPairs[index].Color2;

            Application.Current.Resources["PrimaryGradient"] = new LinearGradientBrush(color1, color2, point1, point2);

            if (index == ColorPairs.Count - 1)
            {
                index = 0;

                //меняем направление градиента на противоположное, чтобы равномерно распределить нагрузку на пиксели
                (point1, point2) = (point2, point1);
            }
            else
                index++;
        }

    }
}
