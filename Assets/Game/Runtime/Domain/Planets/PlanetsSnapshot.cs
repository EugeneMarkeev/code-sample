using System;
using System.Collections.Generic;

namespace Domain.Planets
{
    [Serializable]
    public struct PlanetsSnapshot
    {
        public List<PlanetSnapshot> AllPlanets;
    }
}