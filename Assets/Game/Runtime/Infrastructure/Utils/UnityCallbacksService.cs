using System;
using UnityEngine;

namespace Infrastructure.Utils
{
    public class UnityCallbacksService : MonoBehaviour
    {
        private void OnApplicationFocus(bool hasFocus)
        {
            OnApplicationFocusChanged?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnApplicationPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitted?.Invoke();
        }

        public event Action<bool> OnApplicationPaused;
        public event Action<bool> OnApplicationFocusChanged;
        public event Action OnApplicationQuitted;
    }
}