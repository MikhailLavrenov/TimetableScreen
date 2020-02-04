using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class DepartmentsViewModel:BindableBase
    {
        public Settings Settings { get; set; }

        public DepartmentsViewModel(Settings settings)
        {
            Settings = settings;
        }


    }
}
