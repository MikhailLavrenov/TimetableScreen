using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        IRegionManager regionManager;
        IContainer container;
        Type lastNavigation;

        public Settings Settings { get; set; }

        public DelegateCommand<Type> NavigateCommand { get; }
        public DelegateCommand SaveSettingsCommand { get; }
        public DelegateCommand SendSettingsCommand { get; }
        public DelegateCommand RecieveSettingsCommand { get; }

        public ShellViewModel(IRegionManager regionManager, Settings settings, IContainer container)
        {
            this.container = container;
            this.regionManager = regionManager;
            Settings = settings;

            NavigateCommand = new DelegateCommand<Type>(NavigateExecute);
            SaveSettingsCommand = new DelegateCommand(() => Settings.Save());
            SendSettingsCommand = new DelegateCommand(SendSettingsExecute);
            RecieveSettingsCommand = new DelegateCommand(RecieveSettingsExecute);
        }

        private void NavigateExecute(Type viewType)
        {
            lastNavigation = viewType;
            regionManager.RequestNavigate("MainRegion", viewType.Name);
        }

        private void SendSettingsExecute()
        {
            Client.Send(Settings.ScreenAddress, Settings.ScreenPort, Settings);
        }

        private void RecieveSettingsExecute()
        {
            Settings= Client.Recieve<Settings>(Settings.ScreenAddress, Settings.ScreenPort);
            container.UseInstance(typeof(Settings), Settings);

            if (lastNavigation != default)
            {
                regionManager.Regions["MainRegion"].RemoveAll();
                NavigateExecute(lastNavigation);
            }
        }

    }
}
