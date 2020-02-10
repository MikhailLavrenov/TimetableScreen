using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TimetableScreen.Configurator.Infrastructure;

namespace TimetableScreen
{
    public class CircleAnimationBehavior : Behavior<FrameworkElement>
    {
        protected FrameworkElement AnimatedElement { get; set; }
        //элемент относительно которого расчитываются параметры (размеры) анимации
        protected FrameworkElement AnimationParametersTargetElement { get; set; }
        /// <summary>
        /// Имя элемента относительно которого расчитываются параметры (размеры) анимации
        /// </summary>
        public string AnimationParametersTarget { get; set; }

        private ScreenViewModel dataContext;

        protected override void OnAttached()
        {
            AnimatedElement = AssociatedObject;
            AnimationParametersTargetElement = AssociatedObject.FindLogicalParent(AnimationParametersTarget);

            dataContext = (ScreenViewModel)AssociatedObject.DataContext;

            dataContext.PropertyChanged += EventHandler;
        }
        protected override void OnDetaching()
        {
            dataContext.PropertyChanged -= EventHandler;
        }
        protected void EventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(dataContext.CurrentPage))
                return;

            var elipseGeometry = new EllipseGeometry(Mouse.GetPosition(AnimatedElement), 0, 0);
            var point = Mouse.GetPosition(AnimationParametersTargetElement);
            var x = Math.Max(point.X, AnimationParametersTargetElement.ActualWidth - point.X);
            var y = Math.Max(point.Y, AnimationParametersTargetElement.ActualHeight - point.Y);
            var radius = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            AnimatedElement.Clip = elipseGeometry;

            var duration = TimeSpan.FromMilliseconds(500);

            var ease = new ExponentialEase
            {
                EasingMode = EasingMode.EaseIn,
                Exponent = 1.5
            };

            var animationOpacity = new DoubleAnimation(0, 1, duration);
            animationOpacity.EasingFunction = ease;
            AnimatedElement.BeginAnimation(UIElement.OpacityProperty, animationOpacity);

            var animationX = new DoubleAnimation(0, radius, duration);
            animationX.EasingFunction = ease;
            elipseGeometry.BeginAnimation(EllipseGeometry.RadiusXProperty, animationX);

            var animationY = new DoubleAnimation(0, radius, duration);
            animationY.EasingFunction = ease;
            animationY.Completed += (sndr, args) => AnimatedElement.Clip = null;
            elipseGeometry.BeginAnimation(EllipseGeometry.RadiusYProperty, animationY);
        }
    }
}
