using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class DepartmentsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive { get => false; }
        public Settings Settings { get; set; }

        public DelegateCommand<object> MoveUpCommand { get; }
        public DelegateCommand<object> MoveDownCommand { get; }

        public DepartmentsViewModel(Settings settings)
        {
            Settings = settings;

            MoveUpCommand = new DelegateCommand<object>(MoveUpExecute);
            MoveDownCommand = new DelegateCommand<object>(MoveDownExecute);
        }

        public void MoveUpExecute(object item)
        {
            if (item is Department department)
            {
                var itemIndex = Settings.Departments.IndexOf(department);
                if (itemIndex > 0)
                    Settings.Departments.Move(itemIndex, itemIndex - 1);
            }
        }
        public void MoveDownExecute(object item)
        {
            if (item is Department department)
            {
                var itemIndex = Settings.Departments.IndexOf(department);
                if (itemIndex >= 0 && itemIndex < Settings.Departments.Count - 1)
                    Settings.Departments.Move(itemIndex, itemIndex + 1);
            }
        }
    }
}
