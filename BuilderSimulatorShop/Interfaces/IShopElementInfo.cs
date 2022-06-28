using UnityEngine;

namespace UI.Game.ReworkTablet.Interfaces
{
    /// <summary>
    /// Shop element info
    /// </summary>
    public interface IShopElementInfo
    {
        int BuyCost { get; set; }
        int Quantity { get; set; }
        string Name { get; set; }
        string TranslateKey { get; set; }

        void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview);
    }
}
