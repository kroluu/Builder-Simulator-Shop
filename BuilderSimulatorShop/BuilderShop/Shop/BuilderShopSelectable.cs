using Core.Managers;
using UI.Game.ReworkTablet.BuilderShop.Gamepad;
using UI.Game.ReworkTablet.Shop;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.BuilderShop.Shop
{
    /// <summary>
    /// Gamepad element behaviour for Build24 shop
    /// </summary>
    public class BuilderShopSelectable : ShopSelectable<BuilderShopElement>
    {
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            isSelected = true;
            BuilderShopGamepadInputHandler.AssignContentElement(gameObject);
            if(InputDeviceDetector.GamepadActive)
                BuilderShopGamepadInputHandler.ScrollToElement(elementRect);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
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
        }
    }
}