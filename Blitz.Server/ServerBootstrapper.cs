using System;
using System.ServiceModel;

using Agatha.Common.WCF;
using Agatha.ServiceLayer;
using Agatha.ServiceLayer.WCF;

using Blitz.Common.Core;
using Blitz.Server.Core;

using Microsoft.Practices.Unity;

namespace Blitz.Server
{
    public class ServerBootstrapper
    {
        private const string END_POINT = "http://localhost:1234/Agatha";

        public ServerBootstrapper()
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
            new ServiceLayerConfiguration(
                typeof(AssemblyHook).Assembly, typeof(Common.AssemblyHook).Assembly, new Agatha.Unity.Container(container)).
                Initialize();

            KnownTypeProvider.RegisterDerivedTypesOf<object>(new[] { typeof(Common.AssemblyHook) });
        }
    }
}