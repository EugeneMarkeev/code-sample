using System;
using System.Collections.Generic;
using Domain.Common;
using VContainer;

namespace Domain.Planets
{
    public class Planets : ISnapshotable<PlanetsSnapshot>
    {
        private readonly List<PlanetModel> _planetsModels = new();

        [Preserve]
        public Planets()
        {
        }

        public PlanetsSnapshot GetSnapshot()
        {
            var planetsSnapshots = new List<PlanetSnapshot>();
            foreach (var planetModel in _planetsModels) planetsSnapshots.Add(planetModel.GetSnapshot());

            return new PlanetsSnapshot
            {
                AllPlanets = planetsSnapshots
            };
        }

        public void RestoreFromSnapshot(PlanetsSnapshot snapshot)
        {
            foreach (var planetSnapshot in snapshot.AllPlanets)
            {
                var planetModel = new PlanetModel();
                planetModel.RestoreFromSnapshot(planetSnapshot);
                Add(planetModel);
            }
        }

        public event Action<PlanetModel> PlanetAdded;

        public void Add(PlanetModel planet)
        {
            _planetsModels.Add(planet);
            PlanetAdded?.Invoke(planet);
        }
    }
}