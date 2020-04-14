using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace PLCSimPP.PresentationCommon.ViewData
{
    /// <summary>
    /// Base class for setup view data
    /// </summary>
    public class EditViewDataBase : INotifyPropertyChanged, INotifyDataErrorInfo, IEditViewData, IDataErrorInfo
    {
        /// <summary>
        /// Initialize view data
        /// </summary>
        public virtual void Initialize()
        {
            this.mErrors.Clear();
            this.mRedo.Clear();
            this.mUndo.Clear();
        }
        /// <summary>
        /// Loaded
        /// </summary>
        public void Loaded()
        {
            SetLoaded(this);
        }
        /// <summary>
        /// Set loaded
        /// </summary>
        /// <param name="o"></param>
        private void SetLoaded(object o)
        {

            if (o is IList)
            {
                var v = o as IList;
                foreach (object item in v)
                {
                    if (item is EditObject)
                    {
                        var obj = item as EditObject;
                        obj.IsValueChanged = false;
                    }

                    var items =
                        o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var citem in items)
                    {
                        object vvv = citem.GetValue(o, null);

                        if (vvv != null)
                            SetLoaded(vvv);

                    }
                }
            }
            else
            {
                if (o is INotifyPropertyChanged)
                {
                    if (o is EditObject)
                    {
                        var obj = o as EditObject;
                        obj.IsValueChanged = false;
                    }
                    var items =
                        o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                  BindingFlags.DeclaredOnly);
                    foreach (var item in items)
                    {
                        object vvv = item.GetValue(o, null);

                        if (vvv != null)
                            SetLoaded(vvv);

                    }
                }
            }

            RaisePropertyChanged("ErrorCount");
            RaisePropertyChanged("HaveError");

        }
        /// <summary>
        /// Constructor
        /// </summary>
        public EditViewDataBase()
        {
            this.SetPropertyChanged(this);
        }
        /// <summary>
        /// Set property changed event to object
        /// </summary>
        /// <param name="o"></param>
        protected void SetPropertyChanged(object o)
        {

            if (o is INotifyCollectionChanged)
            {
                var v = o as INotifyCollectionChanged;
                if (o.GetType().GetCustomAttributes(typeof(RejectObject), false).Any())
                    return;
                v.CollectionChanged += ViewDataBaseCollectionChanged;
            }
            else
            {
                if (o is INotifyPropertyChanged)
                {
                    var v = o as INotifyPropertyChanged;
                    v.PropertyChanged -= ViewDataBasePropertyChanged;
                    v.PropertyChanged += ViewDataBasePropertyChanged;
                    var items = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var item in items)
                    {
                        object vvv = item.GetValue(o, null);

                        if (vvv != null)
                            SetPropertyChanged(vvv);

                    }
                }

                if (o is INotifyDataErrorInfo)
                {
                    var v = o as INotifyDataErrorInfo;
                    v.ErrorDied -= VOnErrorDied;
                    v.ErrorDied += VOnErrorDied;

                    v.ErrorOccured -= VOnErrorOccured;
                    v.ErrorOccured += VOnErrorOccured;
                }
            }
        }

        private void VOnErrorOccured(object sender, DataErrorOccuredEventArgs dataErrorOccuredEventArgs)
        {
            if (!Errors.Any(a => a.Source == dataErrorOccuredEventArgs.DataErrorInformation.Source && a.ColumnName == dataErrorOccuredEventArgs.DataErrorInformation.ColumnName && a.ErrorMessage == dataErrorOccuredEventArgs.DataErrorInformation.ErrorMessage))
            {
                Errors.Add(dataErrorOccuredEventArgs.DataErrorInformation);
            }
            RaisePropertyChanged("ErrorCount");
            RaisePropertyChanged("HaveError");
        }

        private void VOnErrorDied(object sender, DataErrorDiedEventArgs dataErrorDiedEventArgs)
        {
            if (dataErrorDiedEventArgs.ColumnName == "UsageType")
                Errors.RemoveAll(a => a.ColumnName == dataErrorDiedEventArgs.ColumnName);
            else
                Errors.RemoveAll(
                a => a.Source == dataErrorDiedEventArgs.Source && a.ColumnName == dataErrorDiedEventArgs.ColumnName);
            RaisePropertyChanged("ErrorCount");
            RaisePropertyChanged("HaveError");
        }

        private List<DataErrorInformation> mErrors = new List<DataErrorInformation>();
        /// <summary>
        /// Error informations
        /// </summary>
        public List<DataErrorInformation> Errors
        {
            get { return mErrors; }
        }
        /// <summary>
        /// Raise property change event
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Raise property changed notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="colName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void NotifyPropertyChanged(object sender, string colName, object oldValue, object newValue)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new EnhancedPropertyChangedEventArgs(sender, colName, oldValue, newValue));
                var v = newValue as INotifyPropertyChanged;
                if (v != null)
                {
                    v.PropertyChanged -= new PropertyChangedEventHandler(ViewDataBasePropertyChanged);
                    v.PropertyChanged += new PropertyChangedEventHandler(ViewDataBasePropertyChanged);
                }
                if (newValue != null)
                    SetPropertyChanged(newValue);
            }
        }
        /// <summary>
        /// Collection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewDataBaseCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var oitem in e.NewItems)
                {
                    var item = oitem as EditObject;
                    if (item == null)
                        continue;

                    item.IsDeleted = false;
                    item.PropertyChanged -= ViewDataBasePropertyChanged;
                    item.PropertyChanged += ViewDataBasePropertyChanged;

                    this.SetPropertyChanged(item);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oitem in e.OldItems)
                {
                    var item = oitem as EditObject;
                    if (item == null)
                        continue;
                    mErrors.RemoveAll(a => a.Source == item);
                    item.IsDeleted = true;
                }
                RaisePropertyChanged("ErrorCount");
                RaisePropertyChanged("HaveError");
            }

            if (!IsLoaded)
                return;
            this.mUndo.Push(new EnhancedNotifyCollectionChangedEventArgs(sender, e));
            this.mRedo.Clear();
            RaisePropertyChanged("IsValueChanged");

        }
        /// <summary>
        /// Property changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewDataBasePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            var v = e as EnhancedPropertyChangedEventArgs;
            if (v == null)
                return;
            var v2 = v.SourceObject as EditObject;
            if (v2 != null)
                v2.IsValueChanged = true;
            mUndo.Push(v);
            mRedo.Clear();
            RaisePropertyChanged("IsValueChanged");
        }
        #region Undo Fremawork 
        private readonly Stack<IReset> mUndo = new Stack<IReset>();
        private readonly Stack<IReset> mRedo = new Stack<IReset>();
        /// <summary>
        /// If can do undo
        /// </summary>
        public bool CanUnDo { get { return mUndo.Count != 0; } }
        /// <summary>
        /// If can do redo
        /// </summary>
        public bool CanReDo { get { return mRedo.Count != 0; } }
        /// <summary>
        /// Clear history
        /// </summary>
        public void ClearHistory()
        {
            IsLoaded = false;
            while (true)
            {
                if (!CanUnDo)
                    break;
                var undo = mUndo.Pop();
                undo.Undo();
                mRedo.Push(undo);
            }
            IsLoaded = true;
            RaisePropertyChanged("IsValueChanged");
            RaisePropertyChanged("ErrorCount");
        }
        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            if (!CanUnDo)
                return;
            IsLoaded = false;
            var undo = mUndo.Pop();
            undo.Undo();
            mRedo.Push(undo);
            IsLoaded = true;
            RaisePropertyChanged("IsValueChanged");
            RaisePropertyChanged("ErrorCount");
        }
        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            if (!CanReDo)
                return;
            IsLoaded = false;
            var redo = mRedo.Pop();
            redo.Redo();
            mUndo.Push(redo);
            IsLoaded = true;
            RaisePropertyChanged("IsValueChanged");
            RaisePropertyChanged("ErrorCount");
        }
        #endregion
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool mIsLoaded = false;
        /// <summary>
        /// Is Loaded
        /// </summary>
        public bool IsLoaded
        {
            get { return mIsLoaded; }
            set { mIsLoaded = value; }
        }
        /// <summary>
        /// Is value change
        /// </summary>
        public bool IsValueChanged
        {
            get { return CanUnDo; }
        }
        /// <summary>
        /// Save edit data
        /// </summary>
        public void Save()
        {
            this.mRedo.Clear();
            this.mUndo.Clear();
            RaisePropertyChanged("IsValueChanged");
            Loaded();
        }
        /// <summary>
        /// Is there any error
        /// </summary>
        public bool HaveError
        {
            get { return ErrorCount > 0; }
        }
        /// <summary>
        /// Error count
        /// </summary>
        public int ErrorCount
        {
            get { return this.Errors.Count; }
        }
        /// <summary>
        /// Error died event
        /// </summary>
        public event EventHandler<DataErrorDiedEventArgs> ErrorDied;
        /// <summary>
        /// Error occured event
        /// </summary>
        public event EventHandler<DataErrorOccuredEventArgs> ErrorOccured;
        /// <summary>
        /// Edit view data object
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {

                var items = this.GetType().GetProperty(columnName).GetCustomAttributes(typeof(ValidationAttribute),
                                                                                        false);
                if (items != null)
                {
                    foreach (ValidationAttribute item in items)
                    {
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
                                var rm = new ResourceManager(item.ErrorMessageResourceType.FullName,
                                                                item.ErrorMessageResourceType.Assembly);
                                errorMessage = rm.GetString(item.ErrorMessageResourceName,
                                                            Thread.CurrentThread.CurrentUICulture);
                            }
                            ErrorInfos[columnName] = errorMessage;
                            if (ErrorOccured != null)
                                ErrorOccured(this, new DataErrorOccuredEventArgs(new DataErrorInformation(this, columnName, errorMessage)));
                            Error = errorMessage;
                            this.RaisePropertyChanged("Error");
                            return errorMessage;
                        }
                    }
                }


                if (ErrorInfos.ContainsKey(columnName))
                {
                    ErrorInfos.Remove(columnName);
                    if (ErrorDied != null)
                        ErrorDied(this, new DataErrorDiedEventArgs(this, columnName));
                }

                Errors.RemoveAll(a => a.ColumnName == columnName);
                Error = ""; this.RaisePropertyChanged("Error");
                return "";
            }
        }
        private Dictionary<string, string> mErrorInfos = new Dictionary<string, string>();

        /// <summary>
        /// Error informations
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public Dictionary<string, string> ErrorInfos
        {
            get { return mErrorInfos; }
        }
        /// <summary>
        /// Error information
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public string Error { get; private set; }

    }

    /// <summary>
    /// Reset edit status interface
    /// </summary>
    public interface IReset
    {
        /// <summary>
        /// Undo
        /// </summary>
        void Undo();
        /// <summary>
        /// Redo
        /// </summary>
        void Redo();
    }
}
