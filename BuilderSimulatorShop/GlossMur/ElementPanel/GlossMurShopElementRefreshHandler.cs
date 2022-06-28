using System;

namespace UI.Game.ReworkTablet.GlossMur.ElementPanel
{
    public static class GlossMurShopElementRefreshHandler
    {
        public static event Action OnRefreshElement;
        public static event Action OnSellPanelPickupRefresh;
        public static void PublishOnRefreshElement()
        {
            OnRefreshElement?.Invoke();
        }

        public static void PublishOnSellPanelPickupRefresh()
        {
            OnSellPanelPickupRefresh?.Invoke();
        }
    }
}
