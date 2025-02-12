using System;
using Domain.Common;
using Domain.PlayerResources;
using Infrastructure.Configs;
using Infrastructure.Repository;
using UnityEngine.Scripting;

namespace Application.Resources
{
    public class PlayerResourcesController : ISaveable
    {
        private readonly IRepositoryService _repositoryService;

        [Preserve]
        public PlayerResourcesController(IRepositoryService repositoryService, IConfigsService configsService)
        {
            _repositoryService = repositoryService;
        }

        public PlayerResources PlayerResources { get; private set; }

        public void Save()
        {
            _repositoryService.Save(PlayerResources.GetSnapshot());
        }

        public void Initialize()
        {
            PlayerResources = new PlayerResources();

            if (_repositoryService.TryLoad<PlayerResourcesSnapshot>(out var snapshot))
                PlayerResources!.RestoreFromSnapshot(snapshot);
            else
                PlayerResources.Add(new Resource(Constants.Resources.SoftCurrency, 100));
        }

        public bool TryToPurchase(Resource resource, Action<bool> callback = null)
        {
            if (PlayerResources.HasEnough(resource))
            {
                PlayerResources.Remove(resource);
                callback?.Invoke(true);
                return true;
            }

            callback?.Invoke(false);
            return false;
        }

        public void Add(Resource resource)
        {
            PlayerResources.Add(resource);
        }
    }
}