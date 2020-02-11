using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace TimetableScreen.Configurator.Models
{
    public class Department : BindableBase
    {
        private string name;
        private ObservableCollection<Timetable> timetables;

        public string Name { get => name; set => SetProperty(ref name, value); }
        public ObservableCollection<Timetable> Timetables { get => timetables; set => SetProperty(ref timetables, value); }

        public Department()
        {
            Timetables = new ObservableCollection<Timetable>();
        }
    }
}
