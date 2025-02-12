using Application.Configs.GameDesign;
using Application.Game;
using Domain.Common;
using Domain.Planets;
using UnityEngine;

namespace Domain.Factories
{
    public interface IPlanetFactory
    {
        IPlanetPresenter CreatePresenter(PlanetModel planetModel, Transform parent);
        PlanetSnapshot CreatePlanet(int id, PlanetConfig planetConfig, Position2D position, float radius);
        PlanetSnapshot CreateRandomPlanet(int id, Position2D position, float radius);
        PlanetsSnapshot GenerateNewWorld();
    }
}