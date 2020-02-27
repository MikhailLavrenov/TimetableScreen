using Prism.Mvvm;
using System.Windows.Media;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ColorsViewModel : BindableBase
    {
        public Settings Settings { get; set; }

        public bool KeepAlive { get => false; }

        public ColorsViewModel(Settings settings)
        {
            Settings = settings;

            //Settings.BackgroundColors.Add(("#000000","#000000"));
        }
    }
}
