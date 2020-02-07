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
                return new Settings() { TimetableAddress="127.0.0.1", TimetablePort=8642 };
        }
    }
}
