using UnityEngine;

namespace UI.Game.ReworkTablet.Interfaces
{
    /// <summary>
    /// Shop filter information
    /// </summary>
    public interface IShopAreaInfo
    {
        Sprite Preview { get; set; }
        string Name { get; set; }
        string TranslateKey { get; set; }

        void AssignToArea(string _name, string _translateKey, Sprite _preview);
    }
}