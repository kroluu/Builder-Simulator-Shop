using System;
using Core.Managers;
using Languages;
using UI.Game.ReworkTablet.BuilderShop.ElementPanel;
using UI.Game.ReworkTablet.Shop;
using UnityEngine;

namespace UI.Game.ReworkTablet.BuilderShop.Shop
{
    /// <summary>
    /// Build24 shop element class for storing information about construction object
    /// </summary>
    public abstract class BuilderShopElement : ShopElement
    {
        protected ConstructionObjectID constructionObjectID;

        protected override void Awake()
        {
            BuilderShopElementRefreshHandler.OnRefreshElement += RefreshValues;
            base.Awake();
            if(SecondBehaviourButton)
                SecondBehaviourButton.onClick.AddListener(OnSecondButtonBehaviour);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            BuilderShopElementRefreshHandler.OnRefreshElement -= RefreshValues;
        }

        /// <summary>
        /// Refreshes element quantity
        /// </summary>
        public override void RefreshValues()
        {
            Quantity = ScenesCommunicator.GetGameData.equipmentData.GetConstructionItemAmount(constructionObjectID);
            base.RefreshValues();
        }

        /// <summary>
        /// Assigns construction object info to fields
        /// </summary>
        /// <param name="_cost">Element cost</param>
        /// <param name="_name">Element name</param>
        /// <param name="_translateKey">Element translate key name</param>
        /// <param name="_quantity">Element quantity</param>
        /// <param name="_preview">Element UI preview</param>
        public override void AssignValuesToElement(int _cost, string _name, string _translateKey, int _quantity, Sprite _preview)
        {
            if (Enum.TryParse(_name, out ConstructionObjectID id))
            {
                constructionObjectID = id;
            }
            else
            {
                Debug.LogError($"Error during parsing builder shop element name to {nameof(ConstructionObjectID)}");
            }

            Preview.sprite = AssetsManager.Instance.inventoryContainer.GetConstructionSprite(constructionObjectID)
                .constructionView;
            base.AssignValuesToElement(_cost, _name, _translateKey, _quantity, _preview);
        }

        /// <summary>
        /// Translates construction object name to specific language selected in option
        /// </summary>
        protected override void Translate()
        {
            base.Translate();
            NameTMP.text = LanguageSupport.GetStringFromDictionary(TranslateKey);
        }
        
        protected abstract void SetInteractionPermission(ref ConstructionObjectID[] _constructionsAllowedToInteract);
    }
}
