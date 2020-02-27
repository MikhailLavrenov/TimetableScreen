using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    class TimetablesViewModel : BindableBase, IRegionMemberLifetime
    {
        private Department selectedDepartment;
        private ObservableCollection<Timetable> timetables;

        public bool KeepAlive { get => false; }
        public Settings Settings { get; set; }
        public Department SelectedDepartment
        {
            get => selectedDepartment;
            set
            {
                SetProperty(ref selectedDepartment, value);
                Timetables = SelectedDepartment?.Timetables;
            }
        }
        public ObservableCollection<Timetable> Timetables { get => timetables; set => SetProperty(ref timetables, value); }

        public DelegateCommand<object> MoveUpCommand { get; }
        public DelegateCommand<object> MoveDownCommand { get; }

        public TimetablesViewModel(Settings settings)
        {
            Settings = settings;

            SelectedDepartment = Settings.Departments.FirstOrDefault();

            MoveUpCommand = new DelegateCommand<object>(MoveUpExecute);
            MoveDownCommand = new DelegateCommand<object>(MoveDownExecute);
        }

        public void MoveUpExecute(object item)
        {
            if (item is Timetable timetable)
            {
                var itemIndex = Timetables.IndexOf(timetable);
                if (itemIndex > 0)
                    Timetables.Move(itemIndex, itemIndex - 1);
            }
        }
        public void MoveDownExecute(object item)
        {
            if (item is Timetable timetable)
            {
                var itemIndex = Timetables.IndexOf(timetable);
                if (itemIndex >= 0 && itemIndex < Timetables.Count - 1)
                    Timetables.Move(itemIndex, itemIndex + 1);
            }
        }

    }
}
