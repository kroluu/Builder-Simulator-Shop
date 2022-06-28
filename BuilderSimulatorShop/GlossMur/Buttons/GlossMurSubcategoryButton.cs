using Data.Furnishing;
using Languages;
using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Enums;
using UI.Game.ReworkTablet.GlossMur.ElementPanel;
using UI.Game.ReworkTablet.GlossMur.Gamepad;
using UI.Game.ReworkTablet.GlossMur.Spawners;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Buttons
{
    public class GlossMurSubcategoryButton : ShopSubcategoryButton<FurnitureRoomCategory>
    {
        protected override void DoSpawn()
        {
            base.DoSpawn();
            TabletContainer.Instance.Resolve<GlossMurShopElementPanelSwitcher>().TryChangePanel(ShopElementPanelType.Buy,false);
            GlossMurShopSubcategoryStateKeeper.PublishOnSelectSubcategory(subcategory, this);
            if (TabletContainer.Instance.Resolve<GlossMurShopSubcategoryAreaSpawner>() is { } spawner)
            {
                spawner.PublishOnElementSpawn(subcategory);
            }
            GlossMurShopGamepadInputHandler.SelectSubcategory(gameObject);
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            if(InputDeviceDetector.Instance.CurrentDevice != InputDeviceDetector.DeviceType.Controller) return;
            base.OnSelect(eventData);
            GlossMurShopGamepadInputHandler.SelectSubcategory(gameObject);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if(ReferenceEquals(GlossMurShopSubcategoryStateKeeper.GetCurrentSubcategorySelected, this)) return;
            base.OnDeselect(eventData);
        }
        
        public void AssignSubcategory(FurnitureRoomCategory _subcategory)
        {
            subcategory = _subcategory;
            Translate();
        }

        protected override void Translate()
        {
            base.Translate();
            subcategoryTMP.text = LanguageSupport.GetStringFromDictionary($"Tablet_GlossMur_Subcategory_{subcategory}");
        }
    }
}