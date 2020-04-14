using System.Windows.Forms;

namespace CommonLib.Dialog.Implementation
{
    public class DisplayMessaegDefaultDialog : IDisplayMessageDialog
    {
        #region IDisplayMessageDialog Members

        public void Display(string messageArg, string captionArg)
        {
            MessageBox.Show(messageArg, captionArg, MessageBoxButtons.OK);
        }

        #endregion
    }
}
