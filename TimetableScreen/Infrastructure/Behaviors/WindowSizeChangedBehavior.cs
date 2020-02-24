using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using TimetableScreen.ViewModels;

namespace TimetableScreen.Infrastructure
{
    public class WindowSizeChangedBehavior : Behavior<FrameworkElement>
    {
        Window window;

        protected override void OnAttached()
        {
            window = (Window)AssociatedObject;

            window.SizeChanged += Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            var viewModel=(ScreenViewModel)window.DataContext;
            viewModel.InitializeCommand.Execute();
        }

        protected override void OnDetaching()
        {
            window.SizeChanged -= Handler;
        }
    }
}
