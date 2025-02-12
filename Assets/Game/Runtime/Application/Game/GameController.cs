using Application.Resources;
using Infrastructure.Factories;
using Infrastructure.Panels;
using Infrastructure.Repository;
using Presentation.GameplayPanel;
using Presentation.LevelUpPanel;
using Presentation.PlanetPanel;
using Presentation.TopPanel;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Application.Game
{
    public class GameController : IInitializable
    {
        private readonly ISavesController _gameSaveController;
        private readonly IIocFactory _iocFactory;
        private readonly IPanelsService _panelsService;
        private readonly PlanetsController _planetsController;
        private readonly PlayerResourcesController _playerResourcesController;

        [Preserve]
        public GameController(PlayerResourcesController playerResourcesController, ISavesController gameSaveController,
            IIocFactory iocFactory, IPanelsService panelsService, PlanetsController planetsController)
        {
            _planetsController = planetsController;
            _playerResourcesController = playerResourcesController;
            _gameSaveController = gameSaveController;
            _iocFactory = iocFactory;
            _panelsService = panelsService;
        }

        void IInitializable.Initialize()
        {
            _playerResourcesController.Initialize();

            var topBarPanel = _panelsService.Open<TopPanel>();
            topBarPanel.SetPresenter(_iocFactory.Create<TopPanelPresenter>());
            
            var planetPanel = _panelsService.Open<PlanetPanel>();
            planetPanel.SetPresenter(_iocFactory.Create<PlanetPanelPresenter>());
            _panelsService.Close<PlanetPanel>();
            
            var gameplayPanel = _panelsService.Open<GameplayPanel>();
            gameplayPanel.SetPresenter(_iocFactory.Create<GameplayPanelPresenter>());

            _planetsController.Initialize();
            _gameSaveController.SaveAllLocal();
        }
    }
}