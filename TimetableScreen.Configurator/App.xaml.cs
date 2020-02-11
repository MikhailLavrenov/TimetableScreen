using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;
using TimetableScreen.Configurator.Models;
using TimetableScreen.Configurator.Views;

namespace TimetableScreen.Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(Settings.Load());

            containerRegistry.RegisterForNavigation<DepartmentsView>();
            containerRegistry.RegisterForNavigation<TimetablesView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
        }
    }
}
