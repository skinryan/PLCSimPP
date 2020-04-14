using System.ComponentModel;

namespace PLCSimPP.PresentationCommon.ViewData
{
    /// <summary>
    /// Enhanced Property Changed Event Args
    /// </summary>
    public class EnhancedPropertyChangedEventArgs : PropertyChangedEventArgs, IReset
    {
        /// <summary>
        /// Object
        /// </summary>
        public object SourceObject { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public object SourceValue { get; set; }
        /// <summary>
        /// New value
        /// </summary>
        public object NewValue { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="propertyName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public EnhancedPropertyChangedEventArgs(object sender, string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            SourceObject = sender;
            SourceValue = oldValue;
            NewValue = newValue;
        }
        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            SourceObject.GetType().GetProperty(PropertyName).SetValue(SourceObject, SourceValue, null);
        }
        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            SourceObject.GetType().GetProperty(PropertyName).SetValue(SourceObject, NewValue, null);
        }
    }
}

