using NLog;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;
using TimetableScreen.Configurator.Models;
using TimetableScreen.Views;

namespace TimetableScreen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private ILogger logger;

        protected override Window CreateShell()
        {
            return Container.Resolve<ScreenView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(containerRegistry.GetContainer());
            containerRegistry.RegisterInstance(Settings.Load());
            containerRegistry.RegisterInstance<ILogger>(LogManager.GetCurrentClassLogger());

            containerRegistry.RegisterForNavigation<TimetableView>();
            containerRegistry.RegisterForNavigation<TitleView>();
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
