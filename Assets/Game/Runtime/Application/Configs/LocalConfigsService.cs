using System;
using System.Collections.Generic;
using Application.Configs.GameDesign;
using Application.Configs.LocalSOConfigs;
using Cysharp.Threading.Tasks;
using EditorAttributes;
using Infrastructure.Configs;
using UnityEngine;

namespace Application.Configs
{
    [CreateAssetMenu(fileName = nameof(LocalConfigsService), menuName = "Game/" + nameof(LocalConfigsService),
        order = 0)]
    public class LocalConfigsService : ScriptableObject, IConfigsService
    {
        [SerializeField]
        private ResourcesConfigsSO _resourcesConfigsSO;

        [SerializeField]
        private GameDesignConfigsSO _gameDesignConfigsConfigsSO;

        private readonly Dictionary<Type, object> _cachedConfigs = new();

        public UniTask Initialize()
        {
            InitializeInternal();

            return UniTask.CompletedTask;
        }

        public T Get<T>()
        {
            return (T)_cachedConfigs[typeof(T)];
        }

#if UNITY_EDITOR
        [Button]
        public void InitializeEditor()
        {
            InitializeInternal();
        }
#endif

        private void InitializeInternal()
        {
            _cachedConfigs.Clear();

            AddToCache(_resourcesConfigsSO.ResourcesConfigs);
            AddToCache(_gameDesignConfigsConfigsSO.PlanetsConfig);
            AddToCache(_gameDesignConfigsConfigsSO.GeneralConfig);
        }

        private void AddToCache<T>(T config)
        {
            _cachedConfigs.Add(config.GetType(), config);
        }
    }
}