using Prism.Mvvm;
using System;

namespace TimetableScreen.Configurator.Models
{
    [Serializable]
    public class Timetable : BindableBase
    {
        private string specialty;
        private string fullName;        
        private string cabinet;
        private string site;
        private string mondayBegin;
        private string mondayEnd;
        private string tuesdayBegin;
        private string tuesdayEnd;
        private string wednesdayBegin;
        private string wednesdayEnd;
        private string thursdayBegin;
        private string thursdayEnd;
        private string fridayBegin;
        private string fridayEnd;
        private string saturdayBegin;
        private string saturdayEnd;
        private string sundayBegin;
        private string sundayEnd;
        private string note;

        public string Specialty { get => specialty; set => SetProperty(ref specialty, value); }
        public string FullName { get => fullName; set => SetProperty(ref fullName, value); }        
        public string Cabinet { get => cabinet; set => SetProperty(ref cabinet, value); }
        public string Site { get => site; set => SetProperty(ref site, value); }
        public string MondayBegin { get => mondayBegin; set => SetProperty(ref mondayBegin, value); }
        public string MondayEnd { get => mondayEnd; set => SetProperty(ref mondayEnd, value); }
        public string TuesdayBegin { get => tuesdayBegin; set => SetProperty(ref tuesdayBegin, value); }
        public string TuesdayEnd { get => tuesdayEnd; set => SetProperty(ref tuesdayEnd, value); }
        public string WednesdayBegin { get => wednesdayBegin; set => SetProperty(ref wednesdayBegin, value); }
        public string WednesdayEnd { get => wednesdayEnd; set => SetProperty(ref wednesdayEnd, value); }
        public string ThursdayBegin { get => thursdayBegin; set => SetProperty(ref thursdayBegin, value); }
        public string ThursdayEnd { get => thursdayEnd; set => SetProperty(ref thursdayEnd, value); }
        public string FridayBegin { get => fridayBegin; set => SetProperty(ref fridayBegin, value); }
        public string FridayEnd { get => fridayEnd; set => SetProperty(ref fridayEnd, value); }
        public string SaturdayBegin { get => saturdayBegin; set => SetProperty(ref saturdayBegin, value); }
        public string SaturdayEnd { get => saturdayEnd; set => SetProperty(ref saturdayEnd, value); }
        public string SundayBegin { get => sundayBegin; set => SetProperty(ref sundayBegin, value); }
        public string SundayEnd { get => sundayEnd; set => SetProperty(ref sundayEnd, value); }
        public string Note { get => note; set => SetProperty(ref note, value); }

    }
}
