using System;
using Application.Game;
using Domain.PlayerResources;
using UnityEngine;
using VContainer;

namespace Presentation.LevelUpPanel
{
    public class PlanetPanelPresenter : IPlanetPanelPresenter
    {
        private PlanetsController _planetsController;

        public int Id { get; private set; }
        public int PlanetId { get; private set; }
        public int Level { get; private set; }
        public int Income { get; private set; }
        public int Population { get; private set; }
        public Sprite Sprite { get; private set; }
        public Resource NextUpgradePrice { get; private set; }

        public event Action OnUpdatedData;
        
        [Preserve]
        public PlanetPanelPresenter(PlanetsController planetsController)
        {
            _planetsController = planetsController;
        }

        public void Dispose()
        {
            OnUpdatedData = null;
        }

        public void SetupPlanet(int id)
        {
            Id = id;
            UpdatePlanetData();
        }

        public void Purchase()
        {
            _planetsController.PurchasePlanet(Id);
            _planetsController.TryToPurchasePlanet(Id);
        }

        private void UpdatePlanetData()
        {
            Debug.Log("UpdatePlanetData");
            var planetData = _planetsController.GetPlanetData(Id);
            PlanetId = planetData.PlanetId;
            Income = planetData.Income;
            NextUpgradePrice = planetData.NextUpgradePrice;
            Level = planetData.Level;
            Population = planetData.Population;
            Sprite = planetData.Sprite;
            
            OnUpdatedData?.Invoke();
        }
    }
}