using System;
using System.Collections.Generic;
using Core.Helpers;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;

namespace UI.Game.ReworkTablet.Spawners
{
    /// <summary>
    /// Abstract class for spawning elements on UI meant to buy
    /// </summary>
    /// <typeparam name="TEnum">Type of elements to spawn</typeparam>
    public abstract class ShopBuyElementSpawner<TEnum> : MonoBehaviour,IShopForceSpawnElements
    {
        private event Action<TEnum> OnElementSpawn;
        [SerializeField] protected Transform elementPrefab;
        [SerializeField] protected Transform elementsContent;
        protected TEnum selectedSubcategory;
        
        protected virtual void Awake()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            OnElementSpawn += TrySpawnElements;
        }

        private void Unsubscribe()
        {
            OnElementSpawn -= TrySpawnElements;
        }

        public void PublishOnElementSpawn(TEnum _subcategoryToSpawn)
        {
            OnElementSpawn?.Invoke(_subcategoryToSpawn);
        }

        protected abstract void Bind<TInherit>(TInherit _instance) where TInherit : ShopBuyElementSpawner<TEnum>;
        protected abstract void TrySpawnElements(TEnum _subcategoryToSpawn);
        protected virtual void SpawnElements(TEnum _subcategoryToSpawn)
        {
            selectedSubcategory = _subcategoryToSpawn;
            elementsContent.DestroyChildren();
            elementsContent.DetachChildren();
        }

        /// <summary>
        /// Checks if spawner is able to instantiate elements
        /// </summary>
        /// <param name="_subcategoryToSpawn"></param>
        /// <returns>Whether spawner is able to spawn or not</returns>
        protected bool CanSpawn(TEnum _subcategoryToSpawn) =>
            !EqualityComparer<TEnum>.Default.Equals(_subcategoryToSpawn, selectedSubcategory);

        /// <summary>
        /// Forces spawner to instantiate current selected subcategory's elements
        /// </summary>
        public virtual void ForceSpawn()
        {
            SpawnElements(selectedSubcategory);
        }

        /// <summary>
        /// Cancels asynchronous spawning
        /// </summary>
        public virtual void CancelAsyncSpawner()
        {
            
        }

        /// <summary>
        /// Forces spawner to instantiate passed subcategory's elements
        /// </summary>
        /// <param name="_forcedSubcategory"></param>
        public virtual void ForceSpawn(TEnum _forcedSubcategory)
        {
            SpawnElements(_forcedSubcategory);
        }
    }
}