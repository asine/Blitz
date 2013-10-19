using System;
using System.ServiceModel;

using Agatha.ServiceLayer.WCF;

using Microsoft.Practices.Unity;

using Naru.Agatha;
using Naru.Log4Net;
using Naru.Unity;

using Raven.Client;
using Raven.Client.Embedded;

namespace Blitz.Server
{
    public class Bootstrapper
    {
        private const string END_POINT = "http://localhost:1234/Agatha";

        public ServiceHost Host { get; private set; }

        public Bootstrapper()
        {
            var container = new UnityContainer();

            container
                .ConfigureNaruLog4Net("ILogInject.UnityCommonLogging.Blitz.Server")
                .ConfigureNaruAgathaServer(typeof(Common.AssemblyHook).Assembly, typeof(AssemblyHook).Assembly);

            InitialiseRavenDB(container);

            Console.WriteLine("EndPoint - {0}", END_POINT);

            var baseAddress = new Uri(END_POINT);
            Host = new ServiceHost(typeof(WcfRequestProcessor), baseAddress);
            Host.Open();
        }

        private static void InitialiseRavenDB(IUnityContainer container)
        {
            var documentStore = new EmbeddableDocumentStore {DataDirectory = "~/DataDir"};
            documentStore.Initialize();

            container.RegisterSingletonInstance<IDocumentStore>(documentStore);
        }
    }
}