using System.Windows.Forms;

namespace CommonLib.Dialog.Implementation
{
    public class PromptYesNgDefaultDialog : IPromptYesNoDialog
    {
        #region IPromptYesNoDialog Members

        public PromptYesNoDialogResult Prompt(string messageArg, string captionArg)
        {
            var result = MessageBox.Show(messageArg, captionArg,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return result == DialogResult.Yes ? PromptYesNoDialogResult.Yes : PromptYesNoDialogResult.No;
        }

        #endregion
    }
}
