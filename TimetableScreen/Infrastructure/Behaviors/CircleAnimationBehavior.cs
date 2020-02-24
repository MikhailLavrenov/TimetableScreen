using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.ViewModels;

namespace TimetableScreen.Infrastructure
{
    public class CircleAnimationBehavior : Behavior<FrameworkElement>
    {
        private FrameworkElement animatedElement;
        //элемент относительно которого расчитываются параметры (размеры) анимации
        private FrameworkElement animationParametersTargetElement;
        private TimetableViewModel dataContext;
        /// <summary>
        /// Имя элемента относительно которого расчитываются параметры (размеры) анимации
        /// </summary>
        public string AnimationParametersTarget { get; set; }

        protected override void OnAttached()
        {
            animatedElement = AssociatedObject;
            animationParametersTargetElement = animatedElement.FindLogicalParent(AnimationParametersTarget);

            dataContext = (TimetableViewModel)AssociatedObject.DataContext;
            dataContext.PropertyChanged += EventHandler;
        }
        protected override void OnDetaching()
        {
            dataContext.PropertyChanged -= EventHandler;
        }
        protected void EventHandler(object sender, PropertyChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (e.PropertyName != nameof(dataContext.CurrentPage))
                    return;

                var scale = dataContext.Settings.Scale;

                var center = new Point(0, 0);
                var elipseGeometry = new EllipseGeometry(center, 0, 0);
                var x = animationParametersTargetElement.ActualWidth;
                var y = animationParametersTargetElement.ActualHeight;
                var radius = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                animatedElement.Clip = elipseGeometry;

                var duration = TimeSpan.FromMilliseconds(1500);

                var ease = new ExponentialEase
                {
                    EasingMode = EasingMode.EaseIn,
                    Exponent = 0.5
                };

                var animationOpacity = new DoubleAnimation(0, 1, duration);
                animationOpacity.EasingFunction = ease;
                animatedElement.BeginAnimation(UIElement.OpacityProperty, animationOpacity);

                var ease2 = new ExponentialEase
                {
                    EasingMode = EasingMode.EaseIn,
                    Exponent = 1.3
                };

                var animationX = new DoubleAnimation(0, radius, duration);
                animationX.EasingFunction = ease2;
                elipseGeometry.BeginAnimation(EllipseGeometry.RadiusXProperty, animationX);

                var animationY = new DoubleAnimation(0, radius, duration);
                animationY.EasingFunction = ease2;
                animationY.Completed += (sndr, args) => animatedElement.Clip = null;
                elipseGeometry.BeginAnimation(EllipseGeometry.RadiusYProperty, animationY);

            });
        }
    }
}
