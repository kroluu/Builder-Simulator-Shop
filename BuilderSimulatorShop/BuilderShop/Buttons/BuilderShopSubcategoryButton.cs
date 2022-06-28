using Languages;
using UI.Game.ReworkTablet.BuilderShop.ElementPanel;
using UI.Game.ReworkTablet.BuilderShop.Gamepad;
using UI.Game.ReworkTablet.BuilderShop.Spawners;
using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Enums;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.BuilderShop.Buttons
{
    /// <summary>
    /// Build24 button class for spawning construction objects able to buy
    /// </summary>
    public class BuilderShopSubcategoryButton : ShopSubcategoryButton<ConstructionObjectCategory>, IShopSubcategoryAssigner<ConstructionObjectCategory>
    {
        /// <summary>
        /// Spawns construction objects able to buy if possible
        /// </summary>
        protected override void DoSpawn()
        {
            base.DoSpawn();
            bool viewChanged = TabletContainer.Instance.Resolve<BuilderShopElementPanelSwitcher>().TryChangePanel(ShopElementPanelType.Buy,false);
            BuilderShopSubcategoryStateKeeper.PublishOnSelectSubcategory(this);
            if (TabletContainer.Instance.Resolve<BuilderShopBuyElementSpawner>() is { } spawner)
            {
                if(viewChanged) spawner.ForceSpawn(subcategory);
                else spawner.PublishOnElementSpawn(subcategory);
            }
            BuilderShopGamepadInputHandler.SelectSubcategory(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if(InputDeviceDetector.Instance.CurrentDevice != InputDeviceDetector.DeviceType.Controller) return;
            base.OnSelect(eventData);
            BuilderShopGamepadInputHandler.SelectSubcategory(gameObject);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if(ReferenceEquals(BuilderShopSubcategoryStateKeeper.GetSelectedSubcategory, this)) return;
            base.OnDeselect(eventData);
        }

        /// <summary>
        /// Assigns subcategory
        /// </summary>
        /// <param name="_subcategory"></param>
        public void AssignSubcategory(ConstructionObjectCategory _subcategory)
        {
            subcategory = _subcategory;
            Translate();
        }

        /// <summary>
        /// Translate subcategory name
        /// </summary>
        protected override void Translate()
        {
            base.Translate();
            subcategoryTMP.text = LanguageSupport.GetStringFromDictionary($"Tablet_Contorama_Category_{subcategory}");
        }
    }
}