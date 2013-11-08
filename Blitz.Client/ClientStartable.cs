using System.Windows;

using Blitz.Client.Shell;

using Naru.WPF.Menu;
using Naru.WPF.ModernUI.Assets.Icons;
using Naru.WPF.MVVM;

namespace Blitz.Client
{
    public class ClientStartable
    {
        private readonly ShellViewModel _shellViewModel;
        private readonly IMenuService _menuService;

        public ClientStartable(ShellViewModel shellViewModel, IMenuService menuService)
        {
            _shellViewModel = shellViewModel;
            _menuService = menuService;
        }

        public void Start()
        {
            ConfigureDefaultMenus();

            CreateAndShowShell();
        }

        private void CreateAndShowShell()
        {
            // Setup the Shell
            var shellView = ViewService.CreateView(_shellViewModel.GetType());
            ViewService.BindViewModel(shellView, _shellViewModel);

            // Setup application shutdown
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            // Show the Shell and activate it
            Application.Current.MainWindow = (Window)shellView;
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.Activate();
        }

        private void ConfigureDefaultMenus()
        {
            var newMenuGroupItem = _menuService.CreateMenuGroupItem();
            newMenuGroupItem.DisplayName = "New";
            newMenuGroupItem.ImageName = IconNames.NEW;

            _menuService.Items.Add(newMenuGroupItem);
        }
    }
}