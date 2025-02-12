using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.PlayerResources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Application.Configs.GameDesign
{
    [CreateAssetMenu(fileName = nameof(GameDesignConfigsSO),
        menuName = "Game/Game Design/" + nameof(GameDesignConfigsSO),
        order = 0)]
    public class GameDesignConfigsSO : ScriptableObject
    {
        public GeneralConfig GeneralConfig;
        public PlanetsConfig PlanetsConfig;
    }

    [Serializable]
    public class GeneralConfig
    {
        public int PlanetCount;
        public List<PrefabData> prefabs;

        public GameObject GetPrefabByType(PrefabType prefabType)
        {
            return prefabs.FirstOrDefault(x => x.prefabType == prefabType)?.prefab;
        }
    }

    [Serializable]
    public class PrefabData
    {
        public PrefabType prefabType;
        public GameObject prefab;
    }

    public enum PrefabType
    {
        None = 0,
        Planet = 1
    }

    [Serializable]
    public class PlanetsConfig
    {
        public PlanetConfig[] Planets;

        public PlanetConfig GetRandomPlanetConfig()
        {
            return Planets[Random.Range(0, Planets.Length)];
        }

        public PlanetConfig GetPlanetConfigByIndex(int index)
        {
            return Planets[index];
        }
    }

    [Serializable]
    public class PlanetConfig
    {
        [SerializeField]
        private int _id;

        [SerializeField]
        private string _name;

        [SerializeField]
        private int _incomeTimer;

        [SerializeField]
        private PlanetIconId _iconId;

        [SerializeField]
        private PlanetStateConfig[] _stages;

        public PlanetConfig(int id, string name, int incomeTimer, PlanetIconId iconId, PlanetStateConfig[] stages)
        {
            _id = id;
            _name = name;
            _incomeTimer = incomeTimer;
            _iconId = iconId;
            _stages = stages;
        }

        public int Id => _id;
        public string Name => _name;
        public int IncomeTimer => _incomeTimer;
        public PlanetIconId IconId => _iconId;
        public PlanetStateConfig[] Stages => _stages;
    }

    [Serializable]
    public class PlanetStateConfig
    {
        [SerializeField]
        private Resource _price;

        [SerializeField]
        private int _incomeAmount;

        [SerializeField]
        private int _population;

        public PlanetStateConfig(Resource price, int incomeAmount, int population)
        {
            _price = price;
            _incomeAmount = incomeAmount;
            _population = population;
        }

        public Resource Price => _price;
        public int IncomeAmount => _incomeAmount;
        public int Population => _population;
    }
}