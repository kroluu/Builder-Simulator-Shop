using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.Enums;
using UI.Game.ReworkTablet.GlossMur.Spawners;

namespace UI.Game.ReworkTablet.GlossMur.Buttons
{
    /// <summary>
    /// GlossMur button for spawning subcategories included in category
    /// </summary>
    public class GlossMurCategoryButton : ShopCategoryButton<GlossMurShopCategory>
    {
        protected override void DoSpawn()
        {
            base.DoSpawn();
            if (TabletContainer.Instance.Resolve<GlossMurShopSubcategorySpawner>() is { } spawner)
            {
                spawner.PublishOnSubcategorySpawn(category);
            }
        }
    }
}
