using System;
using Core.Audio;
using UI.Game.ReworkTablet.GlossMur.Buttons;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    public class GlossMurShopSubcategoryAreaStateKeeper : MonoBehaviour
    {
        private static event Action<IShopSubcategorySelector> OnSelectArea;
        private static event Action OnSpawnFromMainArea;
        public static IShopSubcategorySelector CurrentSubcategorySelected;
        [SerializeField] private GlossMurMainAreaButton mainAreaButton;
        

        private void Awake()
        {
            OnSelectArea += SelectArea;
            OnSpawnFromMainArea += SpawnFromMainArea;
        }

        private void OnDestroy()
        {
            OnSelectArea -= SelectArea;
            OnSpawnFromMainArea -= SpawnFromMainArea;
        }
        
        public static void PublishOnSelectArea(IShopSubcategorySelector _subcategoryToSelect)
        {
            OnSelectArea?.Invoke(_subcategoryToSelect);
        }

        public static void PublishOnSpawnFromMainArea()
        {
            OnSpawnFromMainArea?.Invoke();
        }

        private void SelectArea(IShopSubcategorySelector _subcategoryToSelect)
        {
            if(CurrentSubcategorySelected == _subcategoryToSelect) return;
            CurrentSubcategorySelected?.DeselectButton();
            CurrentSubcategorySelected = _subcategoryToSelect;
            CurrentSubcategorySelected.SelectButton();
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Filter");
        }

        private void SpawnFromMainArea()
        {
            CurrentSubcategorySelected = null;
            mainAreaButton.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}