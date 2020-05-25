using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

namespace BCI.PLCSimPP.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message+"\n"+e.Exception.StackTrace);

            e.Handled = true;
        }

        protected override Window CreateShell()
        {

            return Container.Resolve<BCI.PLCSimPP.Launcher.Views.MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<BCI.PLCSimPP.Service.ServicesModule>();
            moduleCatalog.AddModule<BCI.PLCSimPP.MainWindow.MainWindowModule>();
            moduleCatalog.AddModule<BCI.PLCSimPP.Layout.LayoutModule>();
            moduleCatalog.AddModule<BCI.PLCSimPP.Log.LogModule>();
            moduleCatalog.AddModule<BCI.PLCSimPP.Config.ConfigModule>();
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
