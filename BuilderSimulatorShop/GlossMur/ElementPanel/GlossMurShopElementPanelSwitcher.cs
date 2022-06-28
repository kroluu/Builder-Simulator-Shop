using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.ElementPanel;
using UI.Game.ReworkTablet.Enums;
using UI.Game.ReworkTablet.GlossMur.Spawners;

namespace UI.Game.ReworkTablet.GlossMur.ElementPanel
{
    public class GlossMurShopElementPanelSwitcher : ShopElementPanelSwitcher<ShopElementPanelType>
    {
        private void Awake()
        {
            Bind(this);
        }

        private void Start()
        {
            PANELS_BY_TYPE.Add(ShopElementPanelType.Buy,new ShopElementPanelEnabler(buyPanel,TabletContainer.Instance.Resolve<GlossMurShopBuyElementSpawner>()));
            PANELS_BY_TYPE.Add(ShopElementPanelType.Sell,new ShopElementPanelEnabler(sellPanel,TabletContainer.Instance.Resolve<GlossMurShopSellElementSpawner>()));
            PANELS_BY_TYPE[ShopElementPanelType.Sell].SetVisibility(false);
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }
    }
}
