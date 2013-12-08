using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.ContextMenu;
using Naru.WPF.Dialog;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.CRM.Client.Edit
{
    public class ClientEditViewModel : Workspace
    {
        private readonly IClientEditService _service;

        public ClientModel Model { get; private set; }

        public BindableCollection<string> Genders { get; private set; }

        public ClientEditViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                                   IClientEditService service, ClientModel model, BindableCollection<string> genderCollection) 
            : base(log, scheduler, standardDialog)
        {
            _service = service;

            Model = model;

            Genders = genderCollection;
        }

        protected override Task OnInitialise()
        {
            return BusyViewModel.ActiveAsync("Loading")
                                .Then(() => _service.GetGendersAsync(), Scheduler.Task.TPL)
                                .Do(x => Genders.AddRangeAsync(x), Scheduler.Dispatcher.TPL)
                                .Finally(() => BusyViewModel.InActive(), Scheduler.Dispatcher.TPL);
        }

        protected override void CleanUp()
        {
            Model.Dispose();
        }
    }
}