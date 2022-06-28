using Core.Audio;
using Core.Helpers;
using Core.Managers;
using Equipment.Items.InHands;
using Gameplay.SkillsSystem;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.Skills.Element
{
    /// <summary>
    /// Temporary class, clickable skill system WIP after full release
    /// </summary>
    public class DiamondShovelSkillInfo : SkillInfo, IPointerClickHandler
    {
        private Shovel.ShovelType currentSelectedShovel = Shovel.ShovelType.Normal;
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectShovel();
        }

        private void SelectShovel()
        {
            UpgradesData upgradesData = ScenesCommunicator.GetGameData.PlayerProfile.upgradesData;
            currentSelectedShovel = currentSelectedShovel.NextEnum();
            switch (currentSelectedShovel)
            {
                case Shovel.ShovelType.Diamond:
                    Unlock();
                    break;
                case Shovel.ShovelType.Normal:
                    Select();
                    break;
            }

            Shovel.Type = currentSelectedShovel;
            upgradesData.selectedShovel = currentSelectedShovel;
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Skills_DiamondShovel");
        }
    }
}