using TMPro;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Interfaces
{
    /// <summary>
    /// Shop element inspector fields
    /// </summary>
    public interface IShopElementInspector
    {
        Image Preview { get; set; }
        Button MainBehaviourButton { get; set; }
        TextMeshProUGUI NameTMP { get; set; }
        TextMeshProUGUI QuantityTMP { get; set; }
        TextMeshProUGUI CostTMP { get; set; }
        Button SecondBehaviourButton { get; set; }
        
        void RefreshValues();
    }
}