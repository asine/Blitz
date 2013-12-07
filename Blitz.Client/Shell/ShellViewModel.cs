using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.Menu;
using Naru.WPF.Presentation;
using Naru.WPF.Windows.Controls;
using Naru.WPF.MVVM;

using Blitz.Client.Settings.Appearance;

using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.UserInteractionHost;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Shell
{
    public class ShellViewModel : Workspace, IWindowViewModel
    {
        private readonly Func<IUserInteractionHostViewModel> _userInteractionHostViewModelFactory;

        public BindableCollection<IMenuItem> MenuItems { get; private set; }

        public BindableCollection<IToolBarItem> ToolBarItems { get; private set; }

        public BindableCollection<Link> TitleLinks { get; private set; }

        public BindableCollection<IViewModel> Items { get; private set; }

        #region ShowUserInteractionHost

        private bool _showUserInteractionHost;

        public bool ShowUserInteractionHost
        {
            get { return _showUserInteractionHost; }
            private set
            {
                _showUserInteractionHost = value;
                RaisePropertyChanged(() => ShowUserInteractionHost);
            }
        }

        #endregion

        #region UserInteractionHost

        private IUserInteractionHostViewModel _userInteractionHost;

        public IUserInteractionHostViewModel UserInteractionHost
        {
            get { return _userInteractionHost; }
            private set
            {
                _userInteractionHost = value;
                RaisePropertyChanged(() => UserInteractionHost);
            }
        }

        #endregion

        public ShellViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                              IToolBarService toolBarService, IMenuService menuService, IEventStream eventStream,
                              IAppearanceViewModel appearanceViewModel,
                              BindableCollection<IViewModel> itemsCollection,
                              BindableCollection<Link> linkCollection,
                              IUserInteraction userInteraction,
                              Func<IUserInteractionHostViewModel> userInteractionHostViewModelFactory)
            : base(log, scheduler, standardDialog)
        {
            _userInteractionHostViewModelFactory = userInteractionHostViewModelFactory;
            ToolBarItems = toolBarService.Items;
            MenuItems = menuService.Items;
            Items = itemsCollection;

            TitleLinks = linkCollection;

            TitleLinks.Add(new Link
                           {
                               DisplayName = "Appearance",
                               Command = new DelegateCommand(() => userInteraction.ShowModalAsync(appearanceViewModel))
                           });

            eventStream.Of<IViewModel>()
                .ObserveOn(Scheduler.Dispatcher.RX)
                .Subscribe(x => Items.Add(x));

            userInteraction.RegisterHandler(UserInteractionModalHandler);

            // Dummy menu item to test UserInterations
            var menuGroupItem = menuService.CreateMenuGroupItem();
            menuGroupItem.DisplayName = "Test";

            var menuItem = menuService.CreateMenuButtonItem();
            menuItem.DisplayName = "User Interactions";
            menuItem.Command = new DelegateCommand(() => standardDialog.InformationAsync("Title", "Message")
                                                                       .Do(x =>
                                                                             {

                                                                             }, Scheduler.Task.TPL));
            menuGroupItem.Items.Add(menuItem);
            menuService.Items.Add(menuGroupItem);
        }

        private Task UserInteractionModalHandler<TViewModel>(TViewModel viewModel)
            where TViewModel : IUserInteractionViewModel
        {
            var tcs = new TaskCompletionSource<object>();

            Scheduler.Dispatcher.ExecuteSync(() =>
                                             {
                                                 var userInteractionHostViewModel = _userInteractionHostViewModelFactory();
                                                 userInteractionHostViewModel.Initialise(viewModel);

                                                 IDisposable closing = null;
                                                 closing = userInteractionHostViewModel.Closed
                                                                    .Subscribe(x =>
                                                                               {
                                                                                   tcs.TrySetResult(null);

                                                                                   ShowUserInteractionHost = false;

                                                                                   if (closing != null)
                                                                                   {
                                                                                       closing.Dispose();
                                                                                   }
                                                                               });

                                                 UserInteractionHost = userInteractionHostViewModel;
                                                 ShowUserInteractionHost = true;
                                             });

            return tcs.Task;
        }
    }
}