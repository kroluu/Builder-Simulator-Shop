using Data.Furnishing;
using UI.Game.ReworkTablet.Interfaces;

namespace UI.Game.ReworkTablet.GlossMur.Interfaces
{
    public interface IShopFurnitureInfo
    {
        FurnitureRoomCategory Category { get; set; }
        string Subcategory { get; set; }
        FurnituresSet FurnituresSet { get; set; }
        void AssignFurniture(FurnitureRoomCategory _category, string _subcategory, FurnituresSet _furniture);
    }
}