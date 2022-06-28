using System;

namespace UI.Game.ReworkTablet.Interfaces
{
    /// <summary>
    /// Interface for assigning specific shop subcategory
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public interface IShopSubcategoryAssigner<in TEnum>
    {
        void AssignSubcategory(TEnum _subcategory);
    }
}