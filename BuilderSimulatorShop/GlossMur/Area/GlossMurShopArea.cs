using UI.Game.ReworkTablet.Area;
using UI.Game.ReworkTablet.Buttons;

namespace UI.Game.ReworkTablet.GlossMur.Area
{
    public class GlossMurShopArea : ShopArea
    {
        protected override void InjectTypeToButton()
        {
            if (GetComponentInChildren<ShopAreaButton<string>>() is { } areaButton)
            {
                areaButton.AssignName(Name);
            }
        }
    }
}
