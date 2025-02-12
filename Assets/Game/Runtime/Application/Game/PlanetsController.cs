using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Configs.GameDesign;
using Application.Resources;
using Cysharp.Threading.Tasks;
using Domain.Factories;
using Domain.Planets;
using Domain.PlayerResources;
using Infrastructure.Configs;
using Infrastructure.Factories;
using Infrastructure.Factories.Game;
using Infrastructure.Panels;
using Infrastructure.Repository;
using Presentation.GameplayPanel;
using Presentation.PlanetPanel;
using UnityEngine;
using UnityEngine.Scripting;

namespace Application.Game
{
    public class PlanetsController : ISaveable, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConfigsService _configsService;
        private readonly IPlanetFactory _planetFactory;
        private readonly PlayerResourcesController _playerResourcesController;
        private readonly IRepositoryService _repositoryService;
        private readonly List<IPlanetPresenter> _planetPresenters = new();
        private readonly IPanelsService _panelsService;

        private GameplayPanel _gameplayPanel;

        public Planets Planets { get; private set; }
        
        [Preserve]
        public PlanetsController(IRepositoryService repositoryService, IConfigsService configsService,
            IIocFactory iocFactory, IGameObjectFactory gameObjectFactory, IPlanetFactory planetFactory,
            PlayerResourcesController playerResourcesController, IPanelsService panelsService)
        {
            _panelsService = panelsService;
            _playerResourcesController = playerResourcesController;
            _planetFactory = planetFactory;
            _configsService = configsService;
            _repositoryService = repositoryService;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Save()
        {
            _repositoryService.Save(Planets.GetSnapshot());
        }

        public void Initialize()
        {
            Planets = new Planets();
            Planets.PlanetAdded += OnPlanetAdded;
            
            _panelsService.IsOpened(out _gameplayPanel);
            
            if (_repositoryService.TryLoad<PlanetsSnapshot>(out var snapshot))
            {
                Debug.Log($"Snapshot to restore {JsonUtility.ToJson(snapshot)}");
                Planets!.RestoreFromSnapshot(snapshot);
            }
            else
            {
                Debug.Log("No PlanetsSnapshot found");
                Planets!.RestoreFromSnapshot(_planetFactory.GenerateNewWorld());
            }

            IncomeCycle(_cancellationTokenSource.Token).Forget();
        }

        public PlanetFullData GetPlanetData(int id)
        {
            var planetPresenter = _planetPresenters.First(x => x.Id == id);
            return new PlanetFullData()
            {
                Id = id, PlanetId = planetPresenter.PlanetId, Income = planetPresenter.CurrentLevel.IncomeAmount,
                Level = planetPresenter.Level,
                NextUpgradePrice = planetPresenter.NextLevel?.Price ?? new Resource(),
                Population = planetPresenter.Population,
                Sprite = planetPresenter.PlanetSpriteUnlocked
            };
        }

        private async UniTaskVoid IncomeCycle(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay(1000, cancellationToken: cancellationToken);

                if (cancellationToken.IsCancellationRequested) break;

                IssueTick();
            }
        }

        private void IssueTick()
        {
            foreach (var planet in _planetPresenters)
            {
                planet.GenerateIncome();
            }
        }

        private void OnPlanetAdded(PlanetModel planetModel)
        {
            var planetPresenter = _planetFactory.CreatePresenter(planetModel, _gameplayPanel.GetSpawnZone());
            planetPresenter.OnTryToPurchase += TryToPurchasePlanet;
            planetPresenter.OnTakenIncome += TakenIncome;

            _planetPresenters.Add(planetPresenter);
        }

        private void TakenIncome(Resource income)
        {
            Debug.Log($"Take Income {JsonUtility.ToJson(income)}");
            _playerResourcesController.Add(income);
        }

        public void TryToPurchasePlanet(int id)
        {
            var planetPresenter = _planetPresenters.First(x => x.Id == id);

            if (planetPresenter.State == 0)
            {
                PurchasePlanet(id);
            }
            else
            {
                var isOpened = _panelsService.IsOpened<PlanetPanel>(out var planetPanel);
                if (!isOpened)
                {
                    planetPanel.Show();
                }

                planetPanel.SetupPlanet(id);
            }
        }

        public void PurchasePlanet(int id)
        {
            var planetPresenter = _planetPresenters.First(x => x.Id == id);
            _playerResourcesController.TryToPurchase(planetPresenter.NextLevel.Price,
                planetPresenter.PurchaseResultReceived);
        }
    }
}