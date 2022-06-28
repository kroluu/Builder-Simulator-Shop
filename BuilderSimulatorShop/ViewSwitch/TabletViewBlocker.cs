using System.Collections;
using Core.Events;
using Core.Managers;
using UI.Game.ReworkTablet.Interfaces;
using UI.Game.Tablet;
using UnityEngine;

namespace UI.Game.ReworkTablet.ViewSwitch
{
    public class TabletViewBlocker : MonoBehaviour
    {
        private readonly ITabletViewBlock[] blocker = new ITabletViewBlock[2];
    
        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent(out blocker[i]);
            }
            GameEvents.OnBeginContructionStep += TryBlock;
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            TryBlock(GameSceneManager.Instance.ConstructionStagesManager.CurrentConstructionStep);
        }

        private void OnDestroy()
        {
            GameEvents.OnBeginContructionStep -= TryBlock;
        }

        private void TryBlock(ConstructionStep _step)
        {
            if(_step < ConstructionStep.InteriorFinishing) return;
            
            blocker[0].Block();
            
            blocker[1].Unblock();
            TabletStateMachine.Instance.stateMachine.Fire(TabletMachineTrigger.GlossWallTrigger);
            GameEvents.OnBeginContructionStep -= TryBlock;
        }
        
    }
}
