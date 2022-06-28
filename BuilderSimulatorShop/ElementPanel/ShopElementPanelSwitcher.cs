using System;
using System.Collections.Generic;
using Core.Helpers;
using Languages;
using TMPro;
using Tools.Translations;
using UnityEngine;

namespace UI.Game.ReworkTablet.ElementPanel
{
    /// <summary>
    /// Abstract class behavior for handling switching panel to buy/sell
    /// </summary>
    /// <typeparam name="TCategory">Enum parameter specifies type of shop</typeparam>
    public abstract class ShopElementPanelSwitcher<TCategory> : MonoBehaviour where TCategory : Enum
    {
        [SerializeField] private TextMeshProUGUI switchButtonNameTMP;
        [SerializeField] private TextTranslator switchButtonNameTranslator;
        
        [SerializeField] protected GameObject[] buyPanel;
        [SerializeField] protected GameObject[] sellPanel;
        protected readonly Dictionary<TCategory, ShopElementPanelEnabler> PANELS_BY_TYPE = new Dictionary<TCategory, ShopElementPanelEnabler>(2);
        protected TCategory selectedPanel;
        private static readonly string SWITCH_BUTTON_TRANSLATE_PREFIX = "Tablet_Contorama_Panel_";
        public TCategory GetSelectedPanel => selectedPanel;
        protected abstract void Bind<TInherit>(TInherit _instance) where TInherit : ShopElementPanelSwitcher<TCategory>;
        
        /// <summary>
        /// Tries to switch shop panel view
        /// </summary>
        /// <param name="_panelToSet">Shop type</param>
        /// <param name="_withSpawn">Wheter shop elements should be spawned or not</param>
        /// <returns>Was panels changed</returns>
        public bool TryChangePanel(TCategory _panelToSet, bool _withSpawn = true)
        {
            if (EqualityComparer<TCategory>.Default.Equals(selectedPanel, _panelToSet)) return false;
            
            ChangePanel(_panelToSet, _withSpawn);
            return true;
        }
        
        public void ChangePanel(bool _withSpawn = true)
        {
            selectedPanel = selectedPanel.NextEnum();
            SetPanelsVisibility(_withSpawn);
        }

        private void ChangePanel(TCategory _panelToSet, bool _withSpawn = true)
        {
            selectedPanel = _panelToSet;
            SetPanelsVisibility(_withSpawn);
        }
        
        /// <summary>
        /// Sets panels visibility
        /// </summary>
        /// <param name="_withSpawn">Wheter shop elements should be spawned or not</param>
        private void SetPanelsVisibility(bool _withSpawn)
        {
            TCategory[] types = (TCategory[])Enum.GetValues(typeof(TCategory));
            for (int i = 0; i < PANELS_BY_TYPE.Count; i++)
            {
                PANELS_BY_TYPE[types[i]].SetVisibility(EqualityComparer<TCategory>.Default.Equals(types[i], selectedPanel), _withSpawn);
            }

            TCategory nextType = selectedPanel.NextEnum();
            switchButtonNameTMP.text =
                LanguageSupport.GetStringFromDictionary($"{SWITCH_BUTTON_TRANSLATE_PREFIX}{nextType.FastString()}");
            switchButtonNameTranslator.SetKey($"{SWITCH_BUTTON_TRANSLATE_PREFIX}{nextType.FastString()}",false);
        }
    }
}
