using BCI.PLCSimPP.PresentationControls.ViewData;
using System.ComponentModel;
using System.Windows.Forms;
namespace BCI.PLCSimPP.PresentationControls
{
    /// <summary>
    /// Base class of setup controllers
    /// </summary>
    public class ControllerBase : INotifyPropertyChanging, INotifyPropertyChanged, IController
    {
        #region INotifyPropertyChanging Members
        /// <summary>
        /// Property Changing event
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Administrative Properties

        /// <summary>
        /// Whether the view model should ignore property-change events.
        /// </summary>
        public virtual bool IgnorePropertyChangeEvents { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanged == null) return;

            // Raise event
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, e);
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">The name of the changing property.</param>
        public virtual void RaisePropertyChanging(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanging == null) return;

            // Raise event
            var e = new PropertyChangingEventArgs(propertyName);
            PropertyChanging(this, e);
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>

        public ControllerBase()
        {
        }

        void StatusManagementSystemReadyEvent()
        {
            RaisePropertyChanged("CanExportImport");
            RaisePropertyChanged("CanEdit");
        }




        /// <summary>
        /// The data of controller
        /// </summary>
        public IEditViewData Data { get; set; }

        /// <summary>
        /// Data validation
        /// </summary>
        /// <returns></returns>
        public bool CheckOut()
        {
            //TODO: yes no cancel prompt
            var result = true;
            if (Data.IsValueChanged)
            {
                //var dialogResult = mPromptSaveDiscardCancelDialog.Prompt(Properties.Resources.DataChangedPrompting, Properties.Resources.Save,
                //    CabernetResourceManager.GetEnglishResources(nameof(Properties.Resources.DataChangedPrompting), typeof(Properties.Resources)),
                //    CabernetResourceManager.GetEnglishResources(nameof(Properties.Resources.Save), typeof(Properties.Resources)),
                //    mSetupLog, mSecurity.CurrentUserName);

                //if (dialogResult == PromptYesNoCancelDialogResult.Yes)
                //    result = Save();
                //else if (dialogResult == PromptYesNoCancelDialogResult.No)
                //{
                //    result = true;
                //    LoadViewDatas();
                //}
                //else if (dialogResult == PromptYesNoCancelDialogResult.Cancel)
                //    result = false;
            }
            return result;
        }


        /// <summary>
        /// Saving event
        /// </summary>
        public event ControllerSaveHandle Saving;

        /// <summary>
        /// Loading data event
        /// </summary>
        public event ControllerLoadViewDatasHandle LoadViewDatasing;
        /// <summary>
        /// Data validation event
        /// </summary>
        public event ControllerValidationErrorHandle ValidationError;

        /// <summary>
        /// Save setup configuration
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (Data.HaveError)
            {                
                MessageBox.Show(Properties.Resources.ContentError, Properties.Resources.Error);
                if (ValidationError != null)
                    ValidationError();
                return false;
            }
            if (Saving != null)
                Saving();
            Data.Save();

            return true;

        }

        /// <summary>
        /// Load setup data
        /// </summary>
        public void LoadViewDatas()
        {
            Data.IsLoaded = false;
            Data.Initialize();
            if (LoadViewDatasing != null)
                LoadViewDatasing();
            Data.IsLoaded = true;
            Data.Loaded();
        }
    }
}
