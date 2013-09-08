using System;
using System.IO;
using System.ServiceModel;

using Agatha.ServiceLayer;
using Agatha.ServiceLayer.WCF;
using Agatha.Unity;

using Blitz.Common.Agatha;
using Blitz.Common.Core;
using Blitz.Server.Core;

using Common.Logging;

using ILogInject.Unity;

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

            // Configure Logging
            container
                .AddNewExtension<BuildTracking>()
                .AddNewExtension<CommonLoggingLogCreationExtension>()
                .RegisterInstance<ILog4NetConfiguration>(new Log4NetConfiguration("ILogInject.UnityCommonLogging.Blitz.Server"));

            InitialiseAgatha(container);

            ConfigureLog4Net(container);

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

        private static void ConfigureLog4Net(IUnityContainer container)
        {
            var configuration = container.Resolve<ILog4NetConfiguration>();
            if (!Directory.Exists(configuration.LogDirectoryPath))
            {
                Directory.CreateDirectory(configuration.LogDirectoryPath);
            }
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(configuration.LogDirectoryPath, configuration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
        }
    }
}