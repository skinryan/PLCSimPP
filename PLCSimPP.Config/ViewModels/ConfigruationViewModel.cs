using System.Windows.Input;
using BCI.PLCSimPP.Comm.Models;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Forms;
using BCI.PLCSimPP.Config.Controllers;
using BCI.PLCSimPP.PresentationControls;
using Prism.Regions;
using BCI.PLCSimPP.Comm.Constant;
using Prism.Events;
using BCI.PLCSimPP.Comm.Events;
using System;
using BCI.PLCSimPP.Config.ViewDatas;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using System.Threading;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class ConfigruationViewModel : ViewModelBase
    {

        const string FILETYPE_EXE = "exe";
        const string FILETYPE_XML = "xml";
        private ClientConfigInfo mConfigInfo;
        private readonly IEventAggregator mEventAggr;

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

        public ConfigruationViewModel(ConfigurationController configurationController, IEventAggregator eventAggr)
        {
            mEventAggr = eventAggr;
            ConfigurationController = configurationController;

            CancelCommand = new DelegateCommand(DoCancel);
            SelectFilePathCommand = new DelegateCommand<string>((fileType) =>
            {
                var data = ConfigurationController.Data as ViewDatas.ConfigruationViewData;
                if (data != null)
                {
                    var path = DoSelectPath(fileType);
                    if (!string.IsNullOrEmpty(path))
                        data.SitemapFilePath = path;

                }
            });

            SelectDCSimPathCommand = new DelegateCommand<string>((fileType) =>
            {
                var data = ConfigurationController.Data as ViewDatas.ConfigruationViewData;
                if (data != null)
                {
                    var path = DoSelectPath(fileType);
                    if (!string.IsNullOrEmpty(path))
                        data.DcSimLocation = path;
                }

            });

            SelectDxCSimPathCommand = new DelegateCommand<string>((fileType) =>
            {
                var data = ConfigurationController.Data as ViewDatas.ConfigruationViewData;
                if (data != null)
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
            if (fileType == FILETYPE_EXE)
            {
                filterStr = "exe files (*.exe)|*.exe";
            }
            if (fileType == FILETYPE_XML)
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
            if (ConfigurationController.Data.IsValueChanged)
            {
                var result = MessageBox.Show("Do you need to save the changed Settings?", "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    ConfigurationController.Save();
                }
                else if (result == DialogResult.No)
                {
                    ConfigurationController.LoadViewDatas();
                }
                else
                {
                    return;
                }
            }

            mEventAggr.GetEvent<NavigateEvent>().Publish("DeviceLayout");
        }

        /// <summary>
        /// Save information
        /// </summary>
        protected override void Save()
        {
            var data = (ConfigruationViewData)ConfigurationController.Data;

            var config = ServiceLocator.Current.GetInstance<IConfigService>();
            var oldpath = config.ReadSysConfig().SiteMapPath;

            if (ConfigurationController.Save())
            {
                Thread.Sleep(500);
                mEventAggr.GetEvent<ReLoadSiteMapEvent>().Publish(true);

                MessageBox.Show("Save Sucess");
            }
        }
        /// <summary>
        /// Load data
        /// </summary>
        protected override void LoadInitializedData()
        {
            //ConfigurationController.LoadViewDatas();
        }

    }
}
