using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Buttons
{
    /// <summary>
    /// Abstract class for spawning specific subcategories based on passed shop
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ShopCategoryButton<T> : Button where T: System.Enum
    {
        [SerializeField] protected T category;
        
        public override void OnPointerClick(PointerEventData _eventData)
        {
            base.OnPointerClick(_eventData);
            DoSpawn();
        }

        protected virtual void DoSpawn()
        {
            //General stuff WIP
        }
    }
}
