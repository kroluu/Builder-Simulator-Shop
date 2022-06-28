using UI.Game.Tablet;

namespace UI.Game.ReworkTablet.Interfaces
{
    public interface ITabletViewInfo
    {
        TabletMachineTrigger Trigger { get; set; }

        public void SelectButton();
        public void DeselectButton();
    }
}