using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TimetableScreen.ViewModels;

namespace TimetableScreen.Infrastructure
{
    class ProgressBarBehavior : Behavior<FrameworkElement>
    {
        INotifyPropertyChanged dataContext;

        public static DependencyProperty TimeIntervalProperty { get; set; }
        public int TimeInterval { get => (int)GetValue(TimeIntervalProperty); set => SetValue(TimeIntervalProperty, value); }

        static ProgressBarBehavior()
        {
            TimeIntervalProperty = DependencyProperty.Register(
                    "TimeInterval",
                    typeof(int),
                    typeof(ProgressBarBehavior));
        }

        protected override void OnAttached()
        {
            dataContext = AssociatedObject.DataContext as INotifyPropertyChanged;

            if (dataContext != null)
                dataContext.PropertyChanged += OnPropertyChanged;

            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;

            if (dataContext != null)
                dataContext.PropertyChanged -= OnPropertyChanged;
        }

        protected void OnLoaded(object sender, EventArgs e) => Handler();
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
                Handler();
        }

        protected void Handler()
        {
            var animation = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(TimeInterval));
            AssociatedObject.BeginAnimation(ProgressBar.ValueProperty, animation);
        }

    }
}
