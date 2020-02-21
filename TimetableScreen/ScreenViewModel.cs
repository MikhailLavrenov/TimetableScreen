using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using TimetableScreen.Configurator.Infrastructure;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class ScreenViewModel : BindableBase
    {
        private Settings settings;
        private Server server;
        private List<ObservableCollection<Department>> pages;
        private ObservableCollection<Department> currentPage;
        private int currentPageIndex;
        private DispatcherTimer timer;
        private bool canMoveToNextPage;


        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }
        public List<ObservableCollection<Department>> Pages { get => pages; set => SetProperty(ref pages, value); }
        public ObservableCollection<Department> CurrentPage { get => currentPage; set => SetProperty(ref currentPage, value); }
        public int CurrentPageIndex { get => currentPageIndex; set => SetProperty(ref currentPageIndex, value); }
        public string PagesTotal { get => $"Страница {CurrentPageIndex + 1} из {Pages?.Count ?? 1}"; }
       
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand MinimizeCommand { get; }
        public DelegateCommand InitializePagesCommand { get; }
        public DelegateCommand<Timetable> MoveToNextPageCommand { get; }

        public ScreenViewModel(Settings settings)
        {
            Settings = settings;

            server = new Server();
            server.NetworkTransmissionEvent += OnNetworkTransmission;

            timer = new DispatcherTimer();
            timer.Tick += OnTimerTick;

            CloseCommand = new DelegateCommand(CloseExecute);
            MinimizeCommand = new DelegateCommand(() => System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized);
            InitializePagesCommand = new DelegateCommand(InitializePagesExecute);
            MoveToNextPageCommand = new DelegateCommand<Timetable>(MoveToNextPageExecute, (x) => canMoveToNextPage);

            SleepMode.PreventOn();
            ApplicationStartUpManager.Set(Settings.AutoLoad);

            InitializePagesExecute();
        }

        private void OnNetworkTransmission(object obj, NetworkTransmissionEventArgs args)
        {
            if (args.Operation == Operation.RecieveFromServer && args.ObjectType == typeof(Settings))
                server.SendRequestedObject(args.RequestId, Settings);
            else if (args.Operation == Operation.SendToServer && args.ObjectType == typeof(Settings))
            {
                timer.Stop();

                Settings = (Settings)args.Object;
                Settings.Save();

                ApplicationStartUpManager.Set(Settings.AutoLoad);

                InitializePagesExecute();
            }
        }
        private void CloseExecute()
        {
            server.Stop();
            SleepMode.PreventOff();
            System.Windows.Application.Current.Shutdown();
        }
        private void MoveToNextPageExecute(Timetable movingPhysician)
        {
            ObservableCollection<Department> nextPage;

            if (Pages.Count - 1 == CurrentPageIndex)
            {
                nextPage = new ObservableCollection<Department>();
                Pages.Add(nextPage);
                RaisePropertyChanged(nameof(PagesTotal));
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
        private void InitializePagesExecute()
        {
            timer.Stop();

            CurrentPage = new ObservableCollection<Department>();
            CurrentPage.AddRange(Settings.Departments);
            Pages = new List<ObservableCollection<Department>>();
            Pages.Add(CurrentPage);
            CurrentPageIndex = 0;
            canMoveToNextPage = true;

            RaisePropertyChanged(nameof(PagesTotal));

            server.Stop();
            server.Start(IPAddress.Any, Settings.ScreenPort);

            timer.Interval = TimeSpan.FromSeconds(Settings.ShowPageTime);
            timer.Start();
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (Pages.Count == 1)
            {
                timer.Stop();
                canMoveToNextPage = false;
                return;
            }

            if (CurrentPageIndex == Pages.Count - 1)
            {
                CurrentPageIndex = 0;
                canMoveToNextPage = false;
            }
            else
                CurrentPageIndex++;


            CurrentPage = Pages[CurrentPageIndex];

            RaisePropertyChanged(nameof(PagesTotal));
        }
    }
}
