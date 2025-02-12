using System;
using Domain.Common;

namespace Domain.Planets
{
    [Serializable]
    public struct PlanetSnapshot
    {
        public int Id;
        public int PlanetId;
        public int Level;
        public int State;
        public int IncomeCountdown;
        public int EarnedIncome;
        public Position2D Position;
        public PlanetIconId IconId;
        public float Radius;
    }
}