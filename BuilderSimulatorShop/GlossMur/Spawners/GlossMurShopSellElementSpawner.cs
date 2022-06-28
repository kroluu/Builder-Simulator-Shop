using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Builder.Data;
using Core;
using Core.Helpers;
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
    /// GlossMur spawner class derrived from ShopBuyElementSpawner for spawning construction objects meant to sell by player
    /// </summary>
    public class GlossMurShopSellElementSpawner : ShopSellElementSpawner
    {
        private CancellationTokenSource tokenSource = null;
        private void Awake()
        {
            Bind(this);
            SubscribeToEvents();
        }
        
        private void OnDisable()
        {
            tokenSource?.Cancel();
        }

        private void SubscribeToEvents()
        {
            ElementPanel.GlossMurShopElementRefreshHandler.OnSellPanelPickupRefresh += SpawnElements;
        }

        private void UnsubscribeFromEvents()
        {
            ElementPanel.GlossMurShopElementRefreshHandler.OnSellPanelPickupRefresh -= SpawnElements;
        }

        public override void CancelAsyncSpawner()
        {
            tokenSource?.Cancel();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        protected override void SpawnElements()
        {
            sellContent.DestroyChildren();
            sellContent.DetachChildren();
            List<FurnitureObjectData> playerFurnitures = ScenesCommunicator.GetGameData.equipmentData.GetFurnitureList();
            DelaySpawn(playerFurnitures);
        }
        
        /// <summary>
        /// Asynchronously method for spawning elements on UI passed as list in parameter
        /// </summary>
        /// <param name="_furnitures">List of elements meant to spawn</param>
        private async void DelaySpawn(IList<FurnitureObjectData> _furnitures)
        {
            GlossMurShopSellSelectableGenerator.ClearSelectables();
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            for (int i = 0; i < _furnitures.Count; i++)
            {
                if(token.IsCancellationRequested)
                    return;
                Transform element = Instantiate(sellPrefab, sellContent);
                if (element.TryGetComponent(out IShopElementInfo elementInfo) && element.TryGetComponent(out IShopFurnitureColorIndex colorIndex) && element.TryGetComponent(out GlossMurShopSelectable selectable))
                {
                    FurnituresSet furnitureData = AssetsManager.Instance.furnituresDatabase.GetFurnituresContainer(_furnitures[i].category)
                        .GetSubcategory(_furnitures[i].subcategory).furnituresSets
                        .FirstOrDefault(_x => _x.name == _furnitures[i].name);
                    colorIndex.SelectedIndex = _furnitures[i].index;
                    elementInfo.AssignValuesToElement(_furnitures[i].cost,_furnitures[i].name, furnitureData.nameTranslationKey,
                        _furnitures[i].amount, 
                        furnitureData.furnitures[_furnitures[i].index].render);
                    
                    GlossMurShopSellSelectableGenerator.AddSelectable(selectable);
                }

                try
                {
                    await Task.Run(() => Task.Delay(Config.TABLET_ELEMENTS_SPAWN_DELAY, token), token);
                }
                catch (TaskCanceledException taskCancelException)
                {
#if UNITY_EDITOR || DEVMODE
                    Debug.Log($"Spawning at script {nameof(GlossMurShopSellElementSpawner)} stopped");
#endif
                    return;
                }
            }
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }
    }
}