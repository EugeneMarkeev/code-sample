using Application.Initialization;
using Infrastructure.Factories;
using VContainer;
using VContainer.Unity;

namespace Application.DI
{
    public class InitSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IIocFactory, VContainerFactory>(Lifetime.Scoped);
            builder.RegisterEntryPoint<InitializationController>();
        }
    }
}