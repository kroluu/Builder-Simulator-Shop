using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using UI.Game.ReworkTablet.Buttons;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Gamepad
{
    public class GlossMurShopGamepadAreaHandler : MonoBehaviour
    {
        private static readonly List<ShopAreaButton<string>> AREA_BUTTONS = new List<ShopAreaButton<string>>(1);
        private static int CurrentIndex = 0;
        private static event Action OnRefreshArea;
        private static event Action OnDelaySelection;

        private static void PublishOnDelaySelection()
        {
            OnDelaySelection?.Invoke();
        }

        public static void PublishOnRefreshArea()
        {
            OnRefreshArea?.Invoke();
        }

        private void Awake()
        {
            OnRefreshArea += DelayAssign;
            OnDelaySelection += DelaySelection;
        }

        private void OnDestroy()
        {
            OnRefreshArea -= DelayAssign;
            OnDelaySelection -= DelaySelection;
        }

        public static void GlossMurAreaInputHandle()
        {
            if (InputManager.Instance.inputActions.UI.Trigger_R.WasPressedThisFrame())
            {
                SelectElement(1);
            }
            else if (InputManager.Instance.inputActions.UI.Trigger_L.WasPressedThisFrame())
            {
                SelectElement(-1);
            }
        }

        private static void SelectElement(int _sign)
        {
            PublishOnDelaySelection();
            CurrentIndex += _sign;
            if (CurrentIndex >= AREA_BUTTONS.Count)
                CurrentIndex = 0;
            else if (CurrentIndex < 0)
                CurrentIndex = AREA_BUTTONS.Count - 1;
            AREA_BUTTONS[CurrentIndex].OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void DelaySelection()
        {
            StartCoroutine(DelaySelectionCoroutine());
        }

        private IEnumerator DelaySelectionCoroutine()
        {
            yield return new WaitForEndOfFrame();
            GlossMurShopGamepadInputHandler.SelectElement(1);
        }

        private void DelayAssign()
        {
            StartCoroutine(AssignAreasForInputHandle());
        }

        private IEnumerator AssignAreasForInputHandle()
        {
            yield return new WaitForEndOfFrame();
            AREA_BUTTONS.Clear();
            foreach (Transform child in transform)
            {
                if (child.GetChild(0).TryGetComponent(out ShopAreaButton<string> button))
                {
                    AREA_BUTTONS.Add(button);
                }
            }

            CurrentIndex = 0;
        }
    }
}
