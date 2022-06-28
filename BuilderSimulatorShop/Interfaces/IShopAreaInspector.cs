using TMPro;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Interfaces
{
    /// <summary>
    /// Shop inspector fields
    /// </summary>
    public interface IShopAreaInspector
    {
        TextMeshProUGUI NameTMP { get; set; }
        Image PreviewImage { get; set; }
    }
}