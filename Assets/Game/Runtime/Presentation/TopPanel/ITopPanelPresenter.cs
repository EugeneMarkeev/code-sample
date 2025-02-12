using System;
using UnityEngine;

namespace Presentation.TopPanel
{
    public interface ITopPanelPresenter : IDisposable
    {
        public ulong SoftCurrencyCount { get; }
        Sprite SoftCurrencySprite { get; }
        public event Action OnSoftCurrencyChanged;
    }
}