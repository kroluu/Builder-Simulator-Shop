using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Tutorial;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.GlossMur.Shop
{
    public class TutorialGlossMurShopSellElement : GlossMurShopSellElement, IShopElementTutorial
    {
        [field: SerializeField] public Image TutorialIndicator { get; set; }
        
        protected override void Awake()
        {
            GlossMurShopIndicatorKeeper.OnSetPermission += SetPermission;
            base.Awake();
        }

        private void OnDestroy()
        {
            GlossMurShopIndicatorKeeper.OnSetPermission -= SetPermission;
        }
        public void Indicate()
        {
            
        }

        public void SetPermission()
        {
            MainBehaviourButton.interactable = TabletContainer.Instance.Resolve<GlossMurShopIndicatorKeeper>().CanIndicate(Name);
        }

        public override void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview)
        {
            base.AssignValuesToElement(_cost, _name, _translateKey, _quantity, _preview);
            SetPermission();
        }
    }
}