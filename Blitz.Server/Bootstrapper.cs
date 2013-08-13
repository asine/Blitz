﻿using System;
using System.ServiceModel;

using Agatha.ServiceLayer;
using Agatha.ServiceLayer.WCF;
using Agatha.Unity;

using Blitz.Common.Agatha;
using Blitz.Common.Core;
using Blitz.Server.Core;

using Microsoft.Practices.Unity;

namespace Blitz.Server
{
    public class Bootstrapper
    {
        private const string END_POINT = "http://localhost:1234/Agatha";

        public Bootstrapper()
        {
            var container = new UnityContainer();

            container.RegisterType<ILog, ConsoleLogger>();

            InitialiseAgatha(container);

            Console.WriteLine("EndPoint - {0}", END_POINT);

            var baseAddress = new Uri(END_POINT);
            Host = new ServiceHost(typeof(WcfRequestProcessor), baseAddress);
            Host.Open();
        }

        public ServiceHost Host { get; private set; }

        private static void InitialiseAgatha(IUnityContainer container)
        {
            new ServiceLayerConfiguration(typeof(AssemblyHook).Assembly, typeof(Common.AssemblyHook).Assembly, new Container(container))
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(typeof(Common.AssemblyHook).Assembly);
        }
    }
}