using Core.Audio;
using UI.Game.ReworkTablet.BuilderShop.ElementPanel;
using UI.Game.ReworkTablet.Buttons;
using UI.Game.ReworkTablet.Container;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.BuilderShop.Buttons
{
    /// <summary>
    /// Button for switching panels to buy/sell in Build24 shop
    /// </summary>
    public class BuilderShopElementPanelButton : ShopElementPanelButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            TabletContainer.Instance.Resolve<BuilderShopElementPanelSwitcher>().ChangePanel();
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "Button_Reject_Click");
        }
    }
}
