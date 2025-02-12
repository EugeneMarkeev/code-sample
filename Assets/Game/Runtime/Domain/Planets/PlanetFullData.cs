using Domain.PlayerResources;
using UnityEngine;

namespace Domain.Planets
{
    public struct PlanetFullData
    {
        public int Id;
        public int PlanetId;
        public int Level;
        public int Income;
        public int Population;
        public Sprite Sprite;
        public Resource NextUpgradePrice;
    }
}