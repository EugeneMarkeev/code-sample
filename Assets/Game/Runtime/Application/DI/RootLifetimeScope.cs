using Application.Configs;
using Infrastructure.Configs;
using Infrastructure.Factories;
using Infrastructure.Repository;
using Infrastructure.Time;
using Infrastructure.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Application.DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private LocalConfigsService _localConfigsService;

        [SerializeField]
        private SpritesConfigService _spritesConfigService;

        [SerializeField]
        private UnityCallbacksService _callbacksServicePrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IIocFactory, VContainerFactory>(Lifetime.Singleton);
            builder.Register<RepositoryService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(InstantiateUnityCallbacksService());
            builder.RegisterEntryPoint<LocalTimeService>().As<ITimeService>().AsSelf();

            ConfigureConfigs(builder);
        }

        private UnityCallbacksService InstantiateUnityCallbacksService()
        {
            var service = Instantiate(_callbacksServicePrefab);
            DontDestroyOnLoad(service.gameObject);

            return service;
        }

        private void ConfigureConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance<IConfigsService>(_localConfigsService);
            builder.RegisterInstance<ISpritesConfigService>(_spritesConfigService);
        }
    }
}