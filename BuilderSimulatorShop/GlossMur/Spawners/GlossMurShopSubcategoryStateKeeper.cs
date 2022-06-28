using System;
using Data.Furnishing;
using UI.Game.ReworkTablet.GlossMur.Buttons;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    public class GlossMurShopSubcategoryStateKeeper : MonoBehaviour
    {
        private static event Action<FurnitureRoomCategory, IShopSubcategorySelector> OnSelectSubcategory; 
        private static IShopSubcategorySelector CurrentSubcategorySelected;
        public static FurnitureRoomCategory CurrentRoomCategory;

        public static IShopSubcategorySelector GetCurrentSubcategorySelected => CurrentSubcategorySelected;
        private void Start()
        {
            transform.GetChild(0).GetComponentInChildren<GlossMurSubcategoryButton>().OnPointerClick(new PointerEventData(EventSystem.current));
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

        public static void PublishOnSelectSubcategory(FurnitureRoomCategory _roomCategory, IShopSubcategorySelector _subcategoryToSelect)
        {
            OnSelectSubcategory?.Invoke(_roomCategory, _subcategoryToSelect);
        }

        private void SelectSubcategory(FurnitureRoomCategory _roomCategory, IShopSubcategorySelector _subcategoryToSelect)
        {
            if(CurrentSubcategorySelected == _subcategoryToSelect) return;
            CurrentRoomCategory = _roomCategory;
            CurrentSubcategorySelected?.DeselectButton();
            CurrentSubcategorySelected = _subcategoryToSelect;
            CurrentSubcategorySelected.SelectButton();
        }
    }
}
