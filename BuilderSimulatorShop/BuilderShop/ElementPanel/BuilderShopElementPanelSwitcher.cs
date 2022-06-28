using UI.Game.ReworkTablet.BuilderShop.Spawners;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.ElementPanel;
using UI.Game.ReworkTablet.Enums;

namespace UI.Game.ReworkTablet.BuilderShop.ElementPanel
{
    public class BuilderShopElementPanelSwitcher : ShopElementPanelSwitcher<ShopElementPanelType>
    {
        private void Awake()
        {
            Bind(this);
        }

        private void Start()
        {
            PANELS_BY_TYPE.Add(ShopElementPanelType.Buy,new ShopElementPanelEnabler(buyPanel,TabletContainer.Instance.Resolve<BuilderShopBuyElementSpawner>()));
            PANELS_BY_TYPE.Add(ShopElementPanelType.Sell,new ShopElementPanelEnabler(sellPanel,TabletContainer.Instance.Resolve<BuilderShopSellElementSpawner>()));
            PANELS_BY_TYPE[ShopElementPanelType.Sell].SetVisibility(false);
        }

        protected override void Bind<TInherit>(TInherit _instance)
        {
            TabletContainer.Instance.Bind(_instance);
        }
    }
}
