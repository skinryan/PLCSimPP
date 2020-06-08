using System.Windows.Input;
using BCI.PLCSimPP.Comm.Models;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Forms;
using BCI.PLCSimPP.Config.Controllers;
using BCI.PLCSimPP.PresentationControls;
using Prism.Regions;
using BCI.PLCSimPP.Comm.Constants;
using Prism.Events;
using BCI.PLCSimPP.Comm.Events;
using System;
using BCI.PLCSimPP.Config.ViewDatas;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using System.Threading;
using BCI.PLCSimPP.Comm.Interfaces;
using DcSimCom;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private const string FILE_TYPE_EXE = "exe";
        private const string FILE_TYPE_XML = "xml";
        private readonly IEventAggregator mEventAggr;
        private readonly IAutomation mAutomation;

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

        public ConfigurationViewModel(ConfigurationController configurationController, IEventAggregator eventAggr, IAutomation automation)
        {
            mEventAggr = eventAggr;
            ConfigurationController = configurationController;
            mAutomation = automation;

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
                eventAggr.GetEvent<NavigateEvent>().Publish(name);
                eventAggr.GetEvent<LoadDataEvent>().Publish(name);
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

            var path = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName.Trim() : string.Empty;
            return path;
        }

        private void DoCancel()
        {
            if (CheckLeaving())
            {
                mEventAggr.GetEvent<NavigateEvent>().Publish(ViewName.DEVICE_LAYOUT);
            }
        }

        public bool CheckLeaving()
        {
            if (ConfigurationController.Data.IsValueChanged)
            {
                var result = MessageBox.Show("Do you need to save the changed Settings?", "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    ConfigurationController.Save();
                    return true;
                }
                else if (result == DialogResult.No)
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
                MessageBox.Show("The settings cannot be modified while the system is connected.");
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
