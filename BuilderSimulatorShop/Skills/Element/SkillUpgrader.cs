using System.Collections.Generic;
using Core.Managers;
using Gameplay.SkillsSystem;
using Languages;
using Sirenix.OdinInspector;
using TMPro;
using UI.Game.ReworkTablet.Skills.Interfaces;
using UI.Game.ReworkTablet.Skills.Managers;
using UnityEngine;

namespace UI.Game.ReworkTablet.Skills.Element
{
    public class SkillUpgrader : MonoBehaviour, ISkillUpdate, ISkillInspector
    {
        [EnumPaging][SerializeField] private Skill skillType;
        [SerializeField] private List<SkillInfo> skills;
        
        [field: SerializeField] public TextMeshProUGUI SkillTitleTMP { get; set; }
        [field: SerializeField] public TextMeshProUGUI SkillStatisticTMP { get; set; }
        public void UpdateSkill(UpgradesData _upgradesData)
        {
            SkillData skill = _upgradesData.GetSkillRecordBySkillType(skillType);
            for (int i = 0; i < skills.Count; i++)
            {
                if(skill.currentLevel - i > 1)
                {
                    skills[i].Unlock();
                    continue;
                }
                if (skill.currentLevel - i == 1)
                {
                    skills[i].Select();
                    break;
                }
            }

            if (skill.currentLevel >= 3)
                SkillStatisticTMP.enabled = false;

            SkillTitleTMP.text = LanguageSupport.GetStringFromDictionary($"Tablet_Skills_{skillType}_Title");
            SkillStatisticTMP.text = string.Format(LanguageSupport.GetStringFromDictionary($"Tablet_Skills_{skillType}_Description"),
                $"{skill.currentExperience}/{skill.experienceToNextLevel}");
        }

        private void TranslateText()
        {
            SkillData skill =
                ScenesCommunicator.GetGameData.PlayerProfile.upgradesData.GetSkillRecordBySkillType(skillType);
            SkillTitleTMP.text = LanguageSupport.GetStringFromDictionary($"Tablet_Skills_{skillType}_Title");
            SkillStatisticTMP.text = string.Format(LanguageSupport.GetStringFromDictionary($"Tablet_Skills_{skillType}_Description"),
                $"{skill.currentExperience}/{skill.experienceToNextLevel}");
        }

        private void Awake()
        {
            SkillsManager.OnSkillsUpdate += UpdateSkill;
            LanguageSupport.OnTextRefreshOnScene += TranslateText;
        }

        private void OnDestroy()
        {
            SkillsManager.OnSkillsUpdate -= UpdateSkill;
            LanguageSupport.OnTextRefreshOnScene -= TranslateText;
        }
    }
}
