using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Skills.Element
{
    public class SkillInfo : MonoBehaviour
    {
        [SerializeField] private Image skillView;
        [SerializeField] private Sprite unlockedSprite;
        [SerializeField] private Sprite selectedSprite;
        private static readonly Vector3 UNLOCKED_SIZE = new Vector3(0.8f, 0.8f, 0.8f);
        
        public void Unlock()
        {
            skillView.sprite = unlockedSprite;
            transform.localScale = Vector3.one;
        }
        
        public void Select()
        {
            skillView.sprite = selectedSprite;
            transform.localScale = Vector3.one;
        }
    }
}