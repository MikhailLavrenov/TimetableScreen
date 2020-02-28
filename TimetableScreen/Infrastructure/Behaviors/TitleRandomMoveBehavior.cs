using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TimetableScreen.Infrastructure
{   

    /// <summary>
    /// Сдвигает текст на произвольное кол-во точек чтобы снизить нагрузку одни и теже пиксели экрана
    /// </summary>
    public class TitleRandomMoveBehavior:Behavior<FrameworkElement>
    {
        private static Random random = new Random();

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {            
            var randomBounds=(int)((TextBlock)AssociatedObject).FontSize/2;

            var marging = AssociatedObject.Margin;

            marging.Top = random.Next(-randomBounds, randomBounds);

            AssociatedObject.Margin = marging;
        }
    }
}
