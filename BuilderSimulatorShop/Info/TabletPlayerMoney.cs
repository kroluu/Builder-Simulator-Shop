using System;
using System.Text.RegularExpressions;
using Core.Events;
using Core.Helpers;
using Core.Managers;
using Core.ScenesLogic;
using DG.Tweening;
using Languages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Info
{
    public class TabletPlayerMoney : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyTMP;
        private static readonly Regex REGEX_WHITESPACES = new Regex(@"\s+");
        private Tween cashTween;
        [SerializeField] private Image infiniteIMG;
        
                
        private void Start()
        {
            GameEvents.UIEvents.OnMoneyUpdate += UpdateMoney;
            UpdateMoney();
            DetectForSandboxMode();
        }

        private void OnDestroy()
        {
            GameEvents.UIEvents.OnMoneyUpdate -= UpdateMoney;
            /*LanguageSupport.OnTextRefreshOnScene -= TranslateText;*/
        }

        private void TranslateText()
        {
            moneyTMP.text = LanguageSupport.GetStringFromDictionary("Game_Sandbox_InfiniteMoney");
        }

        private void DetectForSandboxMode()
        {
            SceneLoadingHandler sceneLoadingHandler = ScenesCommunicator.Instance.sceneLoadingHandler;
            if(sceneLoadingHandler.SelectedGameMode != GameMode.Sandbox && sceneLoadingHandler.SelectedGameMode != GameMode.InteriorsOnly) return;
            GameEvents.UIEvents.OnMoneyUpdate -= UpdateMoney;
            cashTween?.Kill();
            /*LanguageSupport.OnTextRefreshOnScene += TranslateText;
            moneyTMP.text = LanguageSupport.GetStringFromDictionary("Game_Sandbox_InfiniteMoney");
            moneyTMP.enableAutoSizing = true;*/
            moneyTMP.gameObject.SetActive(false);
            infiniteIMG.gameObject.SetActive(true);
        }

        private void UpdateMoney()
        {
            int actualPlayerCash = ScenesCommunicator.GetGameData.equipmentData.PlayerCash;
            if (Int32.TryParse(RemoveWhitespaces(moneyTMP.text), out int convertedMoney))
            {
                cashTween?.Kill();
                cashTween = DOVirtual.Int(convertedMoney, actualPlayerCash, 0.5f, (_value) =>
                {
                    moneyTMP.text = _value.ToCashFormat();
                });
                return;
            }
            moneyTMP.text = actualPlayerCash.ToCashFormat();
        }
        
        private static string RemoveWhitespaces(string _input)
        {
            int len = _input.Length;
            char[] src = _input.ToCharArray();
            int dstIdx = 0;
            for (int i = 0; i < len; i++)
            {
                char ch = src[i];
                switch (ch)
                {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':
                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':
                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':
                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        continue;
                    default:
                        src[dstIdx++] = ch;
                        break;
                }
            }
            return new string(src, 0, dstIdx);
        }
            
    }
}
