using UI.Game.ReworkTablet.BuilderShop.Tutorial;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.BuilderShop.Shop
{
    /// <summary>
    /// Build24 shop sell element class for storing tutorial information about construction object
    /// </summary>
    public class TutorialBuilderShopSellElement : BuilderShopSellElement, IShopElementTutorial
    {
        [field: SerializeField] public Image TutorialIndicator { get; set; }
        
        protected override void Awake()
        {
            BuilderShopElementIndicatorKeeper.OnSetPermission += SetPermission;
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            BuilderShopElementIndicatorKeeper.OnSetPermission -= SetPermission;
        }
        public void Indicate()
        {
            
        }

        public void SetPermission()
        {
            MainBehaviourButton.interactable = TabletContainer.Instance.Resolve<BuilderShopElementIndicatorKeeper>().CanIndicate(constructionObjectID);
            SecondBehaviourButton.interactable = MainBehaviourButton.interactable;
        }

        public override void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview)
        {
            base.AssignValuesToElement(_cost, _name, _translateKey, _quantity, _preview);
            SetPermission();
        }
    }
}