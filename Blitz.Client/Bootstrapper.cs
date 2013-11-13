using Autofac;

using Blitz.Client.Common.DynamicColumnEdit;
using Blitz.Client.Common.DynamicColumnManagement;
using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Common.ExportToExcel;
using Blitz.Client.Common.Report;
using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core.EPPlus;
using Blitz.Client.Customer;
using Blitz.Client.Employee;
using Blitz.Client.Settings.Appearance;
using Blitz.Client.Trading;

using Naru.Agatha;
using Naru.Aufofac.log4Net;
using Naru.Core;
using Naru.Log4Net;
using Naru.WPF;

using Blitz.Client.Shell;

namespace Blitz.Client
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            IContainer container = null;

            var builder = new ContainerBuilder();

            builder.RegisterModule(new AgathaClientModule
            {
                ContainerFactory = () => container,
                RequestResponseAssembly = typeof(Blitz.Common.AssemblyHook).Assembly
            });

            builder.RegisterModule(new LogInjectionModule());
            builder.RegisterModule(new Log4NetModule { SectionName = "CommonLogging.Blitz.Client" });

            builder.RegisterModule(new CoreModule());

            builder.RegisterModule(new WPFModule());
            builder.RegisterType<WPFStartable>().AsSelf();

            builder.RegisterType<EventStream>().As<IEventStream>().InstancePerOwned<ReportViewModel>().SingleInstance();

            builder.RegisterType<ShellViewModel>().AsSelf();
            builder.RegisterType<AppearanceViewModel>().AsSelf();

            builder.RegisterType<DynamicReportDataViewModel>().AsSelf();
            builder.RegisterType<DynamicColumnManagementViewModel>().AsSelf();
            builder.RegisterType<DynamicColumnManagementService>().As<IDynamicColumnManagementService>().InstancePerDependency();

            builder.RegisterType<DynamicColumnEditViewModel>().AsSelf();
            builder.RegisterType<DynamicColumnEditService>().As<IDynamicColumnEditService>().InstancePerDependency();

            builder.RegisterType<BasicExportToExcel>().As<IBasicExportToExcel>().InstancePerDependency();
            builder.RegisterType<ExcelPackageWriter>().AsSelf();
            builder.RegisterType<ExcelWorkSheetWriter>().AsSelf();
            builder.RegisterType<ReflectionDataWriter>().AsSelf();
            builder.RegisterType<HistoryViewModel>().AsSelf();

            builder.RegisterType<ClientStartable>().AsSelf();

            // Trading
            builder.RegisterModule(new TradingModule());
            builder.RegisterType<TradingStartable>().AsSelf();

            // Employee
            builder.RegisterModule(new EmployeeModule());
            builder.RegisterType<EmployeeStartable>().AsSelf();

            // Customer
            builder.RegisterModule(new CustomerModule());
            builder.RegisterType<CustomerStartable>().AsSelf();

            // Build the container
            container = builder.Build();

            // TODO : Ideally change this, so it isn't resolved like this
            container.Resolve<WPFStartable>().Start();
            container.Resolve<ClientStartable>().Start();
            container.Resolve<TradingStartable>().Start();
            container.Resolve<EmployeeStartable>().Start();
            container.Resolve<CustomerStartable>().Start();
        }
    }
}