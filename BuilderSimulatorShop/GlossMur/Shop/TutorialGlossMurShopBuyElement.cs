using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Tutorial;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.GlossMur.Shop
{
    public class TutorialGlossMurShopBuyElement : GlossMurShopBuyElement, IShopElementTutorial
    {
        public Image TutorialIndicator { get; set; }
        
        protected override void Awake()
        {
            GlossMurShopIndicatorKeeper.OnSetPermission += SetPermission;
            base.Awake();
        }

        public override void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview)
        {
            base.AssignValuesToElement(_cost, _name, _translateKey, _quantity, _preview);
            SetPermission();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GlossMurShopIndicatorKeeper.OnSetPermission -= SetPermission;
        }

        public override void ChangeColor(int _colorIndexToShow)
        {
            base.ChangeColor(_colorIndexToShow);
            SetPermission();
        }

        public void Indicate()
        {
            
        }

        public void SetPermission()
        {
            MainBehaviourButton.interactable = FurnituresSet.furnitures is {} && TabletContainer.Instance.Resolve<GlossMurShopIndicatorKeeper>().CanIndicate(FurnituresSet.furnitures[SelectedIndex].assetReference.AssetGUID);
        }
    }
}