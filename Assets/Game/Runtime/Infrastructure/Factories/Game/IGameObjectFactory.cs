using System;
using UnityEngine;

namespace Infrastructure.Factories.Game
{
    public interface IGameObjectFactory
    {
        T Create<T>(GameObject prefab, Transform parent = null) where T : Component;
        GameObject Create(GameObject prefab, Type type, Transform parent = null);
    }
}