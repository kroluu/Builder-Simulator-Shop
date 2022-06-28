using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Interfaces
{
    /// <summary>
    /// Shop's elements tutorial fields
    /// </summary>
    public interface IShopElementTutorial
    {
        Image TutorialIndicator { get; set; }

        void Indicate();
        void SetPermission();
    }
}