using Application.Game;
using Application.Resources;
using Application.SaveGame;
using Domain.Factories;
using Infrastructure.Factories;
using Infrastructure.Factories.Game;
using Infrastructure.Panels;
using Infrastructure.Repository;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Application.DI
{
    public class MainSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private PanelsService _panelsService;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IIocFactory, VContainerFactory>(Lifetime.Scoped);
            builder.Register<IGameObjectFactory, GameObjectFactory>(Lifetime.Scoped);
            builder.Register<IPlanetFactory, PlanetFactory>(Lifetime.Scoped);

            builder.RegisterEntryPoint<GameSaveController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameController>(Lifetime.Scoped).AsSelf();
            builder.RegisterInstance(InstantiatePanelsService());

            ConfigureDomainControllers(builder);
        }

        private void ConfigureDomainControllers(IContainerBuilder builder)
        {
            builder.Register<PlayerResourcesController>(Lifetime.Scoped).AsSelf().As<ISaveable>();
            builder.Register<PlanetsController>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }

        private IPanelsService InstantiatePanelsService()
        {
            var service = Instantiate(_panelsService);
            DontDestroyOnLoad(service.gameObject);

            return service;
        }
    }
}