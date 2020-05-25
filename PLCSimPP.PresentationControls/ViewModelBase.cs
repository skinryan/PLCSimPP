using System.ComponentModel;
using System.Runtime.CompilerServices;
using static BCI.PLCSimPP.PresentationControls.Annotations;
using System.Windows.Input;
using Prism.Commands;

namespace BCI.PLCSimPP.PresentationControls
{
    /// <summary>
    /// This abstract class act as a base class for all view model classes
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModelBase()
        {
            SaveCommand = new DelegateCommand(Save);
            LoadInitializedDataCommand = new DelegateCommand(LoadInitializedData);
        }

        /// <summary>
        /// Save
        /// </summary>
        protected virtual void Save() { }
        /// <summary>
        /// Load Initialized Data
        /// </summary>
        protected virtual void LoadInitializedData() { }

        #region INotifyPropertyChanging Members
        /// <summary>
        /// Property changing event
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Property changed event
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
        [NotifyPropertyChangedInvocator]
        public virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
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

        #region Commands

        /// <summary>
        /// SaveCommand
        /// </summary>
        public ICommand SaveCommand
        {
            get;
            set;
        }
        /// <summary>
        /// Load Initialized Data Command
        /// </summary>
        public ICommand LoadInitializedDataCommand
        {
            get;
            set;
        }
        #endregion
    }
}
