using Core.Audio;
using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.GlossMur.ElementPanel;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Buttons
{
    /// <summary>
    /// Button for switching panels to buy/sell in Build24 shop
    /// </summary>
    public class GlossMurShopElementPanelButton : ShopElementPanelButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            TabletContainer.Instance.Resolve<GlossMurShopElementPanelSwitcher>().ChangePanel();
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_SellBuy");
        }
    }
}
