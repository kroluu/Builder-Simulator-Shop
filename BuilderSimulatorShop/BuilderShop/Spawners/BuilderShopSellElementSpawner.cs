using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Builder.Data;
using Core;
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
    /// Build24 spawner class derrived from ShopBuyElementSpawner for spawning construction objects meant to sell by player
    /// </summary>
    public class BuilderShopSellElementSpawner : ShopSellElementSpawner
    {
        private CancellationTokenSource tokenSource = null;
        private void Awake()
        {
            Bind(this);
        }

        protected override void SpawnElements()
        {
            sellContent.DestroyChildren();

            List<ConstructionObjectData> playerInventory = ScenesCommunicator.GetGameData.equipmentData.GetConstructionList();
            DelaySpawn(playerInventory);
        }

        private async void DelaySpawn(IList<ConstructionObjectData> _constructions)
        {
            BuilderShopSelectableGenerator.ClearSelectables();
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            for (int i = 0; i < _constructions.Count; i++)
            {
                if(token.IsCancellationRequested)
                    return;
                Transform element = Instantiate(sellPrefab, sellContent);
                if (element.TryGetComponent(out IShopElementInfo elementInfo) && element.TryGetComponent(out BuilderShopSelectable selectable))
                {
                    elementInfo.AssignValuesToElement(_constructions[i].cost,_constructions[i].id.FastString(),
                        $"Game_ConstructionObjectID_{_constructions[i].id.FastString()}",_constructions[i].amount,
                        AssetsManager.Instance.inventoryContainer.GetConstructionSprite(_constructions[i].id).constructionView);
                    BuilderShopSelectableGenerator.AddSelectable(selectable);
                }

                try
                {
                    await Task.Run(() => Task.Delay(Config.TABLET_ELEMENTS_SPAWN_DELAY, token), token);
                }
                catch (TaskCanceledException taskCancelException)
                {
#if UNITY_EDITOR || DEVMODE
                    Debug.Log($"Spawning at script {nameof(BuilderShopSellElementSpawner)} stopped");
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