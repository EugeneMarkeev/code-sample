using System.Collections.Generic;
using System.Linq;
using Application.Configs.GameDesign;
using Application.Game;
using Application.Utils;
using Domain.Common;
using Domain.Planets;
using Domain.PlayerResources;
using Infrastructure.Configs;
using Infrastructure.Factories;
using Infrastructure.Factories.Game;
using Presentation;
using UnityEngine;
using VContainer;

namespace Domain.Factories
{
    public class PlanetFactory : IPlanetFactory
    {
        private readonly IConfigsService _configsService;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly GeneralConfig _generalConfig;
        private readonly IIocFactory _iocFactory;
        private readonly PlanetsConfig _planetsConfig;

        [Preserve]
        public PlanetFactory(IIocFactory iocFactory, IGameObjectFactory gameObjectFactory,
            IConfigsService configsService)
        {
            _configsService = configsService;
            _gameObjectFactory = gameObjectFactory;
            _iocFactory = iocFactory;

            _generalConfig = _configsService.Get<GeneralConfig>();
            _planetsConfig = _configsService.Get<PlanetsConfig>();
        }

        public PlanetsSnapshot GenerateNewWorld()
        {
            var allPlanets = new List<PlanetSnapshot>();
            var usedPositions = new List<Vector2>();

            for (var i = 0; i < _generalConfig.PlanetCount; i++)
            {
                var radiusRelative = 0.11f;
                var positionRelative = PlanetPositionFinder.FindValidRelativePosition(usedPositions, radiusRelative);

                if (i == 0)
                    positionRelative = Vector2.one * 0.5f;

                usedPositions.Add(positionRelative);
                allPlanets.Add(CreateRandomPlanet(i, positionRelative.ToPosition2D(), 100));
            }

            return new PlanetsSnapshot { AllPlanets = allPlanets };
        }

        public PlanetSnapshot CreateRandomPlanet(int id, Position2D position, float radius)
        {
            return CreatePlanet(id, _planetsConfig.GetRandomPlanetConfig(), position, radius);
        }

        public PlanetSnapshot CreatePlanet(int id, PlanetConfig planetConfig, Position2D position, float radius)
        {
            var planetSnapshot = new PlanetSnapshot
            {
                Id = id,
                PlanetId = planetConfig.Id,
                Level = 0,
                Position = position,
                IconId = planetConfig.IconId,
                Radius = radius,
                IncomeCountdown = planetConfig.IncomeTimer,
                EarnedIncome = 0
            };

            return planetSnapshot;
        }

        public IPlanetPresenter CreatePresenter(PlanetModel planetModel, Transform parent)
        {
            var planetPresenter = _iocFactory.Create<PlanetPresenter>();
            var planetView = _gameObjectFactory.Create<PlanetView>(_generalConfig.GetPrefabByType(PrefabType.Planet),
                parent);

            var planetConfig = _planetsConfig.GetPlanetConfigByIndex(planetModel.PlanetId);

            var states = planetConfig.Stages.Select(stateData =>
                new PlanetLevel(new Resource(stateData.Price.Id, stateData.Price.Count), stateData.IncomeAmount,
                    stateData.Population)
            ).ToArray();

            planetModel.Setup(states, planetConfig.IncomeTimer);

            planetPresenter.Initialize(planetView, planetModel);
            return planetPresenter;
        }
    }
}