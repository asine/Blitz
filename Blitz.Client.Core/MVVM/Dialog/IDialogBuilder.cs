using System.Windows.Documents;

namespace Blitz.Client.Core.MVVM.Dialog
{
    public interface IDialogBuilder
    {
        IDialogBuilder WithDialogType(DialogType dialogType);

        IDialogBuilder WithAnswers(params Answer[] answers);

        IDialogBuilder WithTitle(string title);

        IDialogBuilder WithMessage(string message);

        Answer Show();
    }
}