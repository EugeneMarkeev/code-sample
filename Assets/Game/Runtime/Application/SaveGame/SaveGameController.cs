using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Repository;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Scripting;

namespace Application.SaveGame
{
    public class GameSaveController : IDisposable, ISavesController
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILocalSavesService _localSavesService;
        private readonly IReadOnlyList<ISaveable> _saveables;
        private readonly UnityCallbacksService _unityCallbacksService;

        [Preserve]
        public GameSaveController(UnityCallbacksService unityCallbacksService, IReadOnlyList<ISaveable> saveables,
            ILocalSavesService localSavesService)
        {
            _unityCallbacksService = unityCallbacksService;
            _saveables = saveables;
            _localSavesService = localSavesService;

            _unityCallbacksService.OnApplicationPaused += OnApplicationPause;
            _unityCallbacksService.OnApplicationQuitted += SaveAllLocal;

            _cancellationTokenSource = new CancellationTokenSource();
            LocalSaveByTime(_cancellationTokenSource.Token).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public void SaveAllLocal()
        {
            Debug.Log("[Saves] Saving all repositories to local storage");

            SaveSavables();
            _localSavesService.SaveAll().Forget();
        }

        private void SaveSavables()
        {
            foreach (var saveable in _saveables) saveable.Save();
        }

        private async UniTaskVoid LocalSaveByTime(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay(5000, cancellationToken: cancellationToken);

                if (cancellationToken.IsCancellationRequested) break;

                SaveAllLocal();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) SaveAllLocal();
        }
    }
}