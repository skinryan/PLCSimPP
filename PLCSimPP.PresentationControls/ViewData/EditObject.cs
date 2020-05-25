using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Threading;
using BCI.PLCSimPP.PresentationControls.ValidationAttributes;

namespace BCI.PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// Edit Object attributes
    /// </summary>
    public class EditObject : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {
        private bool _isDeleted = false;
        /// <summary>
        /// Is object be deleted
        /// </summary>
        [Browsable(false)]
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }
        /// <summary>
        /// Is object value be changed
        /// </summary>
        [Browsable(false)]
        public bool IsValueChanged { get; set; }
        /// <summary>
        /// Property Changed evnet
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Property Changed notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="colName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void NotifyPropertyChanged(object sender, string colName, object oldValue, object newValue)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new EnhancedPropertyChangedEventArgs(sender, colName, oldValue, newValue));
        }
        /// <summary>
        /// Property Changed notification
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Edit object
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                if (IsDeleted)
                    return null;

                var itemsList = this.GetType().GetProperty(columnName).GetCustomAttributes(typeof(ValidationAttribute), false);

                for (int i = 0; i < itemsList.Length; ++i)
                {
                    var item = (ValidationAttribute)itemsList.ElementAt(i);
                    var injectSelfValidation = item as IInjectSelfValidation;
                    if (injectSelfValidation != null)
                        injectSelfValidation.Self = this;
                    if (item.GetType().GetProperty("Parent") != null)
                        item.GetType().GetProperty("Parent").SetValue(item, this, null);
                    object value = this.GetType().GetProperty(columnName).GetValue(this, null);
                    if (!item.IsValid(value))
                    {
                        string errorMessage = "";
                        if (string.IsNullOrEmpty(item.ErrorMessageResourceName))
                        {
                            errorMessage = item.ErrorMessage;
                        }
                        else
                        {
                            var rm = new ResourceManager(item.ErrorMessageResourceType.FullName, item.ErrorMessageResourceType.Assembly);
                            errorMessage = rm.GetString(item.ErrorMessageResourceName, Thread.CurrentThread.CurrentCulture);
                        }
                        ErrorInfos[columnName] = errorMessage;
                        if (ErrorOccured != null)
                            ErrorOccured(this, new DataErrorOccuredEventArgs(new DataErrorInformation(this, columnName, errorMessage)));
                        this.NotifyPropertyChanged("HaveError");
                        return errorMessage;
                    }
                }
                if (ErrorInfos.ContainsKey(columnName))
                {
                    ErrorInfos.Remove(columnName);
                    if (ErrorDied != null)
                        ErrorDied(this, new DataErrorDiedEventArgs(this, columnName));
                }


                //var items = this.GetType().GetProperty(columnName).GetCustomAttributes(typeof(ValidationAttribute), false);
                //var itemsList = items.ToList();
                //foreach (ValidationAttribute item in itemsList)

                return null;
            }
        }
        /// <summary>
        /// Error message
        /// </summary>
        [Browsable(false)]
        public string Error
        {
            get { return ""; }
        }

        private readonly Dictionary<string, string> mErrorInfos = new Dictionary<string, string>();
        /// <summary>
        /// Error infomations
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public Dictionary<string, string> ErrorInfos
        {
            get { return mErrorInfos; }
        }
        /// <summary>
        /// Is object have error
        /// </summary>
        [Browsable(false)]
        public bool HaveError { get { return ErrorInfos.Count > 0; } }
        /// <summary>
        /// Data Error Died event
        /// </summary>
        public event EventHandler<DataErrorDiedEventArgs> ErrorDied;
        /// <summary>
        /// Error Occured event
        /// </summary>
        public event EventHandler<DataErrorOccuredEventArgs> ErrorOccured;
    }
    /// <summary>
    /// Reject Object
    /// </summary>
    public class RejectObject : Attribute
    {

    }
}
