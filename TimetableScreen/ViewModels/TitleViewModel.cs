using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Threading;
using TimetableScreen.Configurator.Models;
using TimetableScreen.Views;

namespace TimetableScreen.ViewModels
{
    public class TitleViewModel : BindableBase,INavigationAware
    {
        Settings settings;
        DispatcherTimer timer;
        IRegionManager regionManager;

        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }
        public double FontSize { get; set; }

        public TitleViewModel(Settings settings, IRegionManager regionManager)
        {
            Settings = settings;
            this.regionManager = regionManager;
            FontSize = Settings.Scale * 48;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(Settings.ShowTitlePageTime);
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            regionManager.RequestNavigate("MainRegion", nameof(TimetableView));
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!timer.IsEnabled)
                timer.Start();
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            timer.Stop();
        }
    }
}
