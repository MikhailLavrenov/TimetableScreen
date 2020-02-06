using Prism.Ioc;
using Prism.Mvvm;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class ShellViewModel : BindableBase
    {
        private Settings settings;
        private NetworkTransport networkTransport;
        private IContainerRegistry containerRegistry;

        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }

        public ShellViewModel(Settings settings, IContainerRegistry containerRegistry)
        {
            Settings = settings;
            this.containerRegistry = containerRegistry;

            networkTransport = new NetworkTransport();
            networkTransport.DataRecieved += DataRecievedHandler;
            Task.Run(() => networkTransport.StartServer(IPAddress.Any, Settings.TimetablePort));
        }

        private void DataRecievedHandler(object oject, ResponseEventArgs args)
        {
            var stream = new MemoryStream();
            stream.Write(args.Buffer, 0, args.Buffer.Length);

            var formatter = new XmlSerializer(typeof(Settings));
            Settings = (Settings)formatter.Deserialize(stream);

            Settings.Save();
            containerRegistry.RegisterInstance(Settings);

            networkTransport.StopServer();
            Task.Run(() => networkTransport.StartServer(IPAddress.Any, Settings.TimetablePort));
        }


    }
}
