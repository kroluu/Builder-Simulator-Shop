using UI.Game.ReworkTablet.Interfaces;
using UI.Game.Tablet;
using UnityEngine;

namespace UI.Game.ReworkTablet.ViewSwitch
{
    public class TabletViewKeeper : MonoBehaviour
    {
        private static ITabletViewInfo CurrentSelected;

        private void Awake()
        {
            CurrentSelected = transform.GetComponentInChildren<ITabletViewInfo>();
        }

        public static void SetView(ITabletViewInfo _viewToSet)
        {
            if(CurrentSelected == _viewToSet) return;
            CurrentSelected?.DeselectButton();
            CurrentSelected = _viewToSet;
            CurrentSelected.SelectButton();
            TabletStateMachine.Instance.stateMachine.Fire(CurrentSelected.Trigger);
        }
    }
}