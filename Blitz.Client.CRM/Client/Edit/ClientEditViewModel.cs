using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Assets.Icons;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Blitz.Client.CRM.Client.Edit
{
    public class ClientEditViewModel : Workspace
    {
        private readonly IClientEditService _service;

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public ClientModel Model { get; private set; }

        public BindableCollection<string> Genders { get; private set; }

        public ClientEditViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                                   IToolBarService toolBarService, BindableCollection<IToolBarItem> toolBarCollection,
                                   IClientEditService service, ClientModel model, BindableCollection<string> genderCollection)
            : base(log, scheduler, standardDialog)
        {
            _service = service;

            ToolBarItems = toolBarCollection;

            Model = model;

            Genders = genderCollection;

            CreateSaveToolBar(toolBarService);
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("Loading")
                                .Then(() => _service.GetGendersAsync(), Scheduler.Task.TPL)
                                .Do(x => Genders.AddRangeAsync(x), Scheduler.Dispatcher.TPL)
                                .Finally(() => BusyViewModel.InActive(), Scheduler.Dispatcher.TPL);
        }

        private void CreateSaveToolBar(IToolBarService toolBarService)
        {
            var saveCommand = new ObservableCommand(Model.IsValid);
            saveCommand.Executed
                       .TakeUntil(BusyViewModel.BusyLatch)
                       .ObserveOn(Scheduler.Task.RX)
                       .TakeUntil(Closed)
                       .Subscribe(_ => _service.SaveAsync());

            var saveToolBarItem = toolBarService.CreateToolBarButtonItem();
            saveToolBarItem.DisplayName = "Save";
            saveToolBarItem.ImageName = IconNames.SAVE;
            saveToolBarItem.Command = saveCommand;

            ToolBarItems.Add(saveToolBarItem);

            this.SyncToolBarItemWithViewModelActivationState(saveToolBarItem);
        }

        protected override void CleanUp()
        {
            Model.Dispose();
        }
    }
}