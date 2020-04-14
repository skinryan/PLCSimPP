namespace CommonLib.Dialog
{
    public enum PromptYesNoDialogResult
    {
        Yes,
        No
    }

    public interface IPromptYesNoDialog
    {
        PromptYesNoDialogResult Prompt(string messageArg, string captionArg);
    }
}
