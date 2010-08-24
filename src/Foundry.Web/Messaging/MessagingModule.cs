using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Foundry.Messaging.Infrastructure;
using Foundry.Messaging.MessageHandlers;

namespace Foundry.Messaging
{
    public class MessagingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new InProcessBus(c)).As<IBus>().HttpRequestScoped();

            builder.RegisterAssemblyTypes(typeof(CreateUserMessageHandler).Assembly)
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMessageHandler<>)))
                .AsClosedTypesOf(typeof(IMessageHandler<>));
        }
    }
}