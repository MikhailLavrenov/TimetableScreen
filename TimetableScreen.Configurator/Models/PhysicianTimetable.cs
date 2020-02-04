using Prism.Mvvm;

namespace TimetableScreen.Configurator.Models
{
    public class PhysicianTimetable : BindableBase
    {
        private string fullName;
        private string specialty;
        private string cabinet;
        private string site;
        private string monday;
        private string tuesday;
        private string wednesday;
        private string thursday;
        private string friday;
        private string saturday;
        private string sunday;
        private string note;

        public string FullName { get => fullName; set => SetProperty(ref fullName, value); }
        public string Specialty { get => specialty; set => SetProperty(ref specialty, value); }
        public string Cabinet { get => cabinet; set => SetProperty(ref cabinet, value); }
        public string Site { get => site; set => SetProperty(ref site, value); }
        public string Monday { get => monday; set => SetProperty(ref monday, value); }
        public string Tuesday { get => tuesday; set => SetProperty(ref tuesday, value); }
        public string Wednesday { get => wednesday; set => SetProperty(ref wednesday, value); }
        public string Thursday { get => thursday; set => SetProperty(ref thursday, value); }
        public string Friday { get => friday; set => SetProperty(ref friday, value); }
        public string Saturday { get => saturday; set => SetProperty(ref saturday, value); }
        public string Sunday { get => sunday; set => SetProperty(ref sunday, value); }
        public string Note { get => note; set => SetProperty(ref note, value); }

    }
}
