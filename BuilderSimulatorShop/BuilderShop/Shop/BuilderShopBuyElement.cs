
using Core;
using Core.Audio;
using Core.Data;
using Core.Events;
using Core.Helpers;
using Core.Managers;
using UI.Game.ReworkTablet.BuilderShop.Enums;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Enums;
using UI.Game.ReworkTablet.Popups;

namespace UI.Game.ReworkTablet.BuilderShop.Shop
{
    /// <summary>
    /// Build24 shop buy element class for storing information about construction object
    /// </summary>
    public class BuilderShopBuyElement : BuilderShopElement
    {
        private BuilderShopBuyCapacity buyCapacity = BuilderShopBuyCapacity.One;
        protected virtual int InventoryElementLimit => Config.MAX_AMOUNT_OF_ITEM_IN_INVENTORY;
        protected override void OnMainButtonBehaviour()
        {
            if(!CanBuyElement()) return;
            if (IsEquipmentFull())
            {
                if(!TabletPopupManager.IsFiring)
                    TabletContainer.Instance.Resolve<TabletPopupManager>().Fire(TabletPopupType.EquipmentFull);
                return;
            }
            BuyElement();
            MainButtonSound();
            MainButtonClickAnimation();
        }

        /// <summary>
        /// Buys specific construction object
        /// </summary>
        protected virtual void BuyElement()
        {
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            equipmentData.PlayerCash -= BuyCost * (int)buyCapacity;
            equipmentData.AddConstructionObjectToEQ(constructionObjectID, (int) buyCapacity);
            Quantity += (int)buyCapacity;
            RefreshValues();
            GameEvents.UIEvents.PublishOnToolbarUpdate(constructionObjectID);
        }

        /// <summary>
        /// Checks if player equipment is full
        /// </summary>
        /// <returns>If equipment is full or not</returns>
        private bool IsEquipmentFull()
        {
            return !ScenesCommunicator.GetGameData.equipmentData.HaveContructionItem(constructionObjectID) &&
                   ScenesCommunicator.GetGameData.equipmentData.IsConstructionEquipmentFull();
        }

        /// <summary>
        /// Checks if player is able to buy construction object
        /// </summary>
        /// <returns></returns>
        private bool CanBuyElement()
        {
            return ScenesCommunicator.GetGameData.equipmentData.CanAffordToBuy(BuyCost * (int)buyCapacity) 
                   && ScenesCommunicator.GetGameData.equipmentData.GetConstructionItemAmount(constructionObjectID) + (int) buyCapacity <= InventoryElementLimit;
        }

        protected override void OnSecondButtonBehaviour()
        {
            SetBuyCapacity();
            MainButtonSound();
            SecondButtonClickAnimation();
        }

        protected override void SetInteractionPermission(ref ConstructionObjectID[] _constructionsAllowedToInteract)
        {
            
        }

        /// <summary>
        /// Sets how many elements players going to buy
        /// </summary>
        private void SetBuyCapacity()
        {
            buyCapacity = buyCapacity.NextEnum();
            secondBehaviourTMP.text = $"X{(int)buyCapacity}";
            CostTMP.text = $"${BuyCost * (int)buyCapacity}";
        }

        protected override void SecondButtonSound()
        {
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Amount");
        }
    }
}