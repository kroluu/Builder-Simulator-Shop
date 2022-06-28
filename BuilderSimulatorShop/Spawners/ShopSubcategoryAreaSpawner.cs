using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Game.ReworkTablet.Spawners
{
    public abstract class ShopSubcategoryAreaSpawner<TEnum> : MonoBehaviour where TEnum: System.Enum
    {
        private event Action<TEnum> OnSubcategorySpawn;
        [SerializeField] protected Transform areaPrefab;
        [SerializeField] protected Transform areaContent;

        protected TEnum selectedSubcategory;

        public TEnum GetSelectedSubcategory => selectedSubcategory;

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
            OnSubcategorySpawn += TrySpawnElements;
        }

        private void Unsubscribe()
        {
            OnSubcategorySpawn -= TrySpawnElements;
        }

        public void PublishOnElementSpawn(TEnum _subcategoryToSpawn)
        {
            OnSubcategorySpawn?.Invoke(_subcategoryToSpawn);
        }
        
        protected abstract void Bind<TInherit>(TInherit _instance) where TInherit : ShopSubcategoryAreaSpawner<TEnum>;

        protected abstract void TrySpawnElements(TEnum _fromSubcategory); 
        protected abstract void SpawnAreas(TEnum _fromSubcategory);
        protected bool CanSpawn(TEnum _subcategoryToSpawn) =>
            !EqualityComparer<TEnum>.Default.Equals(_subcategoryToSpawn, selectedSubcategory);

        public void ForceSpawn(TEnum _forcedSubcategory)
        {
            SpawnAreas(_forcedSubcategory);
        }
    }
}
