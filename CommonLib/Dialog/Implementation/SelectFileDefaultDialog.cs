using System.Windows.Forms;

namespace CommonLib.Dialog.Implementation
{
    public class SelectFileDefaultDialog : ISelectFileDialog
    {
        #region ISelectFileDialog Members

        public string Select(string titleArg, string initialDirArg, string filterArg, bool restoreDirectoryArg)
        {
            var dialog = new OpenFileDialog
                         {
                             Title = titleArg, // "Select a text file"
                             InitialDirectory = initialDirArg, // "c:\\"
                             Filter = filterArg, // "txt files (*.txt)|*.txt|All files (*.*)|*.*"
                             FilterIndex = 1, //Gets or sets the index of the filter currently selected in the file dialog box.
                                              //The index value of the first filter entry is 1.
                             RestoreDirectory = true //Gets or sets a value indicating whether the dialog box restores the current directory before closing.
                         };

            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }

        #endregion
    }
}
