﻿using System.Windows;

namespace Blitz.Client
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}