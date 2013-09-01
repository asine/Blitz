using System;
using System.ServiceModel;

using Agatha.ServiceLayer;
using Agatha.ServiceLayer.WCF;
using Agatha.Unity;

using Blitz.Common.Agatha;
using Blitz.Common.Core;
using Blitz.Server.Core;

using Microsoft.Practices.Unity;

using Raven.Client;
using Raven.Client.Embedded;

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

            InitialiseRavenDB(container);

            Console.WriteLine("EndPoint - {0}", END_POINT);

            var baseAddress = new Uri(END_POINT);
            Host = new ServiceHost(typeof(WcfRequestProcessor), baseAddress);
            Host.Open();
        }

        public ServiceHost Host { get; private set; }

        private static void InitialiseAgatha(IUnityContainer container)
        {
            new ServiceLayerConfiguration(new Container(container))
                .AddRequestAndResponseAssembly(typeof (Common.AssemblyHook).Assembly)
                .AddRequestHandlerAssembly(typeof (AssemblyHook).Assembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(typeof(Common.AssemblyHook).Assembly);
        }

        private static void InitialiseRavenDB(IUnityContainer container)
        {
            var documentStore = new EmbeddableDocumentStore {DataDirectory = "~/DataDir"};
            documentStore.Initialize();

            container.RegisterSingletonInstance<IDocumentStore>(documentStore);
        }
    }
}