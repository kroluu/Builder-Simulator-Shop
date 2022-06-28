using System.Threading;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;

namespace UI.Game.ReworkTablet.Spawners
{
    /// <summary>
    /// Abstract class for spawning elements on UI meant to sell
    /// </summary>
    public abstract class ShopSellElementSpawner : MonoBehaviour,IShopForceSpawnElements
    {
        [SerializeField] protected Transform sellPrefab;
        [SerializeField] protected Transform sellContent;
        protected CancellationToken currentCancellationToken;
        protected abstract void SpawnElements();
        protected abstract void Bind<TInherit>(TInherit _instance) where TInherit : ShopSellElementSpawner;
        public void ForceSpawn()
        {
            SpawnElements();
        }

        public virtual void CancelAsyncSpawner()
        {
            
        }
        
    }
}