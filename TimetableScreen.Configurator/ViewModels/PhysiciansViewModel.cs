using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    class PhysiciansViewModel : BindableBase
    {
            public Settings Settings { get; set; }

            public PhysiciansViewModel(Settings settings)
            {
                Settings = settings;
            }

        }
}
