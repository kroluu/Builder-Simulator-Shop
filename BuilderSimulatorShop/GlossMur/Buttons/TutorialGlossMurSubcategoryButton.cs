using System;
using System.Collections.Generic;
using Core.Events;
using Data.Furnishing;
using DG.Tweening;

namespace UI.Game.ReworkTablet.GlossMur.Buttons
{
    /// <summary>
    /// Subcategory shop button for holding tutorial behaviour
    /// </summary>
    public class TutorialGlossMurSubcategoryButton : GlossMurSubcategoryButton
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
            GameEvents.UIEvents.OnHighlightTabletGlossMurSubcategory += SetPermissionForHighlight;
        }

        private void UnSubscribeMethods()
        {
            GameEvents.UIEvents.OnHighlightTabletGlossMurSubcategory -= SetPermissionForHighlight;
        }

        private void SetPermissionForHighlight(FurnitureRoomCategory? _category, params string[] _subcategories)
        {
            bool equals = EqualityComparer<Enum>.Default.Equals(_category, subcategory);
            if (equals)
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