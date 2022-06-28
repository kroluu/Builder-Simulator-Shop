using Languages;
using TMPro;
using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Area
{
    /// <summary>
    /// Abstract class for holding information about shop filter
    /// </summary>
    public abstract class ShopArea : MonoBehaviour, IShopAreaInfo, IShopAreaInspector
    {
        public Sprite Preview { get; set; }
        public string Name { get; set; }
        public string TranslateKey { get; set; }
        [field: SerializeField] public TextMeshProUGUI NameTMP { get; set; }
        [field: SerializeField] public Image PreviewImage { get; set; }

        private void Awake()
        {
            LanguageSupport.OnTextRefreshOnScene += Translate;
        }

        private void OnDestroy()
        {
            LanguageSupport.OnTextRefreshOnScene -= Translate;
        }

        /// <summary>
        /// Assigns value for shop filter
        /// </summary>
        /// <param name="_name">Filter name</param>
        /// <param name="_translateKey">Filter translate key name</param>
        /// <param name="_preview">Filter UI preview</param>
        public void AssignToArea(string _name, string _translateKey, Sprite _preview)
        {
            Name = _name;
            TranslateKey = _translateKey;
            Preview = _preview;
            if(_preview is {})
                PreviewImage.sprite = Preview;
            Translate();
            InjectTypeToButton();
        }

        protected abstract void InjectTypeToButton();

        /// <summary>
        /// Translates shop name by current selected language
        /// </summary>
        protected void Translate()
        {
            NameTMP.text = LanguageSupport.GetStringFromDictionary(TranslateKey);
        }
    }
}
