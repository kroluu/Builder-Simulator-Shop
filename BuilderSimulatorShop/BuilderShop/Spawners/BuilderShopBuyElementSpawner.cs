using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Builder.Containers;
using Core;
using Core.Audio;
using Core.Data;
using Core.Helpers;
using Core.Managers;
using UI.Game.ReworkTablet.BuilderShop.Shop;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Interfaces;
using UI.Game.ReworkTablet.Spawners;
using UnityEngine;

namespace UI.Game.ReworkTablet.BuilderShop.Spawners
{
    /// <summary>
    /// Build24 spawner class derrived from ShopBuyElementSpawner for spawning construction objects meant to buy by player
    /// </summary>
    public class BuilderShopBuyElementSpawner : ShopBuyElementSpawner<ConstructionObjectCategory>
    {
        private CancellationTokenSource tokenSource = null;
        protected override void Awake()
        {
            selectedSubcategory = ConstructionObjectCategory.Roof;
            Bind(this);
            base.Awake();
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }
        
        /// <summary>
        /// Tries to spawn subcategory's elements
        /// </summary>
        /// <param name="_subcategoryToSpawn">Subcategory passed to be spawned</param>
        protected override void TrySpawnElements(ConstructionObjectCategory _subcategoryToSpawn)
        {
            if(!CanSpawn(_subcategoryToSpawn)) return;
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Category");
            SpawnElements(_subcategoryToSpawn);
        }

        protected override void SpawnElements(ConstructionObjectCategory _subcategoryToSpawn)
        {
            base.SpawnElements(_subcategoryToSpawn);
            List<ConstructionObjectRecord> itemsToSpawn = AssetsManager.Instance.inventoryContainer.GetConstructionObjectRecords()
                .Where(_x => _x.data.category == _subcategoryToSpawn).ToList();
            
            DelaySpawn(itemsToSpawn);
        }

        /// <summary>
        /// Asynchronously method for spawning elements passed as list in parameter
        /// </summary>
        /// <param name="_constructions">List of elements for spawning</param>
        private async void DelaySpawn(IList<ConstructionObjectRecord> _constructions)
        {
            BuilderShopSelectableGenerator.ClearSelectables();
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            for (var i = 0; i < _constructions.Count; i++)
            {
                if(token.IsCancellationRequested)
                    return;
                ConstructionObjectRecord objectRecord = _constructions[i];
                Transform element = Instantiate(elementPrefab, elementsContent);
                
                if (element.TryGetComponent(out IShopElementInfo shopElementInfo) && element.TryGetComponent(out BuilderShopSelectable selectable))
                {
                    shopElementInfo.AssignValuesToElement(objectRecord.data.cost, objectRecord.data.id.FastString(),
                        $"Game_ConstructionObjectID_{objectRecord.data.id.FastString()}",
                        equipmentData.GetConstructionItemAmount(objectRecord.data.id),
                        AssetsManager.Instance.inventoryContainer.GetConstructionSprite(objectRecord.data.id).constructionView);
                    BuilderShopSelectableGenerator.AddSelectable(selectable);
                }
                
                
                try
                {
                    await Task.Run(() => Task.Delay(Config.TABLET_ELEMENTS_SPAWN_DELAY, token), token);
                }
                catch (TaskCanceledException taskCancelException)
                {
#if UNITY_EDITOR || DEVMODE
                    Debug.Log($"Spawning at script {nameof(BuilderShopBuyElementSpawner)} stopped");
#endif
                    return;
                }
            }
        }
    }
}