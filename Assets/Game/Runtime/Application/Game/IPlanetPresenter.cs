using System;
using Domain.Common;
using Domain.Planets;
using Domain.PlayerResources;
using UnityEngine;

namespace Application.Game
{
    public interface IPlanetPresenter : IDisposable
    {
        int Id { get; }
        int State { get; }
        int IncomeTimer { get;}
        int IncomeCountdown { get;}
        int Population { get;}
        int PlanetId { get; }
        int Level { get; }
        float Radius { get; }
        Sprite PlanetSpriteUnlocked { get; }
        Sprite PlanetSpriteLocked { get; }
        Position2D Position { get; }
        PlanetLevel CurrentLevel { get; }
        PlanetLevel NextLevel { get; }

        void TryToBuyNextLevel();
        void PurchaseResultReceived(bool isSuccess);
        void GenerateIncome();
        void TakeIncome();

        public event Action<int> OnTryToPurchase;
        public event Action<Resource> OnTakenIncome;
        public event Action OnUpdatedStaticData;
        public event Action OnLevelChanged;
        public event Action OnSwitchedState;
        public event Action OnTickedIncomeTimer;
    }
}