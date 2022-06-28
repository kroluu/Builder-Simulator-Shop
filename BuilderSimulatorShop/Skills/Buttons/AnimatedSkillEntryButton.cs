using Core.Audio;
using DG.Tweening;
using UI.Game.Tablet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Skills.Buttons
{
    public class AnimatedSkillEntryButton : Button
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            transform.DOScale(Vector3.one * 1.1f, 0.25f);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            transform.DOScale(Vector3.one, 0.25f);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            TabletStateMachine.Instance.stateMachine.Fire(TabletMachineTrigger.SkillsTrigger);
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_SkillsEntry");
        }
    }
}
