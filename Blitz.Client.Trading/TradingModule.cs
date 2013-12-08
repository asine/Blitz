using Autofac;

using Blitz.Client.Trading.Quote.Blotter;
using Blitz.Client.Trading.Quote.Edit;
using Blitz.Client.Trading.Security.Chart;

namespace Blitz.Client.Trading
{
    public class TradingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuoteModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<QuoteBlotterViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<QuoteBlotterService>().As<IQuoteBlotterService>().InstancePerDependency();

            builder.RegisterType<QuoteEditViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<QuoteEditService>().As<IQuoteEditService>().InstancePerDependency();

            builder.RegisterType<ChartViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ChartService>().As<IChartService>().InstancePerDependency();
        }
    }
}