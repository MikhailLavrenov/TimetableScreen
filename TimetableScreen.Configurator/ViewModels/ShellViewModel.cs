using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        IRegionManager regionManager;

        public Settings Settings { get; set; }

        public DelegateCommand<Type> NavigateCommand { get; }
        public DelegateCommand SaveSettingsCommand { get; }
        public DelegateCommand SendSettingsCommand { get; }

        public ShellViewModel(IRegionManager regionManager, Settings settings)
        {
            this.regionManager = regionManager;
            Settings = settings;

            NavigateCommand = new DelegateCommand<Type>(NavigateExecute);
            SaveSettingsCommand = new DelegateCommand(() => Settings.Save());
            SendSettingsCommand = new DelegateCommand(SendSettingsExecute);
        }

        private void NavigateExecute(Type viewType)
        {
            regionManager.RequestNavigate("MainRegion", viewType.Name);
        }
        private void SendSettingsExecute()
        {
            var stream = new MemoryStream();
            var formatter = new XmlSerializer(Settings.GetType());
            formatter.Serialize(stream, Settings);

            var bytes = stream.ToArray();

            //мб socketException
            var address = Dns.GetHostAddresses(Settings.ScreenAddress).First(x=>x.AddressFamily == AddressFamily.InterNetwork);

            NetworkTransport.Send(address, Settings.ScreenPort, bytes);
        }

    }
}
