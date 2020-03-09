using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Events;

namespace PLCSimPP.Service.Console
{
    public class ConsoleService : IConsoleService
    {
        IEventAggregator mEventAggr;

        public ConsoleService(IEventAggregator eventAgg)
        {
            this.mEventAggr = eventAgg;

            //_ea.GetEvent<ConsoleInputEvent>().Subscribe(HandleConsoleInput);
        }

        public void ConsoleInput(string inputText)
        {
            mEventAggr.GetEvent<ConsoleOutputEvent>().Publish(inputText + " Done.");
        }

        //private void HandleConsoleInput(string consoleCmdText)
        //{
        //    // _ea.GetEvent<ConsoleOutputEvent>().Publish(consoleCmdText + " Done.");
        //}


        private void HandleStartCommand()
        {

        }

        private void HandleStopCommand()
        {

        }

        private void HandleLoadSampleCommand()
        {

        }


        private void HandleExportLogCommand()
        {

        }
    }
}
