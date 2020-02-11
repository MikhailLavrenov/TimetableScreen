using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    class TimetablesViewModel : BindableBase
    {
        private Department selectedDepartment;
        private ObservableCollection<Timetable> timetables;

        public Settings Settings { get; set; }
        public Department SelectedDepartment
        {
            get => selectedDepartment;
            set
            {
                SetProperty(ref selectedDepartment, value);
                Timetables = SelectedDepartment.Timetables;
            }
        }
        public ObservableCollection<Timetable> Timetables { get => timetables; set => SetProperty(ref timetables, value); }

        public DelegateCommand<Timetable> MoveUpCommand { get; }
        public DelegateCommand<Timetable> MoveDownCommand { get; }

        public TimetablesViewModel(Settings settings)
        {
            Settings = settings;

            SelectedDepartment = Settings.Departments.FirstOrDefault();

            MoveUpCommand = new DelegateCommand<Timetable>(MoveUpExecute);
            MoveDownCommand = new DelegateCommand<Timetable>(MoveDownExecute);
        }

        public void MoveUpExecute(Timetable item)
        {
            var itemIndex = Timetables.IndexOf(item);
            if (itemIndex > 0)
                Timetables.Move(itemIndex, itemIndex - 1);
        }
        public void MoveDownExecute(Timetable item)
        {
            var itemIndex = Timetables.IndexOf(item);
            if (itemIndex >= 0 && itemIndex < Timetables.Count - 1)
                Timetables.Move(itemIndex, itemIndex + 1);
        }

    }
}
