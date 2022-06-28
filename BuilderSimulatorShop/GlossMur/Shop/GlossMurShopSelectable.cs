using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using UI.Game.ReworkTablet.GlossMur.Buttons;
using UI.Game.ReworkTablet.GlossMur.Gamepad;
using UI.Game.ReworkTablet.Shop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Shop
{
    /// <summary>
    /// Gamepad element behaviour for GlossMur shop
    /// </summary>
    public class GlossMurShopSelectable : ShopSelectable<GlossMurShopElement>
    {
        private int gamepadSelectedColorIndex = 0;
        private IList<GlossMurFurnitureColorColorButton> colorButtons = Array.Empty<GlossMurFurnitureColorColorButton>();
        private Coroutine delaySelectCoroutine;
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            delaySelectCoroutine = StartCoroutine(DelaySelect());
            GlossMurShopGamepadInputHandler.AssignContentElement(gameObject);
            if(InputDeviceDetector.GamepadActive)
                GlossMurShopGamepadInputHandler.ScrollToElement(elementRect);
        }

        /// <summary>
        /// Injects furniture colors for navigation
        /// </summary>
        /// <param name="_colorButtons"></param>
        public void InjectColorsToNavigate(IList<GlossMurFurnitureColorColorButton> _colorButtons)
        {
            colorButtons = _colorButtons;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if(delaySelectCoroutine is {})
                StopCoroutine(delaySelectCoroutine);
            isSelected = false;
            base.OnDeselect(eventData);
        }

        private void Update()
        {
            if(!isSelected) return;

            if (InputManager.Instance.inputActions.UI.MainAction.WasPressedThisFrame())
            {
                element.MainBehaviourButton.OnPointerClick(new PointerEventData(EventSystem.current));
            }
            else if (InputManager.Instance.inputActions.UI.SecondaryAction.WasPressedThisFrame())
            {
                element.SecondBehaviourButton.OnPointerClick(new PointerEventData(EventSystem.current));
            }

            if (InputManager.Instance.inputActions.UI.Shoulder_R.WasPressedThisFrame())
            {
                SelectNext();
            }
        }

        private IEnumerator DelaySelect()
        {
            yield return new WaitForEndOfFrame();
            isSelected = true;
        }

        /// <summary>
        /// Selects next color variant
        /// </summary>
        private void SelectNext()
        {
            if(colorButtons.Count <= 0) return;
            if (++gamepadSelectedColorIndex >= colorButtons.Count)
                gamepadSelectedColorIndex = 0;
            colorButtons[gamepadSelectedColorIndex].OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}
