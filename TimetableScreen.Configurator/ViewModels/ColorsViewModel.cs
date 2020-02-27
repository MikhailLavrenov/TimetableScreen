using Prism.Commands;
using Prism.Mvvm;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen.Configurator.ViewModels
{
    public class ColorsViewModel : BindableBase
    {
        public Settings Settings { get; set; }
        public bool KeepAlive { get => false; }

        public DelegateCommand<object> MoveUpCommand { get; }
        public DelegateCommand<object> MoveDownCommand { get; }
        public DelegateCommand<object> ExchangeCommand { get; }

        public ColorsViewModel(Settings settings)
        {
            Settings = settings;

            MoveUpCommand = new DelegateCommand<object>(MoveUpExecute);
            MoveDownCommand = new DelegateCommand<object>(MoveDownExecute);
            ExchangeCommand = new DelegateCommand<object>(ExchangeExecute);
        }

        public void MoveUpExecute(object item)
        {
            if (item is ColorPair colorPair)
            {
                var itemIndex = Settings.BackgroundColors.IndexOf(colorPair);
                if (itemIndex > 0)
                    Settings.BackgroundColors.Move(itemIndex, itemIndex - 1);
            }
        }
        public void MoveDownExecute(object item)
        {
            if (item is ColorPair colorPair)
            {
                var itemIndex = Settings.BackgroundColors.IndexOf(colorPair);
                if (itemIndex >= 0 && itemIndex < Settings.BackgroundColors.Count - 1)
                    Settings.BackgroundColors.Move(itemIndex, itemIndex + 1);
            }
        }
        public void ExchangeExecute(object item)
        {
            if (item is ColorPair colorPair)
                (colorPair.Color1, colorPair.Color2) = (colorPair.Color2, colorPair.Color1);
        }

    }
}
