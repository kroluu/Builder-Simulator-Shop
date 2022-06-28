using DG.Tweening;
using TMPro;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Enums;
using UnityEngine;

namespace UI.Game.ReworkTablet.Popups
{
    public class TabletPopupManager : MonoBehaviour
    {
        public static bool IsFiring;
        [SerializeField] private TextMeshProUGUI equipmentFullTMP;
        
        private void Awake()
        {
            TabletContainer.Instance.Bind(this);
        }

        private void OnDestroy()
        {
            IsFiring = false;
        }

        public void Fire(TabletPopupType _popupType)
        {
            IsFiring = true;
            switch (_popupType)
            {
                case TabletPopupType.EquipmentFull:
                    EquipmentFullPopup();
                    return;
            }
        }

        private void EquipmentFullPopup()
        {
            equipmentFullTMP.DOFade(1f, 0.25f).OnComplete(() =>
            {
                equipmentFullTMP.DOFade(0f, 0.25f).SetDelay(0.4f).OnComplete(()=>IsFiring = false);
            });
        }
    }
}
