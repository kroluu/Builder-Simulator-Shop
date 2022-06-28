using System;

namespace UI.Game.ReworkTablet.BuilderShop.ElementPanel
{
    /// <summary>
    /// Build24 class for refreshing spawned elements while opening shop
    /// </summary>
    public static class BuilderShopElementRefreshHandler
    {
        public static event Action OnRefreshElement;

        public static void PublishOnRefreshElement()
        {
            OnRefreshElement?.Invoke();
        }
    }
}
