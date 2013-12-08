using Autofac;

using Blitz.Client.CRM.Client.Edit;

namespace Blitz.Client.CRM
{
    public class CRMModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ClientEditViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ClientEditService>().As<IClientEditService>().InstancePerDependency();
        }
    }
}