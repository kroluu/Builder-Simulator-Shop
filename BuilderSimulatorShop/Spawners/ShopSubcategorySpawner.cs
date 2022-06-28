using System;
using System.Collections.Generic;
using Core.Helpers;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;

namespace UI.Game.ReworkTablet.Spawners
{
    public abstract class ShopSubcategorySpawner<TCategory,TSubcategory> : MonoBehaviour where TCategory : Enum where TSubcategory: Enum
    {
        private event Action<TCategory> OnSubcategorySpawn;
        [SerializeField] private Transform subcategoryPrefab;
        [SerializeField] private Transform subcategoryContent;
        protected TCategory selectedCategory;
        
        protected virtual void Awake()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
        
        public void PublishOnSubcategorySpawn(TCategory _categoryToSpawn)
        {
            OnSubcategorySpawn?.Invoke(_categoryToSpawn);
        }

        private void Subscribe()
        {
            OnSubcategorySpawn += SpawnSubcategories;
        }

        private void Unsubscribe()
        {
            OnSubcategorySpawn -= SpawnSubcategories;
        }

        protected abstract void Bind<TInherit>(TInherit _instance) where TInherit : ShopSubcategorySpawner<TCategory,TSubcategory>;

        protected void SpawnSubcategories(TCategory _categoryToSpawn)
        {
            if(EqualityComparer<TCategory>.Default.Equals(_categoryToSpawn, selectedCategory)) return;
            
            selectedCategory = _categoryToSpawn;
            subcategoryContent.DestroyChildren();
            TSubcategory[] subcategories = (TSubcategory[]) Enum.GetValues(typeof(TSubcategory));
            for (int i = 0; i < subcategories.Length; i++)
            {
                Transform subcategory = Instantiate(subcategoryPrefab, subcategoryContent);
                if (subcategory.GetComponentInChildren<IShopSubcategoryAssigner<TSubcategory>>() is { } assigner)
                {
                    assigner.AssignSubcategory(subcategories[i]);
                }
            }
        }
    }
}