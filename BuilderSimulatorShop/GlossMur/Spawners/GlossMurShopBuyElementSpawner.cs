using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Audio;
using Core.Managers;
using Data.Furnishing;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Interfaces;
using UI.Game.ReworkTablet.GlossMur.Shop;
using UI.Game.ReworkTablet.Interfaces;
using UI.Game.ReworkTablet.Spawners;
using UnityEngine;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    /// <summary>
    /// GlossMur spawner class derrived from ShopBuyElementSpawner for spawning furnitures meant to buy by player
    /// </summary>
    public class GlossMurShopBuyElementSpawner : ShopBuyElementSpawner<string>
    {
        private CancellationTokenSource tokenSource = null;
        protected override void Awake()
        {
            Bind(this);
            base.Awake();
        }

        private void OnDisable()
        {
            tokenSource?.Cancel();
        }

        public override void CancelAsyncSpawner()
        {
            tokenSource?.Cancel();
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }

        /// <summary>
        /// Tries to spawn subcategory's elements
        /// </summary>
        /// <param name="_subcategoryToSpawn">Subcategory passed to be spawned</param>
        protected override void TrySpawnElements(string _subcategoryToSpawn)
        {
            if(!CanSpawn(_subcategoryToSpawn)) return;
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Category");
            SpawnElements(_subcategoryToSpawn);
        }

        public override void ForceSpawn()
        {
            if (selectedSubcategory.Contains("All"))
            {
                SpawnAllElements(selectedSubcategory);
                return;
            }
            base.ForceSpawn();
        }

        public void TrySpawnAllElements(string _subcategoryToSpawn)
        {
            if(!CanSpawnAll(_subcategoryToSpawn)) return;
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "Button_Accept_Click");
            SpawnAllElements(_subcategoryToSpawn);
        }

        private bool CanSpawnAll(string _subcategoryToSpawn)
        {
            return selectedSubcategory != _subcategoryToSpawn;
        }

        private void SpawnAllElements(string _subcategoryToSpawn)
        {
            base.SpawnElements(_subcategoryToSpawn);
            
            var container = AssetsManager.Instance.furnituresDatabase.GetFurnituresContainer(GlossMurShopSubcategoryStateKeeper.CurrentRoomCategory);
            if (!container) return;
            
            List<KeyValuePair<string, FurnituresSet>> allFurnitures = new List<KeyValuePair<string, FurnituresSet>>();
            FurnituresContainer.Subcategory[] category = container.GetAllSubcategories;
            
            for (int i = 0; i < category.Length; i++)
            {
                for (int j = 0; j < category[i].furnituresSets.Length; j++)
                {
                    allFurnitures.Add(new KeyValuePair<string, FurnituresSet>(category[i].subcategoryName,category[i].furnituresSets[j]));
                }
            }
            
            DelaySpawn(allFurnitures);
        }

        protected override void SpawnElements(string _subcategoryToSpawn)
        {
            base.SpawnElements(_subcategoryToSpawn);
            
            List<KeyValuePair<string, FurnituresSet>> furnituresToSpawn = new List<KeyValuePair<string, FurnituresSet>>();
            FurnituresSet[] furnituresSet = AssetsManager.Instance.furnituresDatabase.GetFurnituresContainer(GlossMurShopSubcategoryStateKeeper.CurrentRoomCategory)
                .GetSubcategory(_subcategoryToSpawn).furnituresSets;
            for (int i = 0; i < furnituresSet.Length; i++)
            {
                furnituresToSpawn.Add(new KeyValuePair<string, FurnituresSet>(_subcategoryToSpawn,furnituresSet[i]));
            }
            //Profiler.BeginSample("SpawnAlloc");
            DelaySpawn(furnituresToSpawn);
            //Profiler.EndSample();
        }

        /// <summary>
        /// Asynchronously method for spawning elements passed as list in parameter
        /// </summary>
        /// <param name="_furnituresToSpawn">List of elements for spawning</param>
        protected virtual async void DelaySpawn(IList<KeyValuePair<string, FurnituresSet>> _furnituresToSpawn)
        {
            GlossMurShopSelectableGenerator.ClearSelectables();
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            for (int i = 0; i < _furnituresToSpawn.Count; i++)
            {
                if(token.IsCancellationRequested)
                    return;
                Transform furniture = Instantiate(elementPrefab, elementsContent);
                if (furniture.TryGetComponent(out IShopElementInfo elementInfo) && furniture.TryGetComponent(out IShopFurnitureInfo furnitureInfo) && furniture.TryGetComponent(out GlossMurShopSelectable selectable))
                {
                    FurnituresSet furnitureSet = _furnituresToSpawn[i].Value;
                    
                    furnitureInfo.AssignFurniture(GlossMurShopSubcategoryStateKeeper.CurrentRoomCategory,_furnituresToSpawn[i].Key, furnitureSet);
                    elementInfo.AssignValuesToElement(furnitureSet.price, furnitureSet.name, furnitureSet.nameTranslationKey,0,furnitureSet.furnitures[0].render);
                    GlossMurShopSelectableGenerator.AddSelectable(selectable);
                }
                
                try
                {
                    await Task.Run(() => Task.Delay(Config.TABLET_ELEMENTS_SPAWN_DELAY, token), token);
                }
                catch (TaskCanceledException taskCancelException)
                {
#if UNITY_EDITOR || DEVMODE
                    Debug.Log($"Spawning at script {nameof(GlossMurShopBuyElementSpawner)} stopped");
#endif
                    return;
                }
            }
        }
    }
}