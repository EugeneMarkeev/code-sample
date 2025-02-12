using Application.Game;
using Application.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Presentation
{
    public class PlanetView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Button _mainButton;

        [Header("Locked State")]
        [SerializeField]
        private Image _locker;

        [SerializeField]
        private TMP_Text _priceText;

        [SerializeField]
        private Image _priceIcon;

        [Header("Unlocked State")]
        [SerializeField]
        private Image _incomeReadyIcon;

        [SerializeField]
        private Button _incomeReadyButton;

        [SerializeField]
        private TMP_Text _incomeTimerText;

        [SerializeField]
        private Image _incomeProgressBarPanel;

        [SerializeField]
        private Image _incomeProgressBar;

        private IPlanetPresenter _presenter;
        private RectTransform _rectTransform;

        [Preserve]
        private void Construct()
        {
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
            if (_presenter == null) 
                return;
            
            _presenter.Dispose();
            _presenter.OnUpdatedStaticData -= UpdatedStaticData;
            _presenter.OnSwitchedState -= SwitchedState;
            _presenter.OnTickedIncomeTimer -= UpdateTimerBar;
        }

        public void SetPresenter(IPlanetPresenter presenter)
        {
            _presenter = presenter;
            _presenter.OnUpdatedStaticData += UpdatedStaticData;
            _presenter.OnSwitchedState += SwitchedState;
            _presenter.OnTickedIncomeTimer += UpdateTimerBar;

            _mainButton.onClick.AddListener(OnTryToBuy);
            _incomeReadyButton.onClick.AddListener(OnTakeIncome);
        }

        private void UpdateTimerBar()
        {
            if (_presenter.State != 1)
                return;

            _incomeProgressBar.fillAmount = Mathf.Round((float)(_presenter.IncomeTimer - _presenter.IncomeCountdown) /
                _presenter.IncomeTimer * 100) / 100;

            _incomeTimerText.text = $"{_presenter.IncomeCountdown} sec";
        }

        private void OnTakeIncome()
        {
            _presenter.TakeIncome();
        }

        private void OnTryToBuy()
        {
            _presenter.TryToBuyNextLevel();
        }

        private void SwitchedState()
        {
            UpdateStateView();
            UpdateTexts();
            UpdateTimerBar();
            UpdateSprite();
        }

        private void UpdatedStaticData()
        {
            UpdatePosition();
            UpdateSize();
            UpdateSprite();
        }

        private void UpdateSize()
        {
            _rectTransform.sizeDelta = new Vector2(_presenter.Radius, _presenter.Radius);
        }

        private void UpdateSprite()
        {
            _icon.sprite = _presenter.State == 0 ? _presenter.PlanetSpriteLocked : _presenter.PlanetSpriteUnlocked;
        }

        private void UpdatePosition()
        {
            _rectTransform.anchorMin = _presenter.Position.ToVector2();
            _rectTransform.anchorMax = _presenter.Position.ToVector2();
        }

        private void UpdateTexts()
        {
            if (_presenter.State != 0)
                return;

            _priceText.text = _presenter.NextLevel == null ? "MAX" : _presenter.NextLevel.Price.Count.ToString();
        }

        private void UpdateStateView()
        {
            _locker.gameObject.SetActive(_presenter.State == 0);
            _priceText.gameObject.SetActive(_presenter.State == 0);
            _priceIcon.gameObject.SetActive(_presenter.State == 0);

            _incomeTimerText.gameObject.SetActive(_presenter.State == 1);
            _incomeProgressBarPanel.gameObject.SetActive(_presenter.State == 1);

            _incomeReadyIcon.gameObject.SetActive(_presenter.State == 2);
        }
    }
}