using System;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.Game
{
    public class GameObjectFactory : IGameObjectFactory
    {
        [Preserve]
        public GameObjectFactory()
        {
        }

        public T Create<T>(GameObject prefab, Transform parent = null) where T : Component
        {
            var instance = Create(prefab, typeof(T), parent);
            var component = instance.GetComponent<T>();
            if (component == null)
                throw new Exception($"The created object does not contain a {typeof(T).Name} component.");

            return component;
        }

        public GameObject Create(GameObject prefab, Type type, Transform parent = null)
        {
            var instance = Object.Instantiate(prefab, parent);
            return instance;
        }
    }
}