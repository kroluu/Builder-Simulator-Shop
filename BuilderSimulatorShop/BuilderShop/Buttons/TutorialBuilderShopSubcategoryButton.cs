using System.Linq;
using Core.Events;
using DG.Tweening;

namespace UI.Game.ReworkTablet.BuilderShop.Buttons
{
    /// <summary>
    /// Subcategory shop button for holding tutorial behaviour
    /// </summary>
    public class TutorialBuilderShopSubcategoryButton : BuilderShopSubcategoryButton
    {
        protected override void Awake()
        {
            base.Awake();
            SubscribeMethods();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnSubscribeMethods();
        }

        private void SubscribeMethods()
        {
            GameEvents.UIEvents.OnHighlightTabletCategory += SetPermissionForHighlightButton;
        }

        private void UnSubscribeMethods()
        {
            GameEvents.UIEvents.OnHighlightTabletCategory -= SetPermissionForHighlightButton;
        }

        private void SetPermissionForHighlightButton(ConstructionObjectCategory[] _subcategories)
        {
            bool containsSubcategory = _subcategories.Contains(subcategory);
            
            if (containsSubcategory)
            {
                tutorialHighlight.enabled = true;
                tutorialHighlight.DOFade(0f, 0f);
                tutorialHighlight.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                tutorialHighlight.DOKill();
                tutorialHighlight.enabled = false;
                tutorialHighlight.DOFade(0f, 0f);
            }
        }
    }
}