using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Forms;

namespace TimetableScreen
{
    public class SelectDisplayBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Initialized += Handler;
        }

        private void Handler(object sender, EventArgs e)
        {
            var window = AssociatedObject as Window;

            var viewModel = (ScreenViewModel)window.DataContext;

            var displayIndex = viewModel.Settings.UseDisplay - 1;


            if (displayIndex > Screen.AllScreens.Length - 1)
            {
                window.WindowState = WindowState.Minimized;
                return;
            }

            var screenBounds = Screen.AllScreens[displayIndex].Bounds;

            window.Top = screenBounds.Top;
            window.Left = screenBounds.Left;
            window.WindowState = WindowState.Maximized;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Initialized += Handler;
        }
    }
}
