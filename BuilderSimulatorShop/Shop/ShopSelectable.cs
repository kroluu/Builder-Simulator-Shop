using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Shop
{
    /// <summary>
    /// Abstract class for gamepad select behaviour
    /// </summary>
    /// <typeparam name="T">Type of specified shop derrives from ShopElement</typeparam>
    public abstract class ShopSelectable<T> : Selectable where T: ShopElement
    {
        [SerializeField] private List<GameObject> prompts = new List<GameObject>();
        protected bool isSelected;
        protected T element;
        protected RectTransform elementRect;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Image highlightBorder;
        
        protected override void Awake()
        {
            TryGetComponent(out element);
            TryGetComponent(out elementRect);
            SetPromptsState(false);
            base.Awake();
        }

        /// <summary>
        /// Sets prompt states visibility
        /// </summary>
        /// <param name="_isOn"></param>
        private void SetPromptsState(bool _isOn)
        {
            if(!Application.isPlaying) return;
            for (int i = 0; i < prompts.Count; i++)
            {
                prompts[i].SetActive(_isOn);
            }
            if(!_isOn ||InputDeviceDetector.Instance.CurrentDevice == InputDeviceDetector.DeviceType.Controller)
                highlightBorder.material = _isOn ? highlightMaterial : normalMaterial;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            SetPromptsState(false);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SetPromptsState(true);
        }
    }
}