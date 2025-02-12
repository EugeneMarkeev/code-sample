using System;
using Domain.Common;
using Domain.PlayerResources;
using UnityEngine;
using VContainer;

namespace Domain.Planets
{
    public class PlanetModel : ISnapshotable<PlanetSnapshot>
    {
        private PlanetLevel[] _levels;
        private string _name;

        public int Id { get; private set; }
        public int State { get; private set; }
        public int Level { get; private set; }
        public int PlanetId { get; private set; }
        public int IncomeCountdown { get; private set; }
        public int IncomeTimer { get; private set; }
        public int EarnedIncome { get; private set; }
        public float Radius { get; private set; }
        public Position2D Position { get; private set; }
        public PlanetIconId IconId { get; private set; }
        public PlanetLevel CurrentLevel { get; private set; }
        public PlanetLevel NextLevel { get; private set; }
        
        public event Action OnUpdatedStaticData;
        public event Action OnChangedLevel;
        public event Action OnSwitchedState;
        public event Action OnTickedIncomeTimer;
        public event Action<Resource> OnTakenEarnedIncome;
        
        [Preserve]
        public PlanetModel()
        {
        }
        
        public PlanetSnapshot GetSnapshot()
        {
            return new PlanetSnapshot
            {
                Id = Id,
                Level = Level,
                State = State,
                Position = Position,
                IconId = IconId,
                PlanetId = PlanetId,
                Radius = Radius,
                IncomeCountdown = IncomeCountdown,
                EarnedIncome = EarnedIncome
            };
        }

        public void RestoreFromSnapshot(PlanetSnapshot snapshot)
        {
            Id = snapshot.Id;
            Level = snapshot.Level;
            State = snapshot.State;
            Position = snapshot.Position;
            IconId = snapshot.IconId;
            PlanetId = snapshot.PlanetId;
            Radius = snapshot.Radius;
            IncomeCountdown = snapshot.IncomeCountdown;
            EarnedIncome = snapshot.EarnedIncome;
        }
        
        public void Initialization()
        {
            SelLevel(Level);
            
            OnUpdatedStaticData?.Invoke();
            OnTickedIncomeTimer?.Invoke();
        }

        public void Setup(PlanetLevel[] levels, int incomeTimer)
        {
            IncomeTimer = incomeTimer;
            _levels = levels;
        }

        public void SetNextLevel()
        {
            if (Level >= _levels.Length)
                return;

            SelLevel(Level + 1);
        }

        public void TakeIncome()
        {
            OnTakenEarnedIncome?.Invoke(new Resource(Constants.Resources.SoftCurrency, (ulong)EarnedIncome));

            EarnedIncome = 0;
            
            UpdateState();
        }

        public void GenerateIncome()
        {
            if (State <= 0)
                return;

            IncomeCountdown--;
            if (IncomeCountdown <= 0)
            {
                IncomeCountdown = IncomeTimer;
                EarnedIncome += CurrentLevel.IncomeAmount;

                UpdateState();
            }
            
            OnTickedIncomeTimer?.Invoke();
        }

        private void SelLevel(int level)
        {
            if (level < 0 || level >= _levels.Length)
                return;

            Level = level;

            CurrentLevel = _levels[Level];
            NextLevel = Level < _levels.Length - 1 ? _levels[Level + 1] : null;

            OnChangedLevel?.Invoke();

            UpdateState();
        }

        // 0 - Not purchased, 1 - Not ready, 2 - Ready
        private void UpdateState()
        {
            if (Level == 0)
            {
                SetState(0, true);
            }
            else if (EarnedIncome > 0)
            {
                SetState(2);
            }
            else
            {
                SetState(1);
            }
        }

        private void SetState(int state, bool force = false)
        {
            State = state;
            OnSwitchedState?.Invoke();
        }
    }
}