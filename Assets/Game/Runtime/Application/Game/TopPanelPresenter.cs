using System;
using Application.Resources;
using Domain.Common;
using Domain.PlayerResources;
using Infrastructure.Configs;
using Presentation.TopPanel;
using UnityEngine;
using VContainer;

namespace Application.Game
{
    public class TopPanelPresenter : ITopPanelPresenter
    {
        private readonly PlayerResourcesController _playerResourcesController;
        
        public ulong SoftCurrencyCount =>
            _playerResourcesController.PlayerResources.GetCount(Constants.Resources.SoftCurrency);

        public Sprite SoftCurrencySprite { get; }
        public event Action OnSoftCurrencyChanged;

        [Preserve]
        public TopPanelPresenter(PlayerResourcesController playerResourcesController, IConfigsService configsService,
            ISpritesConfigService spritesConfigService)
        {
            _playerResourcesController = playerResourcesController;

            SoftCurrencySprite = spritesConfigService.GetSprite(configsService.Get<ResourcesConfigs>()
                .GetResourceConfig(Constants.Resources.SoftCurrency).IconName);

            _playerResourcesController.PlayerResources.ResourceCountAdded += OnResourceCountChanged;
            _playerResourcesController.PlayerResources.ResourceCountRemoved += OnResourceCountChanged;
        }
        
        public void Dispose()
        {
            _playerResourcesController.PlayerResources.ResourceCountAdded -= OnResourceCountChanged;
            _playerResourcesController.PlayerResources.ResourceCountRemoved -= OnResourceCountChanged;
        }

        private void OnResourceCountChanged(string resourceId, ulong changedCount, ulong totalCount)
        {
            if (resourceId == Constants.Resources.SoftCurrency) OnSoftCurrencyChanged?.Invoke();
        }
    }
}