using System.ComponentModel;
using System.Runtime.CompilerServices;
using static BCI.PLCSimPP.PresentationControls.Annotations;

namespace BCI.PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// Base class of view data
    /// </summary>
    public class ViewDataBase : INotifyPropertyChanged, IViewData
    {
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raise property changed event
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
