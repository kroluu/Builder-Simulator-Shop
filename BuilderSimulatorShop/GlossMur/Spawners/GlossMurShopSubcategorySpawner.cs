using Data.Furnishing;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Enums;
using UI.Game.ReworkTablet.Spawners;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    public class GlossMurShopSubcategorySpawner : ShopSubcategorySpawner<GlossMurShopCategory,FurnitureRoomCategory>
    {
        protected override void Awake()
        {
            selectedCategory = GlossMurShopCategory.Finish;
            Bind(this);
            base.Awake();
        }
        
        private void Start()
        {
            //For testing purpose
            SpawnSubcategories(GlossMurShopCategory.Garden);
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }
    }
}
