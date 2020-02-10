using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class PagingBehavior : Behavior<FrameworkElement>
    {
        private ListView parentListView;
        private ListView listView;
        private ICommand MoveOnNextPageCommand;
        private double scaleY;

        protected override void OnAttached()
        {
            listView = AssociatedObject as ListView;
            parentListView = FindVisualParent<ListView>(listView);
            scaleY = (parentListView.RenderTransform as ScaleTransform).ScaleY;
            MoveOnNextPageCommand = ((ScreenViewModel)FindVisualParent<Window>(parentListView).DataContext).MoveToNextPageCommand;

            listView.Items.CurrentChanged += ItemChangedHandler;
            listView.Loaded += LoadedHandler;

        }

        private void LoadedHandler(object sender, RoutedEventArgs e) => Handler();

        private void ItemChangedHandler(object sender, EventArgs e) => Handler();

        private void Handler()
        {
            var rows = new List<Border>();
            var row = FindVisualChild<Border>(listView, "RowBorder");

            if (!IsOnScreenVisible(row, parentListView))
                MoveOnNextPageCommand.Execute(row.DataContext as PhysicianTimetable);
        }

        protected override void OnDetaching()
        {
            listView.Items.CurrentChanged -= ItemChangedHandler;
            listView.Loaded -= LoadedHandler;
        }



        private bool IsOnScreenVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Point elementTopLeft = new Point(0, 0);
            var y = element.TranslatePoint(new Point(0, element.ActualHeight), container).Y * scaleY;

            if (y > container.ActualHeight)
                return false;

            return true;
        }

        public static T FindVisualParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            while (element != null)
            {
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;

                if (element is T correctlyTyped)
                    return correctlyTyped;
            }

            return null;
        }

        public T FindVisualChild<T>(FrameworkElement element, string name) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child is T correctlyTyped && child.Name == name)
                    return correctlyTyped;

                var result = FindVisualChild<T>(child, name);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
