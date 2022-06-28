using Languages;
using UI.Game.ReworkTablet.GlossMur.ElementPanel;
using UI.Game.ReworkTablet.Shop;

namespace UI.Game.ReworkTablet.GlossMur.Shop
{
    public abstract class GlossMurShopElement : ShopElement
    {
        protected override void Awake()
        {
            GlossMurShopElementRefreshHandler.OnRefreshElement += RefreshValues;
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GlossMurShopElementRefreshHandler.OnRefreshElement -= RefreshValues;
        }

        protected override void Translate()
        {
            base.Translate();
            NameTMP.text = LanguageSupport.GetStringFromDictionary(TranslateKey);
        }
    }
}
