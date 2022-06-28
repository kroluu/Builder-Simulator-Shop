using System.Linq;
using Core.Events;
using Data.Furnishing;
using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Spawners;
using UI.Game.ReworkTablet.GlossMur.Tutorial;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Buttons
{
    public class GlossMurAreaButton : ShopAreaButton<string>
    {
        protected override void Awake()
        {
            base.Awake();
            SubscribeMethods();
        }

        protected override void Start()
        {
            base.Start();
            SetHighlight(GlossMurShopIndicatorKeeper.AreasToIndicate.Contains(areaName));
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
        protected override void DoSpawn()
        {
            base.DoSpawn();
            GlossMurShopSubcategoryAreaStateKeeper.PublishOnSelectArea(this);
            if (TabletContainer.Instance.Resolve<GlossMurShopBuyElementSpawner>() is { } spawner)
            {
                spawner.PublishOnElementSpawn(areaName);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if(ReferenceEquals(GlossMurShopSubcategoryAreaStateKeeper.CurrentSubcategorySelected, this)) return;
            base.OnPointerEnter(eventData);
            SelectButton();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if(ReferenceEquals(GlossMurShopSubcategoryAreaStateKeeper.CurrentSubcategorySelected, this)) return;
            base.OnPointerExit(eventData);
            DeselectButton();
        }
        
        private void SetPermissionForHighlight(FurnitureRoomCategory? _category, params string[] _subcategories)
        {
            bool containsSubcategory = _subcategories.Contains(areaName);
            SetHighlight(containsSubcategory);
        }
        
    }
}