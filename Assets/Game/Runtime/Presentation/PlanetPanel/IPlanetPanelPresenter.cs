using System;
using Domain.PlayerResources;
using UnityEngine;

namespace Presentation.LevelUpPanel
{
    public interface IPlanetPanelPresenter:IDisposable
    {
        int Id { get;}
        int PlanetId { get;}
        int Level { get;}
        int Income { get;}
        int Population { get;}
        Sprite Sprite { get;}
        Resource NextUpgradePrice { get;}
        
        public event Action OnUpdatedData;
        
        void SetupPlanet(int id);
        void Purchase();
    }
}