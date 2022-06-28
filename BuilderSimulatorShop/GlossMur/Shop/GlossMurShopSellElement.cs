using Core;
using Core.Data;
using Core.Managers;
using Languages;
using UI.Game.ReworkTablet.GlossMur.Gamepad;
using UI.Game.ReworkTablet.GlossMur.Interfaces;
using UI.Popups;

namespace UI.Game.ReworkTablet.GlossMur.Shop
{
    public class GlossMurShopSellElement : GlossMurShopElement, IShopFurnitureColorIndex
    {
        private int SellCost => BuyCost / Config.INVENTORY_ITEM_SELL_FACTOR;
        public int SelectedIndex { get; set; }

        protected override void Awake()
        {
            base.Awake();
            if(SecondBehaviourButton)
                SecondBehaviourButton.onClick.AddListener(OnSecondButtonBehaviour);
        }

        protected override void OnMainButtonBehaviour()
        {
            if(!CanSellFurniture()) return;
            Sell();
            SecondButtonSound();
            MainButtonSound();
        }
        

        private bool CanSellFurniture()
        {
            return ScenesCommunicator.GetGameData.equipmentData.GetFurnitureAmount(Name, SelectedIndex) > 0;
        }

        private void Sell(int _amount = 1)
        {
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            equipmentData.RemoveFurnitureFromEquipment(Name,SelectedIndex, _amount);
            Quantity -= _amount;
            equipmentData.PlayerCash += SellCost;
            RefreshValues();
            TryDestroyElement();
        }

        private void TryDestroyElement()
        {
            if(!CanDestroy()) return;
            Destroy(gameObject);
            if (InputDeviceDetector.Instance.CurrentDevice == InputDeviceDetector.DeviceType.Controller)
            {
                GlossMurShopGamepadInputHandler.PublishOnRefreshElementSelect();
            }
        }

        public override void RefreshValues()
        {
            Quantity = ScenesCommunicator.GetGameData.equipmentData.GetFurnitureAmount(Name, SelectedIndex);
            base.RefreshValues();
            TryDestroyElement();
        }

        private bool CanDestroy() => Quantity <= 0;

        private void AskForSellEverything()
        {
            if (!ScenesCommunicator.GetGameData.PlayerProfile.showSellEverythingPopup)
            {
                Sell(Quantity);
                return;
            }
            PopupManager.Instance.FirePopup(PopupType.GlossMurShopSellConfirm,LanguageSupport.GetStringFromDictionary("Tablet_Popup_Sell_All_Description"),
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
            SecondButtonSound();
        }
    }
}