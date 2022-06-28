using Core;
using Core.Audio;
using Core.Data;
using Core.Events;
using Core.Managers;
using Languages;
using UI.Game.ReworkTablet.BuilderShop.Gamepad;
using UI.Popups;

namespace UI.Game.ReworkTablet.BuilderShop.Shop
{
    /// <summary>
    /// Build24 shop sell element class for storing information about construction object
    /// </summary>
    public class BuilderShopSellElement : BuilderShopElement
    {
        private int SellCost => BuyCost / Config.INVENTORY_ITEM_SELL_FACTOR;
        protected override void OnMainButtonBehaviour()
        {
            if(!CanSellElement()) return;
            Sell();
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "Button_Reject_Click");
            SecondButtonSound();
        }
        
        /// <summary>
        /// Checks if player can sell construction object
        /// </summary>
        /// <returns>Able to sell or not</returns>
        private bool CanSellElement()
        {
            return ScenesCommunicator.GetGameData.equipmentData.GetConstructionItemAmount(constructionObjectID) > 0;
        }

        /// <summary>
        /// Sells construction object from player equipment
        /// </summary>
        /// <param name="_amount"></param>
        private void Sell(int _amount = 1)
        {
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            equipmentData.RemoveFromConstructionItem(constructionObjectID, _amount);
            Quantity -= _amount;
            equipmentData.PlayerCash += SellCost * _amount;
            RefreshValues();
            GameEvents.UIEvents.PublishOnToolbarRefresh();
            TryDestroyElement();
        }

        /// <summary>
        /// Tries destroy UI element if capacity reaches zero
        /// </summary>
        private void TryDestroyElement()
        {
            if(!CanDestroy()) return;
            Destroy(gameObject);
            if (InputDeviceDetector.Instance.CurrentDevice == InputDeviceDetector.DeviceType.Controller)
            {
                BuilderShopGamepadInputHandler.PublishOnRefreshElementSelect();
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            TryDestroyElement();
        }

        /// <summary>
        /// Checks if quantity of construction object in player equipment is less or equal to zero
        /// </summary>
        /// <returns></returns>
        private bool CanDestroy()
        {
            return Quantity <= 0;
        }

        /// <summary>
        /// Asks if player is sure to sell all specific construction object from equipment
        /// </summary>
        private void AskForSellEverything()
        {
            if (!ScenesCommunicator.GetGameData.PlayerProfile.showSellEverythingPopup)
            {
                Sell(Quantity);
                return;
            }
            PopupManager.Instance.FirePopup(PopupType.BuildShopSellConfirm,LanguageSupport.GetStringFromDictionary("Tablet_Popup_Sell_All_Description"),
                new PopupParam(LanguageSupport.GetStringFromDictionary("UI_Button_No"), () =>
                {
                    PopupManager.Instance.KillInstancedPopup();
                }),
                new PopupParam(LanguageSupport.GetStringFromDictionary("UI_Button_Yes"), ()=>
                {
                    Sell(Quantity);
                    PopupManager.Instance.KillInstancedPopup();
                }));
            
        }
        

        protected override void OnSecondButtonBehaviour()
        {
            //AskForSellEverything();
            Sell(Quantity);
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "Button_Accept_Click");
            SecondButtonClickAnimation();
        }

        protected override void SetInteractionPermission(ref ConstructionObjectID[] _constructionsAllowedToInteract)
        {
            
        }
    }
}