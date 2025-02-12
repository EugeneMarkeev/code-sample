using System;
using Application.Configs.GameDesign;
using Domain.Common;
using Domain.Planets;
using Domain.PlayerResources;
using Infrastructure.Configs;
using Presentation;
using UnityEngine;
using VContainer;

namespace Application.Game
{
    public class PlanetPresenter : IPlanetPresenter
    {
        private readonly ISpritesConfigService _spritesConfigService;
        private PlanetConfig _config;
        private PlanetModel _model;
        private PlanetView _view;
        
        public int Id { get; private set; }
        public int State { get; private set; }
        public int Level { get; private set; }
        public int IncomeTimer { get; private set; }
        public int IncomeCountdown { get; private set; }
        public int Population { get; private set; }
        public int PlanetId { get; private set; }
        public float Radius { get; private set; }
        public Position2D Position { get; private set; }
        public PlanetLevel CurrentLevel { get; private set; }
        public PlanetLevel NextLevel { get; private set; }
        public Sprite PlanetSpriteUnlocked { get; private set; }
        public Sprite PlanetSpriteLocked { get; private set; }
        
        public event Action<int> OnTryToPurchase;
        public event Action<Resource> OnTakenIncome;
        public event Action OnUpdatedStaticData;
        public event Action OnLevelChanged;
        public event Action OnSwitchedState;
        public event Action OnTickedIncomeTimer;
        
        [Preserve]
        public PlanetPresenter(ISpritesConfigService spritesConfigService)
        {
            _spritesConfigService = spritesConfigService;
        }
        
        public void Dispose()
        {
            _model.OnUpdatedStaticData -= UpdatedStaticData;
            _model.OnChangedLevel -= ChangedLevel;
            _model.OnSwitchedState -= SwitchedState;
            _model.OnTakenEarnedIncome -= TakenEarnedIncome;
            _model.OnTickedIncomeTimer -= TickedIncomeTimer;
        }

        public void TryToBuyNextLevel()
        {
            OnTryToPurchase?.Invoke(Id);
        }

        public void GenerateIncome()
        {
            _model.GenerateIncome();
        }

        public void TakeIncome()
        {
            _model.TakeIncome();
        }

        public void PurchaseResultReceived(bool isSuccess)
        {
            Debug.Log($"Purchase result: {isSuccess}");
            if (!isSuccess)
                return;
            
            _model.SetNextLevel();
        }

        public void Initialize(PlanetView planetView, PlanetModel planetModel)
        {
            _view = planetView;
            _model = planetModel;
            _view.SetPresenter(this);

            _model.OnUpdatedStaticData += UpdatedStaticData;
            _model.OnChangedLevel += ChangedLevel;
            _model.OnSwitchedState += SwitchedState;
            _model.OnTakenEarnedIncome += TakenEarnedIncome;
            _model.OnTickedIncomeTimer += TickedIncomeTimer;

            _model.Initialization();
        }

        private void TickedIncomeTimer()
        {
            IncomeCountdown = _model.IncomeCountdown;
            IncomeTimer = _model.IncomeTimer;

            OnTickedIncomeTimer?.Invoke();
        }

        private void TakenEarnedIncome(Resource resource)
        {
            OnTakenIncome?.Invoke(resource);
        }

        private void SwitchedState()
        {
            State = _model.State;
            OnSwitchedState?.Invoke();
        }

        private void ChangedLevel()
        {
            Level = _model.Level;
            CurrentLevel = _model.CurrentLevel;
            NextLevel = _model.NextLevel;
            Population = CurrentLevel.Population;

            OnLevelChanged?.Invoke();
        }


        private void UpdatedStaticData()
        {
            Id = _model.Id;
            Position = _model.Position;
            Radius = _model.Radius;
            PlanetId = _model.PlanetId;

            PlanetSpriteUnlocked = _spritesConfigService.GetSprite(_model.IconId + Constants.Common.UnlockedPostfix);
            PlanetSpriteLocked = _spritesConfigService.GetSprite(_model.IconId + Constants.Common.LockedPostfix);

            OnUpdatedStaticData?.Invoke();
        }
    }
}