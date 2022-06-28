using Core.Events;
using UI.Game.ReworkToolbar;
using UnityEngine;

namespace UI.Game.ReworkTablet.Info
{
    public class TabletGamepadHelpHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] builderPromptsOnly;
        [SerializeField] private GameObject[] glossMurPromptsOnly;
        [SerializeField] private CanvasGroup helpCG;
        
        private void OnDeviceDetect(InputDeviceDetector.DeviceType _)
        {
            helpCG.alpha = InputDeviceDetector.GamepadActive ? 1 : 0;
        }
        private void Awake()
        {
            GameEvents.UIEvents.OnToolbarChange += OnChangeShop;
            InputDeviceDetector.OnDeviceChange += OnDeviceDetect;
        }

        private void Start()
        {
            OnChangeShop(ToolbarSwitcher.CurrentToolbarType);
            OnDeviceDetect(InputDeviceDetector.Instance.CurrentDevice);
        }

        private void OnDestroy()
        {
            GameEvents.UIEvents.OnToolbarChange -= OnChangeShop;
            InputDeviceDetector.OnDeviceChange -= OnDeviceDetect;
        }

        private void OnChangeShop(ToolbarType _toolbarType)
        {
            bool builderShop = _toolbarType == ToolbarType.Builder;

            for (int i = 0; i < builderPromptsOnly.Length; i++)
            {
                builderPromptsOnly[i].SetActive(builderShop);
            }

            for (int i = 0; i < glossMurPromptsOnly.Length; i++)
            {
                glossMurPromptsOnly[i].SetActive(!builderShop);
            }
        }
    }
}
