using DG.Tweening;
using UI.Game.ReworkTablet.Interfaces;
using UI.Game.Tablet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.ViewSwitch
{
    public class TabletViewSwitchButton : Button, ITabletViewInfo,ITabletViewBlock
    {
        private static readonly Color NORMAL_COLOR = new Color(0.85f, 0.85f, 0.85f,1f);
        [SerializeField] private GameObject block;
        [SerializeField] private GameObject normal;
        
        
        [field: SerializeField] public TabletMachineTrigger Trigger { get; set; }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if(!interactable) return;
            TabletViewKeeper.SetView(this);
            SelectButton();
        }

        public void SelectButton()
        {
            image.DOColor(Color.white, 0.1f);
        }

        public void DeselectButton()
        {
            image.DOColor(NORMAL_COLOR, 0.1f);
        }

        public void Block()
        {
            block.SetActive(true);
            normal.SetActive(false);
            interactable = false;
            DeselectButton();
        }

        public void Unblock()
        {
            block.SetActive(false);
            normal.SetActive(true);
            interactable = true;
            SelectButton();
        }
    }
}
