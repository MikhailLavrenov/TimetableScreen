using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace TimetableScreen.Configurator.Models
{
    [Serializable]
    public class Settings : BindableBase
    {
        private static readonly string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.xml";
        private ObservableCollection<Department> departments;
        private string screenAddress = "127.0.0.1";
        private ushort screenPort = 8642;
        private double scale = 1;
        private int showPageTime = 20;
        private int specialistWidth = 240;
        private int cabinetWidth = 50;
        private int siteWidth = 0;
        private int dayOfWeekWidth = 55;
        private int noteWidth = 0;
        private int useDisplay = 1;
        private bool topMost = true;
        private bool autoLoad = true;
        private int showTitlePageTime=7;
        private string titleText = "Расписания приема специалистов";


        public int UseDisplay { get => useDisplay; set => SetProperty(ref useDisplay, value); }
        public bool TopMost { get => topMost; set => SetProperty(ref topMost, value); }
        public bool AutoLoad { get => autoLoad; set => SetProperty(ref autoLoad, value); }
        public double Scale { get => scale; set => SetProperty(ref scale, value); }
        public int ShowPageTime { get => showPageTime; set => SetProperty(ref showPageTime, value); }
        public int SpecialistWidth { get => specialistWidth; set => SetProperty(ref specialistWidth, value); }
        public int CabinetWidth { get => cabinetWidth; set => SetProperty(ref cabinetWidth, value); }
        public int SiteWidth { get => siteWidth; set => SetProperty(ref siteWidth, value); }
        public int DayOfWeekWidth { get => dayOfWeekWidth; set => SetProperty(ref dayOfWeekWidth, value); }
        public int NoteWidth { get => noteWidth; set => SetProperty(ref noteWidth, value); }
        public int ShowTitlePageTime { get => showTitlePageTime; set => SetProperty(ref showTitlePageTime, value); }
        public string TitleText { get => titleText; set => SetProperty(ref titleText, value); }


        public ObservableCollection<Department> Departments { get => departments; set => SetProperty(ref departments, value); }
        public string ScreenAddress { get => screenAddress; set => SetProperty(ref screenAddress, value); }
        public ushort ScreenPort { get => screenPort; set => SetProperty(ref screenPort, value); }

        public Settings()
        {
            Departments = new ObservableCollection<Department>();
        }

        //сохраняет настройки в xml
        public void Save()
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var formatter = new XmlSerializer(GetType());
                formatter.Serialize(stream, this);
            }
        }
        //загружает настройки из xml
        public static Settings Load()
        {
            if (File.Exists(fileName))
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var formatter = new XmlSerializer(typeof(Settings));
                    return formatter.Deserialize(stream) as Settings;
                }
            }
            else
                return new Settings();
        }
    }
}
