namespace Blitz.Client.Core.MVVM.Dialog
{
    public interface IStandardDialogBuilder
    {
        Answer Question(string title, string message);
        Answer Question(string title, string message, params Answer[] possibleResponens);
        Answer Warning(string title, string message);
        Answer Warning(string title, string message, params Answer[] possibleResponens);
        Answer Information(string title, string message);
        Answer Information(string title, string message, params Answer[] possibleResponens);
        Answer Error(string title, string message);
        Answer Error(string title, string message, params Answer[] possibleResponens);
    }
}