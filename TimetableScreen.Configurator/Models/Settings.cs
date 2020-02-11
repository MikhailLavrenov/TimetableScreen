﻿using Prism.Mvvm;
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
        private string screenAddress;
        private ushort screenPort;
        private double scale;
        private int showPageTime;
        private int specialistWidth;
        private int cabinetWidth;
        private int siteWidth;
        private int dayOfWeekWidth;
        private int noteWidth;

        public double Scale { get => scale; set => SetProperty(ref scale, value); }
        public int ShowPageTime { get => showPageTime; set => SetProperty(ref showPageTime, value); }
        public int SpecialistWidth { get => specialistWidth; set => SetProperty(ref specialistWidth, value); }   
        public int CabinetWidth { get => cabinetWidth; set => SetProperty(ref cabinetWidth, value); }
        public int SiteWidth { get => siteWidth; set => SetProperty(ref siteWidth, value); }
        public int DayOfWeekWidth { get => dayOfWeekWidth; set => SetProperty(ref dayOfWeekWidth, value); }
        public int NoteWidth { get => noteWidth; set => SetProperty(ref noteWidth, value); }

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
                return new Settings() { 
                    ScreenAddress="127.0.0.1", 
                    ScreenPort=8642, 
                    Scale=1,
                    ShowPageTime=60,
                    SiteWidth = 0,
                    CabinetWidth = 40,                    
                    SpecialistWidth =240,
                    DayOfWeekWidth=50,
                    NoteWidth=0
                };
        }
    }
}
