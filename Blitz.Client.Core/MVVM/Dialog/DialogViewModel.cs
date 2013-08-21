using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Core.MVVM.Dialog
{
    public class DialogViewModel : Workspace
    {
        public ObservableCollection<DialogItemViewModel> Answers { get; private set; }

        public Answer SelectedAnswer { get; private set; }

        public string Message { get; private set; }

        public DelegateCommand<DialogItemViewModel> ExecuteCommand { get; private set; } 

        public DialogViewModel(ILog log, IDispatcherService dispatcherService) 
            : base(log, dispatcherService)
        {
            Answers = new ObservableCollection<DialogItemViewModel>();

            ExecuteCommand = new DelegateCommand<DialogItemViewModel>(x =>
            {
                SelectedAnswer = x.Response;

                Close();
            });
        }

        public void Initialise(DialogType dialogType, List<Answer> answers, string title, string message)
        {
            DisplayName = title;
            Message = message;

            for (var index = 0; index < answers.Count; index++)
            {
                var answer = new DialogItemViewModel
                {
                    Response = answers[index],
                    IsDefault = index == 0,
                    IsCancel = index == answers.Count - 1
                };
                Answers.Add(answer);
            }

            var lastButton = Answers.LastOrDefault();
            if (lastButton == null) return;

            SelectedAnswer = lastButton.Response;
        }
    }
}