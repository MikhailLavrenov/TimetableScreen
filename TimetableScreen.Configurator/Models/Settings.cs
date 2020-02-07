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
        private static readonly string fileName = "Settings.xml";
        private ObservableCollection<Department> departments;
        private string timetableAddress;
        private ushort timetablePort;
        private int fontSize;
        private int specialtyWidth;
        private int fullNameWidth;
        private int cabinetWidth;
        private int siteWidth;
        private int dayOfWeekWidth;
        private int noteWidth;

        public int FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }
        public int SpecialtyWidth { get => specialtyWidth; set => SetProperty(ref specialtyWidth, value); }
        public int FullNameWidth { get => fullNameWidth; set => SetProperty(ref fullNameWidth, value); }        
        public int CabinetWidth { get => cabinetWidth; set => SetProperty(ref cabinetWidth, value); }
        public int SiteWidth { get => siteWidth; set => SetProperty(ref siteWidth, value); }
        public int DayOfWeekWidth { get => dayOfWeekWidth; set => SetProperty(ref dayOfWeekWidth, value); }
        public int NoteWidth { get => noteWidth; set => SetProperty(ref noteWidth, value); }

        public ObservableCollection<Department> Departments { get => departments; set => SetProperty(ref departments, value); }
        public string TimetableAddress { get => timetableAddress; set => SetProperty(ref timetableAddress, value); }
        public ushort TimetablePort { get => timetablePort; set => SetProperty(ref timetablePort, value); }       

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
                return new Settings() { 
                    TimetableAddress="127.0.0.1", 
                    TimetablePort=8642, 
                    FontSize=14, 
                    SpecialtyWidth=200,
                    FullNameWidth=250,
                    CabinetWidth=70,
                    SiteWidth=70,
                    DayOfWeekWidth=120,
                    NoteWidth=150
                };
        }
    }
}
