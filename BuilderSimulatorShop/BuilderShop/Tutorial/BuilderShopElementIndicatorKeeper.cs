using System;
using Core.Events;
using UI.Game.ReworkTablet.Container;
using UnityEngine;

namespace UI.Game.ReworkTablet.BuilderShop.Tutorial
{
    public class BuilderShopElementIndicatorKeeper : MonoBehaviour
    {
        private ConstructionObjectID[] objectsToIndicate = Array.Empty<ConstructionObjectID>();
        public static event Action OnSetPermission;

        private static void PublishOnSetPermission()
        {
            OnSetPermission?.Invoke();
        }
        private void Awake()
        {
            TabletContainer.Instance.Bind(this);
            GameEvents.UIEvents.OnEnableTabletItemsBuyPermission += SetObjectsToIndicate;
        }

        private void OnDestroy()
        {
            GameEvents.UIEvents.OnEnableTabletItemsBuyPermission -= SetObjectsToIndicate;
        }

        private void SetObjectsToIndicate(ConstructionObjectID[] _objectsToIndicate)
        {
            objectsToIndicate = _objectsToIndicate;
            PublishOnSetPermission();
        }

        public bool CanIndicate(ConstructionObjectID _objectToCheck)
        {
            for (int i = 0; i < objectsToIndicate.Length; i++)
            {
                if (objectsToIndicate[i] == _objectToCheck)
                    return true;
            }

            return false;
        }
    }
}
