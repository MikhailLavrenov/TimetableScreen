using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TimetableScreen.Infrastructure
{
    public class HorizontalRandomMoveBehavior :Behavior<FrameworkElement>
    {
        public int RandomBounds { get; set; }

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
            var random = new Random();

            var marging = AssociatedObject.Margin;

            marging.Left = random.Next(-RandomBounds, RandomBounds);

            AssociatedObject.Margin = marging;
        }
    }
}
