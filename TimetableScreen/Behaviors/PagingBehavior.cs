using Microsoft.Xaml.Behaviors;
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
            footerBorder = parentListView.FindVisualParent<Window>().FindVisualChild<Border>("FooterBorder");
            dataContext = (ScreenViewModel)parentListView.DataContext;
            MoveOnNextPageCommand = dataContext.MoveToNextPageCommand;

            listView.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => Handler();

        private void Handler()
        {
            var row = listView.FindVisualChild<Border>("RowBorder");

            var y = listView.TranslatePoint(new Point(0, listView.ActualHeight), parentListView).Y * dataContext.Settings.Scale;

            if (y > parentListView.ActualHeight - footerBorder.ActualHeight * dataContext.Settings.Scale)
                MoveOnNextPageCommand.Execute((Timetable)row.DataContext);
        }

        protected override void OnDetaching()
        {
            listView.Loaded -= OnLoaded;
        }
    }
}
