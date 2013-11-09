using Autofac;

using Blitz.Client.Employee.Report;
using Blitz.Client.Employee.ReportParameters;
using Blitz.Client.Employee.ReportRunner;

namespace Blitz.Client.Employee
{
    public class EmployeeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReportViewModel>().AsSelf().InstancePerDependency();

            builder.RegisterType<ReportParameterViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportParameterStepViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportParameterStepService>().As<IReportParameterStepService>().InstancePerDependency();

            builder.RegisterType<ReportRunnerViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ReportRunnerService>().As<IReportRunnerService>().InstancePerDependency();
        }
    }
}