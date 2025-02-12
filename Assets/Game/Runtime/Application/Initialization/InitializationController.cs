using System;
using Application.Initialization.LoadingTasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Factories;
using Infrastructure.LoadingTasks;
using Infrastructure.Repository;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Application.Initialization
{
    public class InitializationController : IInitializable
    {
        private const float LoaderDelaySeconds = 2f;

        private readonly IIocFactory _iocFactory;
        private readonly ILocalSavesService _localSavesService;

        [Preserve]
        public InitializationController(IIocFactory iocFactory, ILocalSavesService localSavesService)
        {
            Debug.Log("Initialization Controller");
            _iocFactory = iocFactory;
            _localSavesService = localSavesService;
        }

        void IInitializable.Initialize()
        {
            UnityEngine.Application.targetFrameRate = 60;
            DOTween.SetTweensCapacity(500, 250);

            StartInitialization().Forget();
        }

        private async UniTaskVoid StartInitialization()
        {
            var initializationSequence = CreateInitializationSequence();
            var waitSequence = new LoadingTaskSequence(
                new DelayTask(TimeSpan.FromSeconds(LoaderDelaySeconds)));

            await UniTask.WhenAll(waitSequence.LoadAsync(), initializationSequence.LoadAsync());

            await _iocFactory.Create<LoadSceneTask, string>("MainScene").LoadAsync();
        }

        private ILoadingTask CreateInitializationSequence()
        {
            return new LoadingTaskSequence(
                _iocFactory.Create<InitializeConfigsServiceTask>(),
                new DelegateTask(_localSavesService.LoadAll),
                _iocFactory.Create<InitializeTimeServiceTask>());
        }
    }
}