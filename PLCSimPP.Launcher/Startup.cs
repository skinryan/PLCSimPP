using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Events;


namespace PLCSimPP.Launcher
{
    class Startup
    {
        //public static IEventAggregator eventAggregator;
        private static bool hookFlag = false;

        private static App prismApp;

        public static App PrismApp
        {
            get
            {
                if (prismApp == null)
                {
                    return prismApp = new App();
                }

                return prismApp;
            }
        }

        /// <summary>  
        /// program entrance。  
        /// </summary>          
        static void Main(string[] args)
        {
            System.Console.WriteLine("PLC Sim Init...");

            Thread thread = new Thread(new ThreadStart(StartApp));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "exit")
                {

                    PrismApp.Dispatcher.Invoke(new Action(() => PrismApp.Shutdown()));
                    System.Console.WriteLine("PLC Sim exit.");
                    break;
                }
                else
                {
                    PrismApp.Dispatcher.Invoke(new Action(() =>
                    {
                        var service = (IConsoleService)prismApp.Container.Resolve(typeof(IConsoleService));
                        service.ConsoleInput(input);
                    }));
                }
            }
        }

        private static void StartApp()
        {
            PrismApp.Activated += PrismApp_Activated;

            PrismApp.Run();
        }

        private static void PrismApp_Activated(object sender, EventArgs e)
        {
            if (!hookFlag)
            {
                System.Console.WriteLine("Init completed.");
                PrismApp.Dispatcher.Invoke(new Action(() =>
                {
                    IEventAggregator eventAgg = (IEventAggregator)prismApp.Container.Resolve(typeof(IEventAggregator));
                    //eventAgg.GetEvent<ConsoleOutputEvent>().Subscribe(Print);
                }));

                hookFlag = true;
            }
        }

        private static void Print(string message)
        {
            System.Console.WriteLine(message);

        }
    }
}
