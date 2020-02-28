using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace TimetableScreen.Infrastructure
{
    public class HorizontalRandomMoveBehavior : Behavior<FrameworkElement>
    {        
        private Random random = new Random();
        public static DependencyProperty ScaleProperty { get; set; }
        public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }
        public int RandomBounds { get; set; }

        static HorizontalRandomMoveBehavior()
        {
            ScaleProperty = DependencyProperty.Register(
                               "Scale",
                               typeof(double),
                               typeof(HorizontalRandomMoveBehavior));
        }

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
        }

        protected void OnLoaded(object sender, EventArgs e)
        {
            var marging = AssociatedObject.Margin;

            marging.Left = random.Next(-RandomBounds, RandomBounds)*Scale;

            AssociatedObject.Margin = marging;
        }
    }
}
