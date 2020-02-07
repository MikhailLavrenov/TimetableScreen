using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    class PhysiciansViewModel : BindableBase
    {
        private Department selectedDepartment;
        private ObservableCollection<PhysicianTimetable> physiciansTimetables;

        public Settings Settings { get; set; }
        public Department SelectedDepartment
        {
            get => selectedDepartment;
            set
            {
                SetProperty(ref selectedDepartment, value);
                PhysiciansTimetables = SelectedDepartment.PhysicianTimetables;
            }
        }
        public ObservableCollection<PhysicianTimetable> PhysiciansTimetables { get => physiciansTimetables; set => SetProperty(ref physiciansTimetables, value); }

        public DelegateCommand<PhysicianTimetable> MoveUpCommand { get; }
        public DelegateCommand<PhysicianTimetable> MoveDownCommand { get; }

        public PhysiciansViewModel(Settings settings)
        {
            Settings = settings;

            SelectedDepartment = Settings.Departments.FirstOrDefault();

            MoveUpCommand = new DelegateCommand<PhysicianTimetable>(MoveUpExecute);
            MoveDownCommand = new DelegateCommand<PhysicianTimetable>(MoveDownExecute);
        }

        public void MoveUpExecute(PhysicianTimetable item)
        {
            var itemIndex = PhysiciansTimetables.IndexOf(item);
            if (itemIndex > 0)
                PhysiciansTimetables.Move(itemIndex, itemIndex - 1);
        }
        public void MoveDownExecute(PhysicianTimetable item)
        {
            var itemIndex = PhysiciansTimetables.IndexOf(item);
            if (itemIndex >= 0 && itemIndex < PhysiciansTimetables.Count - 1)
                PhysiciansTimetables.Move(itemIndex, itemIndex + 1);
        }

    }
}
