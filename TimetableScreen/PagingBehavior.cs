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

        protected override void OnAttached()
        {
            listView = AssociatedObject as ListView;
            parentListView = FindVisualParent<ListView>(listView);
            MoveOnNextPageCommand = ((ScreenViewModel)FindVisualParent<Window>(parentListView).DataContext).MoveOnNextPageCommand;

            listView.Items.CurrentChanged += SourceUpdatedHandler;
        }

        private void SourceUpdatedHandler(object sender, EventArgs e)
        {
            var rows = new List<Border>();
            FindVisualChild(listView, "RowBorder", ref rows);

            foreach (var row in rows)
                if (!IsOnScreenVisible(row, parentListView))
                    MoveOnNextPageCommand.Execute(row.DataContext as PhysicianTimetable);
        }

        protected override void OnDetaching()
        {
            listView.Items.CurrentChanged -= SourceUpdatedHandler;
        }



        //private bool IsOnScreenVisible(FrameworkElement element, FrameworkElement container)
        //{
        //    if (!element.IsVisible)
        //        return false;

        //    Point elementTopLeft;
        //    element.TranslatePoint(elementTopLeft, container);
        //    var elementBottomRight = new Point(elementTopLeft.X + element.Width, elementTopLeft.Y + element.ActualHeight);

        //    if (elementTopLeft.X >= 0 && elementTopLeft.Y >= 0 && elementBottomRight.X <= container.ActualWidth && elementBottomRight.Y <= container.ActualHeight)
        //        return true;

        //    return false;
        //}

        private static bool IsOnScreenVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            var elementRect = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(elementRect);
        }


        public static T FindVisualParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            while (element != null)
            {
                if (element is T correctlyTyped)
                    return correctlyTyped;

                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }

            return null;
        }

        public void FindVisualChild<T>(FrameworkElement element, string name, ref List<T> result) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child is T correctlyTyped && child.Name == name)
                    result.Add(correctlyTyped);
                else
                    FindVisualChild<T>(child, name, ref result);
            }

        }
    }
}
