using Core.Audio;
using DG.Tweening;
using Languages;
using TMPro;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Shop
{
    /// <summary>
    /// Abstract class for storing information about buy/sell element in shop
    /// </summary>
    public abstract class ShopElement : MonoBehaviour, IShopElementInfo, IShopElementInspector
    {
        public int BuyCost { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string TranslateKey { get; set; }
        private static readonly Vector3 SCALED_ANIMATION_VECTOR = new Vector3(0.9f, 0.9f, 0.9f);
        [field: SerializeField] public Image Preview { get; set; }
        [field: SerializeField] public Button MainBehaviourButton { get; set; }
        [field: SerializeField] public TextMeshProUGUI NameTMP { get; set; }
        [field: SerializeField] public TextMeshProUGUI QuantityTMP { get; set; }
        [field: SerializeField] public TextMeshProUGUI CostTMP { get; set; }
        [field: SerializeField] public Button SecondBehaviourButton { get; set; }
        [SerializeField] protected TextMeshProUGUI secondBehaviourTMP;
        
        /// <summary>
        /// Refreshes element quantity
        /// </summary>
        public virtual void RefreshValues()
        {
            QuantityTMP.text = $"{Quantity} {LanguageSupport.GetStringFromDictionary("Game_Tablet_Quantity")}";
        }

        protected virtual void Awake()
        {
            if(MainBehaviourButton) 
                MainBehaviourButton.onClick.AddListener(OnMainButtonBehaviour);
        }

        protected virtual void Start()
        {
            FireAnimation(true);
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected abstract void OnMainButtonBehaviour();
        protected abstract void OnSecondButtonBehaviour();

        /// <summary>
        /// Assigns element info to fields
        /// </summary>
        /// <param name="_cost">Element cost</param>
        /// <param name="_name">Element name</param>
        /// <param name="_translateKey">Element translate key name</param>
        /// <param name="_quantity">Element quantity</param>
        /// <param name="_preview">Element UI preview</param>
        public virtual void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview)
        {
            BuyCost = _cost;
            Name = _name;
            TranslateKey = _translateKey;
            Quantity = _quantity;
            Preview.sprite = _preview;
            Translate();
            RefreshValues();
        }

        /// <summary>
        /// Translates quantity and refreshes cost
        /// </summary>
        protected virtual void Translate()
        {
            QuantityTMP.text = $"{Quantity} {LanguageSupport.GetStringFromDictionary("Game_Tablet_Quantity")}";
            CostTMP.text = $"${BuyCost}";
        }
        
        /// <summary>
        /// Starts appearing element in UI
        /// </summary>
        /// <param name="_state"></param>
        private void FireAnimation(bool _state)
        {
            transform.localScale = _state ? Vector3.zero : Vector3.one;
            transform.DOScale(_state ? Vector3.one : Vector3.zero, 0.25f).SetEase(Ease.OutBack);
        }

        protected void MainButtonSound()
        {
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Buy");
        }

        protected virtual void SecondButtonSound()
        {
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_Sell");
        }

        protected void MainButtonClickAnimation()
        {
            MainBehaviourButton.transform.DOKill();
            MainBehaviourButton.transform.DOScale(SCALED_ANIMATION_VECTOR, 0.1f).OnComplete(() =>
            {
                MainBehaviourButton.transform.DOScale(Vector3.one, 0.1f);
            });
        }
        
        protected void SecondButtonClickAnimation()
        {
            SecondBehaviourButton.transform.DOKill();
            SecondBehaviourButton.transform.DOScale(SCALED_ANIMATION_VECTOR, 0.1f).OnComplete(() =>
            {
                SecondBehaviourButton.transform.DOScale(Vector3.one, 0.1f);
            });
        }
    }
}
