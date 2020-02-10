using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class ScreenViewModel : BindableBase
    {
        private Settings settings;
        private NetworkTransport networkTransport;
        private List<ObservableCollection<Department>> pages;
        private ObservableCollection<Department> currentPage;
        private Queue<Department> onPageQueue;

        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }
        public List<ObservableCollection<Department>> Pages { get => pages; set => SetProperty(ref pages, value); }
        public ObservableCollection<Department> CurrentPage { get => currentPage; set => SetProperty(ref currentPage, value); }

        public DelegateCommand CloseCommand { get; }
        public DelegateCommand<PhysicianTimetable> MoveOnNextPageCommand { get; }


        public ScreenViewModel(Settings settings)
        {
            Settings = settings;

            networkTransport = new NetworkTransport();
            networkTransport.DataRecieved += DataRecievedHandler;
            networkTransport.StartServer(IPAddress.Any, Settings.TimetablePort);

            
            CurrentPage = new ObservableCollection<Department>();
            CurrentPage.AddRange(Settings.Departments);
            Pages = new List<ObservableCollection<Department>>();
            Pages.Add(CurrentPage);
            

            CloseCommand = new DelegateCommand(CloseExecute);
            MoveOnNextPageCommand = new DelegateCommand<PhysicianTimetable>(MoveNextPageExecute);
        }

        private void DataRecievedHandler(object oject, ResponseEventArgs args)
        {
            var stream = new MemoryStream();
            stream.Write(args.Buffer, 0, args.Buffer.Length);
            stream.Position = 0;

            var formatter = new XmlSerializer(Settings.GetType());
            Settings = (Settings)formatter.Deserialize(stream);

            Settings.Save();

            networkTransport.StopServer();
            networkTransport.StartServer(IPAddress.Any, Settings.TimetablePort);
        }
        private void CloseExecute()
        {
            networkTransport.StopServer();
            Application.Current.Shutdown();
        }
        private void MoveNextPageExecute(PhysicianTimetable physician)
        {





        }



    }
}
