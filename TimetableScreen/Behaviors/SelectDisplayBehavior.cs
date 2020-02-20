using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Drawing;
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
            var displayIndex = viewModel.Settings.UseDisplay;

            Rectangle screenBounds;

            if (displayIndex < 1 || displayIndex > Screen.AllScreens.Length)
            {
                screenBounds = Screen.PrimaryScreen.Bounds;
                window.WindowState = WindowState.Minimized;
            }
            else
            {
                screenBounds = Screen.AllScreens[displayIndex - 1].Bounds;
                window.WindowState = WindowState.Normal;
            }

            window.Top = screenBounds.Y;
            window.Left = screenBounds.X;
            window.Width = screenBounds.Width;
            window.Height = screenBounds.Height;

            if (window.WindowState != WindowState.Minimized)
                window.Activate();
        }

        protected override void OnDetaching()
        {
            viewModel.PropertyChanged -= OnPropertyChanged;
            AssociatedObject.Initialized -= OnInitialized;
        }
    }
}
