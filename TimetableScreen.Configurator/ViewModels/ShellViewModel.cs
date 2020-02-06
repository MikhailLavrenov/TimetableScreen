using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        IRegionManager regionManager;

        public Settings Settings { get; set; }

        public DelegateCommand<Type> NavigateCommand { get; }
        public DelegateCommand SaveSettingsCommand { get; }
        public DelegateCommand ApplySettingsCommand { get; }

        public ShellViewModel(IRegionManager regionManager, Settings settings)
        {
            this.regionManager = regionManager;
            Settings = settings;

            NavigateCommand = new DelegateCommand<Type>(NavigateExecute);
            SaveSettingsCommand = new DelegateCommand(() => Settings.Save());
            ApplySettingsCommand = new DelegateCommand(ApplySettingsExecute);
        }

        private void NavigateExecute(Type viewType)
        {
            regionManager.RequestNavigate("MainRegion", viewType.Name);
        }
        private void ApplySettingsExecute()
        {
            var stream = new MemoryStream();
            var formatter = new XmlSerializer(Settings.GetType());
            formatter.Serialize(stream, Settings);
            var bytes = stream.ToArray();

            //мб socketException
            var address = Dns.GetHostAddresses(Settings.TimetableAddress)[0];

            NetworkTransport.Send(address, Settings.TimetablePort, bytes);
        }

    }
}
