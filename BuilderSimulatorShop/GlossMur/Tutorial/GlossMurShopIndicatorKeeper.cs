using System;
using Core.Events;
using Data.Furnishing;
using UI.Game.ReworkTablet.Container;
using UnityEngine;

namespace UI.Game.ReworkTablet.GlossMur.Tutorial
{
    public class GlossMurShopIndicatorKeeper : MonoBehaviour
    {
        public static string[] ObjectsToBuyIndicate = Array.Empty<string>();
        public static string[] AreasToIndicate = Array.Empty<string>();
        public static event Action OnSetPermission;
        
        private static void PublishOnSetPermission()
        {
            OnSetPermission?.Invoke();
        }
        private void Awake()
        {
            ObjectsToBuyIndicate = Array.Empty<string>();
            AreasToIndicate = Array.Empty<string>();
            TabletContainer.Instance.Bind(this);
            GameEvents.UIEvents.OnEnableTabletGlossMurItemsBuyPermission += SetObjectsToIndicate;
            GameEvents.UIEvents.OnHighlightTabletGlossMurSubcategory += SetPermissionForHighlight;
        }

        private void SetPermissionForHighlight(FurnitureRoomCategory? _category, string[] _areas)
        {
            AreasToIndicate = _areas;
        }

        private void OnDestroy()
        {
            GameEvents.UIEvents.OnEnableTabletGlossMurItemsBuyPermission -= SetObjectsToIndicate;
            GameEvents.UIEvents.OnHighlightTabletGlossMurSubcategory -= SetPermissionForHighlight;
        }

        private void SetObjectsToIndicate(string[] _objectsToIndicate)
        {
            ObjectsToBuyIndicate = _objectsToIndicate;
            PublishOnSetPermission();
        }

        public bool CanIndicate(string _objectToCheck)
        {
            for (int i = 0; i < ObjectsToBuyIndicate.Length; i++)
            {
                if (ObjectsToBuyIndicate[i] == _objectToCheck)
                    return true;
            }

            return false;
        }
    }
}
