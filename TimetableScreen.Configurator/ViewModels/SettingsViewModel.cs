using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class SettingsViewModel:BindableBase, IRegionMemberLifetime
    {
        public Settings Settings { get; set; }

        public bool KeepAlive { get => false; }

        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }
    }
}
