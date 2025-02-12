using Domain.Planets;
using Domain.PlayerResources;
using Infrastructure.Panels;
using Presentation.LevelUpPanel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Presentation.PlanetPanel
{
    public class PlanetPanel : PanelBase
    {
        [Header("Top bar")]
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private TMP_Text _planetNameText;

        [Header("Purchase button")]
        [SerializeField]
        private Button _purchaseButton;

        [SerializeField]
        private TMP_Text _priceText;

        [Header("Details")]
        [SerializeField]
        private TMP_Text _populationText;

        [SerializeField]
        private TMP_Text _incomeText;

        [SerializeField]
        private TMP_Text _levelText;

        [SerializeField]
        private Image _planetImage;

        private IPlanetPanelPresenter _presenter;

        private void OnDestroy()
        {
            _presenter?.Dispose();
            
            _closeButton.onClick.RemoveListener(ClosePanel);
            _purchaseButton.onClick.RemoveListener(Purchase);
        }

        public void SetPresenter(IPlanetPanelPresenter presenter)
        {
            _presenter = presenter;

            _presenter.OnUpdatedData += UpdateVisual;
            
            _closeButton.onClick.AddListener(ClosePanel);
            _purchaseButton.onClick.AddListener(Purchase);
        }

        private void UpdateVisual()
        {
            _planetNameText.text = $"Planet #{_presenter.Id + 1}";
            _populationText.text = $"Population {999}";
            _levelText.text = $"Level {_presenter.Level}";
            _incomeText.text = $"Income {_presenter.Income}";
            _priceText.text = $"{_presenter.NextUpgradePrice.Count}";
            _populationText.text = $"{_presenter.Population}";
            _planetImage.sprite = _presenter.Sprite;
            
            _purchaseButton.gameObject.SetActive(_presenter.NextUpgradePrice.Count > 0);
            
        }

        public void SetupPlanet(int id)
        {
            _presenter.SetupPlanet(id);
        }

        private void Purchase()
        {
            _presenter.Purchase();
        }

        private void ClosePanel()
        {
            Hide();
        }
    }
}