using System;
using Core.Managers;
using Gameplay.SkillsSystem;
using UnityEngine;

namespace UI.Game.ReworkTablet.Skills.Managers
{
    public sealed class SkillsManager : MonoBehaviour
    {
        private static UpgradesData upgradesData;
        public static event Action<UpgradesData> OnSkillsUpdate;

        public static void PublishOnSkillsUpdate()
        {
            OnSkillsUpdate?.Invoke(upgradesData);
        }
        private void Awake()
        {
            upgradesData = ScenesCommunicator.GetGameData.PlayerProfile.upgradesData;
        }
    }
}
