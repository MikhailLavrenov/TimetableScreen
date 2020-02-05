using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class SettingsViewModel:BindableBase
    {
        public Settings Settings { get; set; }

        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }
    }
}
