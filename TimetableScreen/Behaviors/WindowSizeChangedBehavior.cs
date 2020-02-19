using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace TimetableScreen.Behaviors
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
            viewModel.InitializePagesCommand.Execute();
        }

        protected override void OnDetaching()
        {
            window.SizeChanged -= Handler;
        }
    }
}
