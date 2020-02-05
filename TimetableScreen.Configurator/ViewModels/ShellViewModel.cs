using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        IRegionManager regionManager;

        public Settings Settings { get; set; }

        public DelegateCommand<Type> NavigateCommand { get; }
        public DelegateCommand SaveSettingsCommand { get; }

        public ShellViewModel(IRegionManager regionManager, Settings settings)
        {
            this.regionManager = regionManager;
            Settings = settings;

            NavigateCommand = new DelegateCommand<Type>(NavigateExecute);
            SaveSettingsCommand = new DelegateCommand(() => Settings.Save());;
        }

        private void NavigateExecute(Type viewType)
        {
            regionManager.RequestNavigate("MainRegion", viewType.Name);
        }

    }
}
