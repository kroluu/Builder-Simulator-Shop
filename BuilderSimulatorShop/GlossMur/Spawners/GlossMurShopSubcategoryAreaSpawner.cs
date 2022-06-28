using Core.Audio;
using Core.Helpers;
using Core.Managers;
using Data.Furnishing;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Gamepad;
using UI.Game.ReworkTablet.Interfaces;
using UI.Game.ReworkTablet.Spawners;
using UnityEngine;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    public class GlossMurShopSubcategoryAreaSpawner : ShopSubcategoryAreaSpawner<FurnitureRoomCategory>
    {
        private const string ALL_AREA_TRANSLATION_KEY = "Tablet_GlossMur_Area_All";
        protected override void Awake()
        {
            selectedSubcategory = FurnitureRoomCategory.Bedroom;
            Bind(this);
            base.Awake();
        }
        
        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }

        protected override void TrySpawnElements(FurnitureRoomCategory _fromSubcategory)
        {
            if(!CanSpawn(_fromSubcategory)) return;
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Category");
            SpawnAreas(_fromSubcategory);
        }

        protected override void SpawnAreas(FurnitureRoomCategory _fromSubcategory)
        {
            selectedSubcategory = _fromSubcategory;
            FurnituresContainer container = AssetsManager.Instance.furnituresDatabase.GetFurnituresContainer(_fromSubcategory);

            areaContent.DestroyChildrenExceptFirst();
            if (areaContent.GetChild(0).TryGetComponent(out IShopAreaInfo mainAreaInfo))
            {
                mainAreaInfo.AssignToArea($"All_{_fromSubcategory.FastString()}",ALL_AREA_TRANSLATION_KEY,null);
            }

            if (container)
            {
                FurnituresContainer.Subcategory[] subcategories = container.GetAllSubcategories;
                for (int i = 0; i < subcategories.Length; i++)
                {
                    Transform area = Instantiate(areaPrefab, areaContent);
                    if (area.TryGetComponent(out IShopAreaInfo areaInfo))
                    {
                        areaInfo.AssignToArea(subcategories[i].subcategoryName, subcategories[i].subcategoryTranslationKey, subcategories[i].subcategoryIcon);
                    }
                }
            }

            GlossMurShopSubcategoryAreaStateKeeper.PublishOnSpawnFromMainArea();
            GlossMurShopGamepadAreaHandler.PublishOnRefreshArea();
        }
    }
}