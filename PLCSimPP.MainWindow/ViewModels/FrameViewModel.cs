using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace PLCSimPP.MainWindow.ViewModels
{
    public class FrameViewModel : BindableBase
    {
        private ILogService logger;

        public DelegateCommand ChangeTitleCommand { get; private set; }

        public FrameViewModel(ILogService LoggerService)
        {
            logger = LoggerService;

            logger.LogSys("Frame view Initialized.");
        }

        
    }
}
