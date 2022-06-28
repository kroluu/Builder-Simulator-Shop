using Core;
using Core.Events;
using UI.Game.ReworkTablet.BuilderShop.Tutorial;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.BuilderShop.Shop
{
    /// <summary>
    /// Build24 shop buy element class for storing tutorial information about construction object
    /// </summary>
    public class TutorialBuilderShopBuyElement : BuilderShopBuyElement, IShopElementTutorial
    {
        [field: SerializeField] public Image TutorialIndicator { get; set; }
        public void Indicate()
        {
            /*bool canIndicate = TabletContainer.Instance.Resolve<BuilderShopElementIndicatorKeeper>().CanIndicate(constructionObjectID);
            if (canIndicate)
            {
                TutorialIndicator.enabled = true;
                TutorialIndicator.DOFade(0f, 0f);
                TutorialIndicator.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                return;
            }
            
            TutorialIndicator.DOKill();
            TutorialIndicator.enabled = false;
            TutorialIndicator.DOFade(0f, 0f);*/
        }
        
        public void SetPermission()
        {
            MainBehaviourButton.interactable = TabletContainer.Instance.Resolve<BuilderShopElementIndicatorKeeper>().CanIndicate(constructionObjectID);
            SecondBehaviourButton.interactable = MainBehaviourButton.interactable;
        }

        protected override void Awake()
        {
            BuilderShopElementIndicatorKeeper.OnSetPermission += SetPermission;
            base.Awake();
        }

        public override void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview)
        {
            base.AssignValuesToElement(_cost, _name, _translateKey, _quantity, _preview);
            SetPermission();
        }

        protected override void BuyElement()
        {
            base.BuyElement();
            GameEvents.UIEvents.PublishOnToolbarUpdate(constructionObjectID);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            BuilderShopElementIndicatorKeeper.OnSetPermission -= SetPermission;
        }

        protected override int InventoryElementLimit => Config.TUTORIAL_MAX_AMOUNT_OF_ITEM_IN_INVENTORY;


        protected override void SetInteractionPermission(ref ConstructionObjectID[] _constructionsAllowedToInteract)
        {
            base.SetInteractionPermission(ref _constructionsAllowedToInteract);
            
        }

        
    }
}