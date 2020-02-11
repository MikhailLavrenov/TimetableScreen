using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class ScreenViewModel : BindableBase
    {
        private Settings settings;
        private NetworkTransport networkTransport;
        private List<ObservableCollection<Department>> pages;
        private ObservableCollection<Department> currentPage;
        private int currentPageIndex;
        private DispatcherTimer timer;

        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }
        public List<ObservableCollection<Department>> Pages { get => pages; set => SetProperty(ref pages, value); }
        public ObservableCollection<Department> CurrentPage { get => currentPage; set => SetProperty(ref currentPage, value); }
        public int CurrentPageIndex { get => currentPageIndex; set => SetProperty(ref currentPageIndex, value); }

        public DelegateCommand CloseCommand { get; }
        public DelegateCommand<Timetable> MoveToNextPageCommand { get; }

        public ScreenViewModel(Settings settings)
        {
            Settings = settings;

            networkTransport = new NetworkTransport();
            networkTransport.DataRecieved += DataRecievedHandler;
            networkTransport.StartServer(IPAddress.Any, Settings.ScreenPort);

            timer = new DispatcherTimer();
            timer.Tick += TimerTick;

            CloseCommand = new DelegateCommand(CloseExecute);
            MoveToNextPageCommand = new DelegateCommand<Timetable>(MoveToNextPageExecute);

            Initialize();
        }

        private void DataRecievedHandler(object oject, ResponseEventArgs args)
        {
            timer.Stop();

            var stream = new MemoryStream();
            stream.Write(args.Buffer, 0, args.Buffer.Length);
            stream.Position = 0;

            var formatter = new XmlSerializer(Settings.GetType());
            Settings = (Settings)formatter.Deserialize(stream);

            Settings.Save();

            networkTransport.StopServer();
            networkTransport.StartServer(IPAddress.Any, Settings.ScreenPort);

            Initialize();
        }
        private void CloseExecute()
        {
            networkTransport.StopServer();
            Application.Current.Shutdown();
        }
        private void MoveToNextPageExecute(Timetable movingPhysician)
        {
            ObservableCollection<Department> nextPage;

            if (Pages.Count - 1 == CurrentPageIndex)
            {
                nextPage = new ObservableCollection<Department>();
                Pages.Add(nextPage);
            }
            else
                nextPage = Pages[CurrentPageIndex + 1];

            Department movingDepartment = null;

            foreach (var department in CurrentPage)
            {
                if (department.Timetables.Any(x => x == movingPhysician))
                {
                    movingDepartment = department;
                    break;
                }
            }

            if (movingDepartment.Timetables.Count <= 1)
                CurrentPage.Remove(movingDepartment);
            else
                movingDepartment.Timetables.Remove(movingPhysician);

            var nextPageDepartment = nextPage.FirstOrDefault(x => x.Name == movingDepartment.Name);

            if (nextPageDepartment == null)
            {
                nextPageDepartment = new Department { Name = movingDepartment.Name };
                nextPage.Add(nextPageDepartment);
            }

            nextPageDepartment.Timetables.Add(movingPhysician);
        }
        private void Initialize()
        {
            CurrentPage = new ObservableCollection<Department>();
            CurrentPage.AddRange(Settings.Departments);
            Pages = new List<ObservableCollection<Department>>();
            Pages.Add(CurrentPage);
            CurrentPageIndex = 0;

            timer.Interval = TimeSpan.FromSeconds(Settings.ShowPageTime);
            timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (Pages.Count == 1)
                return;

            currentPageIndex = currentPageIndex == Pages.Count - 1 ? 0 : currentPageIndex + 1;

            CurrentPage = Pages[currentPageIndex];
        }
    }
}
