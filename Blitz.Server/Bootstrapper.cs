using System;
using System.ServiceModel;

using Agatha.ServiceLayer.WCF;

using Autofac;

using Naru.Agatha;
using Naru.Aufofac.log4Net;
using Naru.Log4Net;

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
            Naru.Core.UnhandledExceptionHandler.InstallDomainUnhandledException();
            Naru.TPL.UnhandledExceptionHandler.InstallTaskUnobservedException();

            IContainer container = null;

            var builder = new ContainerBuilder();

            builder.RegisterModule(new LogInjectionModule());
            builder.RegisterModule(new Log4NetModule
            {
                SectionName = "CommonLogging.Blitz.Server"
            });

            builder.RegisterModule(new AgathaServerModule
            {
                ContainerFactory = () => container,
                HandlerAssembly = typeof(AssemblyHook).Assembly,
                RequestResponseAssembly = typeof(Common.AssemblyHook).Assembly
            });

            InitialiseRavenDB(builder);

            container = builder.Build();

            Console.WriteLine("EndPoint - {0}", END_POINT);

            var baseAddress = new Uri(END_POINT);
            Host = new ServiceHost(typeof(WcfRequestProcessor), baseAddress);
            Host.Open();
        }

        private static void InitialiseRavenDB(ContainerBuilder builder)
        {
            var documentStore = new EmbeddableDocumentStore {DataDirectory = "~/DataDir"};
            documentStore.Initialize();

            builder.RegisterInstance(documentStore).As<IDocumentStore>();
        }
    }
}