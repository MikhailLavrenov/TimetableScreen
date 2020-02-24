using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Net;
using System.Windows;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;
using TimetableScreen.Infrastructure;
using TimetableScreen.Views;

namespace TimetableScreen.ViewModels
{
    public class ScreenViewModel : BindableBase
    {
        Settings settings;
        Server server;
        IRegionManager regionManager;
        IContainer container;

        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }

        public DelegateCommand CloseCommand { get; }
        public DelegateCommand MinimizeCommand { get; }
        public DelegateCommand InitializeCommand { get; }

        public ScreenViewModel(Settings settings, IRegionManager regionManager, IContainer container)
        {
            this.container = container;
            this.regionManager = regionManager;
            Settings = settings;

            server = new Server();
            server.NetworkTransmissionEvent += OnNetworkTransmission;

            CloseCommand = new DelegateCommand(CloseExecute);
            MinimizeCommand = new DelegateCommand(() => System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized);
            InitializeCommand = new DelegateCommand(InitializeExecute);

            SleepMode.PreventOn();
        }

        private void OnNetworkTransmission(object obj, NetworkTransmissionEventArgs args)
        {
            if (args.Operation == Operation.RecieveFromServer && args.ObjectType == typeof(Settings))
                server.SendRequestedObject(args.RequestId, Settings);
            else if (args.Operation == Operation.SendToServer && args.ObjectType == typeof(Settings))
            {
                Settings = (Settings)args.Object;
                Settings.Save();

                container.UseInstance(typeof(Settings), Settings);

                InitializeExecute();
            }
        }
        private void CloseExecute()
        {
            server.Stop();
            SleepMode.PreventOff();
            System.Windows.Application.Current.Shutdown();
        }
        private void InitializeExecute()
        {
            ApplicationStartUpManager.Set(Settings.AutoLoad);

            server.Stop();
            server.Start(IPAddress.Any, Settings.ScreenPort);

            Application.Current.Dispatcher.Invoke(() =>
            {
                //RemoveAll не вызывает событие OnNavigatedFrom
                regionManager.RequestNavigate("MainRegion", string.Empty);
                //Удаляет ранее созданные ViewModel в регионе
                regionManager.Regions["MainRegion"].RemoveAll();
                regionManager.RequestNavigate("MainRegion", nameof(TitleView));
            });

        }
    }
}
