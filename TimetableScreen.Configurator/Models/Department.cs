using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace TimetableScreen.Configurator.Models
{
    public class Department : BindableBase
    {
        private string name;
        private ObservableCollection<PhysicianTimetable> physicianTimetables;

        public string Name { get => name; set => SetProperty(ref name, value); }
        public ObservableCollection<PhysicianTimetable> PhysicianTimetables { get => physicianTimetables; set => SetProperty(ref physicianTimetables, value); }

        public Department()
        {
            PhysicianTimetables = new ObservableCollection<PhysicianTimetable>();
        }
    }
}
