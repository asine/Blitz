using Autofac;

using Blitz.Client.Customer.Report;
using Blitz.Client.Customer.ReportLayout;
using Blitz.Client.Customer.ReportParameters;
using Blitz.Client.Customer.ReportRunner;
using Blitz.Client.Customer.Reportviewer;

namespace Blitz.Client.Customer
{
    public class CustomerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReportViewModel>().AsSelf().InstancePerDependency();

            builder.RegisterType<ReportParameterViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportParameterStepViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportParameterStepService>().As<IReportParameterStepService>().InstancePerDependency();

            builder.RegisterType<ReportLayoutViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportLayoutService>().As<IReportLayoutService>().InstancePerDependency();
            builder.RegisterType<ReportLayoutItemViewModel>().AsSelf().InstancePerDependency();

            builder.RegisterType<ReportRunnerViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportRunnerService>().As<IReportRunnerService>().InstancePerDependency();

            builder.RegisterType<ReportViewerViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportViewerService>().As<IReportViewerService>().InstancePerDependency();
        }
    }
}