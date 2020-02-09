using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimetableScreen
{
    public class PagingBehavior : Behavior<FrameworkElement>
    {
        private ListView listView;

        protected override void OnAttached()
        {
            listView = AssociatedObject as ListView;

            listView.Loaded += SourceUpdatedHandler;
        }

        private void SourceUpdatedHandler(object sender, RoutedEventArgs e)
        {
            var parentListView = FindVisualParent<ListView>(listView);
            var rows = new List<Border>();
            FindVisualChild(listView, "RowBorder", ref rows);

            foreach (var row in rows)
            {
                var visible = IsOnScreenVisible(row, parentListView);
               
                

            }
        }

        protected override void OnDetaching()
        {
            listView.Loaded -= SourceUpdatedHandler;
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
