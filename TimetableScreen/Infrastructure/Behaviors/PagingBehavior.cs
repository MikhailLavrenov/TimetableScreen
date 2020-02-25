using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;
using TimetableScreen.ViewModels;
using TimetableScreen.Views;

namespace TimetableScreen.Infrastructure
{
    public class PagingBehavior : Behavior<FrameworkElement>
    {
        private ListView parentListView;
        private ListView listView;
        private FrameworkElement footer;
        private ICommand MoveOnNextPageCommand;
        private TimetableViewModel dataContext;

        protected override void OnAttached()
        {
            listView = AssociatedObject as ListView;
            parentListView = listView.FindVisualParent<ListView>();            
            dataContext = (TimetableViewModel)parentListView.DataContext;
            MoveOnNextPageCommand = dataContext.MoveToNextPageCommand;

            if (!MoveOnNextPageCommand.CanExecute(null))
                return;

            footer = parentListView.FindVisualParent<TimetableView>().FindVisualChild<FrameworkElement>("Footer");

            listView.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var maxY = parentListView.ActualHeight - footer.ActualHeight * (dataContext.Settings.Scale - 1);

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
