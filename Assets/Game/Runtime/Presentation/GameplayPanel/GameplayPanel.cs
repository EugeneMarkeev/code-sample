using Infrastructure.Panels;
using UnityEngine;

namespace Presentation.GameplayPanel
{
    public class GameplayPanel : PanelBase
    {
        [SerializeField]
        private RectTransform _spawnZone;

        private IGameplayPanelPresenter _presenter;

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }

        public void SetPresenter(IGameplayPanelPresenter presenter)
        {
            _presenter = presenter;
        }

        public RectTransform GetSpawnZone()
        {
            return _spawnZone;
        }
    }
}