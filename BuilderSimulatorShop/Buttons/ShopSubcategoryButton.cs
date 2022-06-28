using DG.Tweening;
using Languages;
using TMPro;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Buttons
{
    /// <summary>
    /// Abstract class derrives from button, spawning elements to buy by specific shop
    /// </summary>
    /// <typeparam name="T">Enum parameter defines type of elements to spawn</typeparam>
    public abstract class ShopSubcategoryButton<T> : Button, IShopSubcategorySelector where T: System.Enum
    {
        [SerializeField] protected T subcategory;
        [SerializeField] protected TextMeshProUGUI subcategoryTMP;
        [SerializeField] protected Image tutorialHighlight;
        [SerializeField] private Material selectedMaterial;
        private Material originalMaterial;

        protected override void Awake()
        {
            base.Awake();
            image = GetComponentInChildren<Image>();
            originalMaterial = image.material;
            LanguageSupport.OnTextRefreshOnScene += Translate;
            Translate();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            LanguageSupport.OnTextRefreshOnScene -= Translate;
        }

        public override void OnPointerClick(PointerEventData _eventData)
        {
            if(!interactable) return;
            base.OnPointerClick(_eventData);
            DoSpawn();
        }

        /// <summary>
        /// Spawns elements to buy in shop
        /// </summary>
        protected virtual void DoSpawn()
        {
            
        }

        protected virtual void Translate()
        {
            
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            transform.DOScale(Vector3.one * 1.05f, 0.2f);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            transform.DOScale(Vector3.one, 0.2f);
        }

        public void SelectButton()
        {
            image.material = selectedMaterial;
            transform.DOScale(Vector3.one * 1.05f, 0.2f);
        }

        public void DeselectButton()
        {
            image.material = originalMaterial;
            transform.DOScale(Vector3.one, 0.2f);
        }

        public void InstantDeselect()
        {
            
        }
    }
}