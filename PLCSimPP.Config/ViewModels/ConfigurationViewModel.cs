using System;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Config.Controllers;
using BCI.PLCSimPP.Config.ViewDatas;
using BCI.PLCSimPP.PresentationControls;
using Prism.Commands;
using Prism.Events;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Services.Dialogs;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        
        private const string FILE_TYPE_EXE = "exe";
        private const string FILE_TYPE_XML = "xml";
        private readonly IEventAggregator mEventAggr;
        private readonly IAutomation mAutomation;
        private readonly IDialogService mDialogService;

        public ICommand EditSiteMapCommand { get; set; }
        public ICommand SelectFilePathCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SelectDCSimPathCommand { get; set; }
        public ICommand SelectDxCSimPathCommand { get; set; }

        /// <summary>
        /// Gets or sets ConfigurationController
        /// </summary>
        public ConfigurationController ConfigurationController
        {
            get;
            private set;
        }

        public ConfigurationViewModel(ConfigurationController configurationController, IEventAggregator eventAggr, IAutomation automation, IDialogService dialogService)
        {
            mEventAggr = eventAggr;
            ConfigurationController = configurationController;
            mAutomation = automation;
            mDialogService = dialogService;

            CancelCommand = new DelegateCommand(DoCancel);
            SelectFilePathCommand = new DelegateCommand<string>((fileType) =>
            {
                if (ConfigurationController.Data is ConfigurationViewData data)
                {
                    var path = DoSelectPath(fileType);
                    if (!string.IsNullOrEmpty(path))
                        data.SiteMapFilePath = path;
                }
            });

            SelectDCSimPathCommand = new DelegateCommand<string>((fileType) =>
            {
                if (ConfigurationController.Data is ConfigurationViewData data)
                {
                    var path = DoSelectPath(fileType);
                    if (!string.IsNullOrEmpty(path))
                        data.DcSimLocation = path;
                }
            });

            SelectDxCSimPathCommand = new DelegateCommand<string>((fileType) =>
            {
                if (ConfigurationController.Data is ConfigurationViewData data)
                {
                    var path = DoSelectPath(fileType);
                    if (!string.IsNullOrEmpty(path))
                        data.DxCSimLocation = path;
                }
            });

            EditSiteMapCommand = new DelegateCommand<string>((name) =>
            {
                if (ConfigurationController.Data is ConfigurationViewData data)
                {
                   var param = new DialogParameters();
                    param.Add("FilePath", data.SiteMapFilePath);
                    mDialogService.ShowDialog("SiteMapEditer", param, r =>
                    {
                    });
                }
            });
        }

        private string DoSelectPath(string fileType)
        {
            string filterStr = string.Empty;
            if (fileType == FILE_TYPE_EXE)
            {
                filterStr = "exe files (*.exe)|*.exe";
            }
            if (fileType == FILE_TYPE_XML)
            {
                filterStr = "XML files (*.xml)|*.xml";
            }

            var dialog = new OpenFileDialog()
            {
                Title = $"Select a {fileType} file", // "Select a text file"
                                                     //InitialDirectory = initialDirArg, // "c:\\"
                Filter = filterStr, // "txt files (*.txt)|*.txt|All files (*.*)|*.*"
                FilterIndex = 1, //Gets or sets the index of the filter currently selected in the file dialog box.
                                 //The index value of the first filter entry is 1.
                                 //RestoreDirectory = true //Gets or sets a value indicating whether the dialog box restores the current directory before closing.
            };

            var diaResult = dialog.ShowDialog();
            if (diaResult.HasValue && diaResult.Value)
            {
                return dialog.FileName.Trim();
            }

            return string.Empty;
        }

        private void DoCancel()
        {
            if (Leaving())
            {
                mEventAggr.GetEvent<NavigateEvent>().Publish(ViewName.DEVICE_LAYOUT);
            }
        }

        public bool Leaving()
        {
            if (ConfigurationController.Data.IsValueChanged)
            {
                var result = MessageBox.Show("Do you need to save the changed Settings?", "Warning", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (mAutomation.IsConnected)
                    {
                        MessageBox.Show(ConfigModule.CANT_SAVE_NOTIFICATION);
                        return false;
                    }

                    if (ConfigurationController.Save())
                    {
                        Thread.Sleep(500);
                        mEventAggr.GetEvent<ReLoadSiteMapEvent>().Publish(true);
                        return true;
                    }

                    return false;
                }
                else if (result == MessageBoxResult.No)
                {
                    ConfigurationController.LoadViewDatas();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Save information
        /// </summary>
        protected override void Save()
        {
            if (mAutomation.IsConnected)
            {
                MessageBox.Show(ConfigModule.CANT_SAVE_NOTIFICATION);
                return;
            }

            if (ConfigurationController.Save())
            {
                Thread.Sleep(500);
                mEventAggr.GetEvent<ReLoadSiteMapEvent>().Publish(true);
                MessageBox.Show("Save Success.");
            }
        }

    }
}
