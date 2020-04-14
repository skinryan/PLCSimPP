using System.Windows.Forms;

namespace CommonLib.Dialog.Implementation
{
    public class SaveFileDefaultDialog : ISaveFileDialog
    {
        #region ISaveFileDialog Members

        public string Select(string titleArg, string initialDirArg, string filterArg, bool restoreDirectoryArg)
        {
            var dialog = new SaveFileDialog
                         {
                             Title = titleArg, // "Save file as..."
                             InitialDirectory = initialDirArg,
                             Filter = filterArg, // "Text files (*.txt)|*.txt|All files (*.*)|*.*"
                             RestoreDirectory = restoreDirectoryArg
                         };
          
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }

        #endregion
    }
}
