namespace CommonLib.Dialog
{
    public interface ISelectFileDialog
    {
        string Select(string titleArg, string initialDirArg, string filterArg, bool restoreDirectoryArg);
    }
}
