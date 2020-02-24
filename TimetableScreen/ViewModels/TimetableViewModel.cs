using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using TimetableScreen.Configurator.Models;
using TimetableScreen.Views;

namespace TimetableScreen.ViewModels
{
    public class TimetableViewModel : BindableBase, INavigationAware
    {
        Settings settings;
        List<ObservableCollection<Department>> pages;
        ObservableCollection<Department> currentPage;
        int currentPageIndex;
        DispatcherTimer timer;
        bool canMoveToNextPage;
        IRegionManager regionManager;

        public Settings Settings { get => settings; set => SetProperty(ref settings, value); }
        public List<ObservableCollection<Department>> Pages { get => pages; set => SetProperty(ref pages, value); }
        public ObservableCollection<Department> CurrentPage { get => currentPage; set => SetProperty(ref currentPage, value); }
        public int CurrentPageIndex { get => currentPageIndex; set => SetProperty(ref currentPageIndex, value); }
        public string PageIndicator { get => $"Страница {CurrentPageIndex + 1} из {Pages?.Count ?? 1}"; }

        public DelegateCommand<Timetable> MoveToNextPageCommand { get; }

        public TimetableViewModel(Settings settings, IRegionManager regionManager)
        {
            Settings = settings;
            this.regionManager = regionManager;

            timer = new DispatcherTimer();
            timer.Tick += OnTimerTick;
           
            MoveToNextPageCommand = new DelegateCommand<Timetable>(MoveToNextPageExecute, (x) => canMoveToNextPage);

            CurrentPage = new ObservableCollection<Department>();
            CurrentPage.AddRange(Settings.Departments);
            Pages = new List<ObservableCollection<Department>>();
            Pages.Add(CurrentPage);
            RaisePropertyChanged(nameof(PageIndicator));
            CurrentPageIndex = 0;
            canMoveToNextPage = true;
            
            timer.Interval = TimeSpan.FromSeconds(Settings.ShowPageTime);
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (Pages.Count != 1)
            {
                if (CurrentPageIndex == Pages.Count - 1)
                    CurrentPageIndex = 0;
                else
                    CurrentPageIndex++;

                CurrentPage = Pages[CurrentPageIndex];

                RaisePropertyChanged(nameof(PageIndicator));
            }

            if (CurrentPageIndex == 0)
                regionManager.RequestNavigate("MainRegion", nameof(TitleView));
        }
        private void MoveToNextPageExecute(Timetable movingPhysician)
        {
            ObservableCollection<Department> nextPage;

            if (Pages.Count - 1 == CurrentPageIndex)
            {
                nextPage = new ObservableCollection<Department>();
                Pages.Add(nextPage);
                RaisePropertyChanged(nameof(PageIndicator));
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
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!timer.IsEnabled)
                timer.Start();
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            canMoveToNextPage = false;
            timer.Stop();
        }
    }
}
