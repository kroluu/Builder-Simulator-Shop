using System;
using UI.Game.ReworkTablet.BuilderShop.Buttons;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.BuilderShop.Spawners
{
    public class BuilderShopSubcategoryStateKeeper : MonoBehaviour
    {
        private static event Action<IShopSubcategorySelector> OnSelectSubcategory;
        private static event Action OnTabletRefreshSpawn;
        private static IShopSubcategorySelector CurrentSubcategorySelected;
        
        private void Start()
        {
            transform.GetChild(0).GetComponentInChildren<BuilderShopSubcategoryButton>().OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void Awake()
        {
            CurrentSubcategorySelected = null;
            OnSelectSubcategory += SelectSubcategory;
        }

        private void OnDestroy()
        {
            OnSelectSubcategory -= SelectSubcategory;
        }

        public static void PublishOnSelectSubcategory(IShopSubcategorySelector _subcategoryToSelect)
        {
            OnSelectSubcategory?.Invoke(_subcategoryToSelect);
        }

        public static void PublishOnTabletRefreshSpawn()
        {
            OnTabletRefreshSpawn?.Invoke();
        }

        public static IShopSubcategorySelector GetSelectedSubcategory => CurrentSubcategorySelected;

        private void SelectSubcategory(IShopSubcategorySelector _subcategoryToSelect)
        {
            if(CurrentSubcategorySelected == _subcategoryToSelect) return;
            CurrentSubcategorySelected?.DeselectButton();
            CurrentSubcategorySelected = _subcategoryToSelect;
            CurrentSubcategorySelected.SelectButton();
        }
    }
}
