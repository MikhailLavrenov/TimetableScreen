using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class DepartmentsViewModel:BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive { get => false; }
        public Settings Settings { get; set; }

        public DelegateCommand<Department> MoveUpCommand { get; }
        public DelegateCommand<Department> MoveDownCommand { get; }

        public DepartmentsViewModel(Settings settings)
        {
            Settings = settings;

            MoveUpCommand = new DelegateCommand<Department>(MoveUpExecute);
            MoveDownCommand = new DelegateCommand<Department>(MoveDownExecute);
        }

        public void MoveUpExecute(Department item)
        {
            var itemIndex = Settings.Departments.IndexOf(item);
            if (itemIndex > 0)
                Settings.Departments.Move(itemIndex, itemIndex - 1);
        }
        public void MoveDownExecute(Department item)
        {
            var itemIndex = Settings.Departments.IndexOf(item);
            if (itemIndex >= 0 && itemIndex < Settings.Departments.Count - 1)
                Settings.Departments.Move(itemIndex, itemIndex + 1);
        }
    }
}
