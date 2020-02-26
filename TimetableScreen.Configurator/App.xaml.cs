using NLog;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;
using TimetableScreen.Configurator.Models;
using TimetableScreen.Configurator.Views;

namespace TimetableScreen.Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private ILogger logger;

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(Settings.Load());
            containerRegistry.RegisterInstance<ILogger>(LogManager.GetCurrentClassLogger());

            containerRegistry.RegisterForNavigation<DepartmentsView>();
            containerRegistry.RegisterForNavigation<TimetablesView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
            containerRegistry.RegisterForNavigation<AboutView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            logger = Container.Resolve<ILogger>();

            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
            DispatcherUnhandledException += LogDispatcherUnhandledException;
        }

        private void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            logger.Error((Exception)args.ExceptionObject, "AppDomainException");
        }
        private void LogDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            logger.Error(args.Exception, "XamlDispatcherException");
        }
    }
}
