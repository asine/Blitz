using System.Collections.Generic;

namespace Blitz.Client.Core.MVVM.Dialog
{
    public class DialogBuilder : IDialogBuilder
    {
        private readonly IViewService _viewService;
        private readonly DialogViewModel _dialogViewModel;
        private readonly List<Answer> _answers = new List<Answer>();

        private DialogType _dialogType;
        private string _title;
        private string _message;

        public DialogBuilder(IViewService viewService, DialogViewModel dialogViewModel)
        {
            _viewService = viewService;
            _dialogViewModel = dialogViewModel;
        }

        public IDialogBuilder WithDialogType(DialogType dialogType)
        {
            _dialogType = dialogType;

            return this;
        }

        public IDialogBuilder WithAnswers(params Answer[] answers)
        {
            foreach (var answer in answers)
            {
                _answers.Add(answer);
            }

            return this;
        }

        public IDialogBuilder WithTitle(string title)
        {
            _title = title;

            return this;
        }

        public IDialogBuilder WithMessage(string message)
        {
            _message = message;

            return this;
        }

        public Answer Show()
        {
            var viewModel = _dialogViewModel;
            viewModel.Initialise(_dialogType, _answers, _title, _message);
            _viewService.ShowModel(viewModel);

            return viewModel.SelectedAnswer;
        }
    }
}