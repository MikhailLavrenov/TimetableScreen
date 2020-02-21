using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class PagingBehavior : Behavior<FrameworkElement>
    {
        private ListView parentListView;
        private ListView listView;
        private Border footerBorder;
        private ICommand MoveOnNextPageCommand;
        private ScreenViewModel dataContext;

        protected override void OnAttached()
        {
            listView = AssociatedObject as ListView;
            parentListView = listView.FindVisualParent<ListView>();            
            dataContext = (ScreenViewModel)parentListView.DataContext;
            MoveOnNextPageCommand = dataContext.MoveToNextPageCommand;

            if (!MoveOnNextPageCommand.CanExecute(null))
                return;

            footerBorder = parentListView.FindVisualParent<Window>().FindVisualChild<Border>("FooterBorder");

            listView.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var maxY = parentListView.ActualHeight - footerBorder.ActualHeight * (dataContext.Settings.Scale - 1);

            var rows = new List<Border>();
            listView.FindVisualChilds("RowBorder", ref rows);

            foreach (var row in rows)
            {
                var rowY = row.TranslatePoint(new Point(0, row.ActualHeight), parentListView).Y * dataContext.Settings.Scale;

                if (rowY > maxY)
                    MoveOnNextPageCommand.Execute((Timetable)row.DataContext);
            }
        }

        protected override void OnDetaching()
        {
            listView.Loaded -= OnLoaded;
        }
    }
}
