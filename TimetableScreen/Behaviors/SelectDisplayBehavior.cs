using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class SelectDisplayBehavior : Behavior<FrameworkElement>
    {
        Window window;
        ScreenViewModel viewModel;

        protected override void OnAttached()
        {
            window = AssociatedObject as Window;

            viewModel = (ScreenViewModel)window.DataContext;

            viewModel.PropertyChanged += OnPropertyChanged;
            AssociatedObject.Initialized += OnInitialized;
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings))
                System.Windows.Application.Current.Dispatcher.Invoke(() => DisplayManager());
        }
        private void OnInitialized(object sender, EventArgs e)
        {
            DisplayManager();
        }

        private void DisplayManager()
        {
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
            viewModel.PropertyChanged -= OnPropertyChanged;
            AssociatedObject.Initialized -= OnInitialized;
        }
    }
}
