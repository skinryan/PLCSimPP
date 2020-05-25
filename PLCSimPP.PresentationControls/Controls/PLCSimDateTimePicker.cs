using System;
using Xceed.Wpf.Toolkit;

namespace BCI.PLCSimPP.PresentationControls.Controls
{
    public class PLCSimDateTimePicker : DateTimePicker
    {
        /// <summary>
        /// On Value Changed
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            base.OnValueChanged(oldValue, newValue);
        }
    }
}
