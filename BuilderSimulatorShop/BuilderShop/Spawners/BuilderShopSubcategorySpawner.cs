using UI.Game.ReworkTablet.BuilderShop.Enums;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Spawners;

namespace UI.Game.ReworkTablet.BuilderShop.Spawners
{
    public class BuilderShopSubcategorySpawner : ShopSubcategorySpawner<BuilderShopCategory,ConstructionObjectCategory>
    {
        protected override void Awake()
        {
            //For testing purpose
            selectedCategory = BuilderShopCategory.Finish;
            Bind(this);
            base.Awake();
        }

        private void Start()
        {
            //For testing purpose
            SpawnSubcategories(BuilderShopCategory.Construction);
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }
    }
}