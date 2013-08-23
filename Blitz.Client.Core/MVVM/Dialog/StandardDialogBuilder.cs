namespace Blitz.Client.Core.MVVM.Dialog
{
    public class StandardDialogBuilder : IStandardDialogBuilder
    {
        private readonly IDialogBuilder<Answer> _dialogBuilder;

        public StandardDialogBuilder(IDialogBuilder<Answer> dialogBuilder)
        {
            _dialogBuilder = dialogBuilder;
        }

        public Answer Question(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(Answer.Yes, Answer.No)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Question(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Warning(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Warning(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Information(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Information(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Error(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Answer Error(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }
    }
}