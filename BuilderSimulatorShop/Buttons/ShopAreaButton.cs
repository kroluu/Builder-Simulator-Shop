using DG.Tweening;
using TMPro;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Buttons
{
    public class ShopAreaButton<T> : Button, IShopSubcategorySelector
    {
        protected T areaName;
        [SerializeField] private TextMeshProUGUI areaNameTMP;
        [SerializeField] private Image tutorialHighlight;
        
        public void AssignName(T _nameToAssign)
        {
            areaName = _nameToAssign;
            InstantDeselect();
        }
        public override void OnPointerClick(PointerEventData _eventData)
        {
            base.OnPointerClick(_eventData);
            DoSpawn();
        }

        protected virtual void DoSpawn()
        {
            //General stuff WIP
        }

        public void SelectButton()
        {
            areaNameTMP.DOFade(1f, 0.25f);
            image.enabled = true;
        }


        public void InstantDeselect()
        {
            Color color = areaNameTMP.color;
            color.a = 0f;
            areaNameTMP.color = color;

            image.enabled = false;
        }

        public void DeselectButton()
        {
            areaNameTMP.DOFade(0f, 0.25f);
            image.enabled = false;
        }

        protected void SetHighlight(bool _state)
        {
            if (_state)
            {
                tutorialHighlight.enabled = true;
                tutorialHighlight.DOFade(0f, 0f);
                tutorialHighlight.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                tutorialHighlight.DOKill();
                tutorialHighlight.enabled = false;
                tutorialHighlight.DOFade(0f, 0f);
            }
        }
    }
}