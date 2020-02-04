using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        IRegionManager regionManager;

        public DelegateCommand<Type> NavigateCommand { get; }

        public ShellViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            NavigateCommand = new DelegateCommand<Type>(NavigateExecute);
        }

        private void NavigateExecute(Type viewType)
        {
            regionManager.RequestNavigate("MainRegion", viewType.Name);
        }

    }
}
