using Autofac;

namespace RX_MVVM
{
    public class Bootstrapper
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PropertyProviderFactory>().As<IPropertyProviderFactory>().SingleInstance();
            containerBuilder.RegisterType<Schedulers>().As<ISchedulers>().SingleInstance();
            containerBuilder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
        }
    }
}
