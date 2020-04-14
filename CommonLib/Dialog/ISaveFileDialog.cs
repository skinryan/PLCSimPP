namespace CommonLib.Dialog
{
    public interface ISaveFileDialog
    {
        string Select(string titleArg, string initialDirArg, string filterArg, bool restoreDirectoryArg);
    }
}
