using UI.Game.ReworkTablet.BuilderShop.Enums;
using UI.Game.ReworkTablet.BuilderShop.Spawners;
using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;

namespace UI.Game.ReworkTablet.BuilderShop.Buttons
{
    /// <summary>
    /// Build24 button for spawning subcategories included in category
    /// </summary>
    public class BuilderShopCategoryButton : ShopCategoryButton<BuilderShopCategory>
    {
        protected override void DoSpawn()
        {
            base.DoSpawn();
            if (TabletContainer.Instance.Resolve<BuilderShopSubcategorySpawner>() is { } spawner)
            {
                spawner.PublishOnSubcategorySpawn(category);
            }
        }
    }
}
