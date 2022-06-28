using System;
using System.Collections;
using Core.Events;
using Core.Managers;
using DG.Tweening;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Enums;
using UI.Game.ReworkTablet.GlossMur.Buttons;
using UI.Game.ReworkTablet.GlossMur.ElementPanel;
using UI.Game.ReworkTablet.GlossMur.Spawners;
using UI.Game.Tablet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.GlossMur.Gamepad
{
    public class GlossMurShopGamepadInputHandler : MonoBehaviour
    {
        [SerializeField] private GlossMurSubcategoryButton defaultSubcategorySelection;
        [SerializeField] private Transform buyElementContent;
        [SerializeField] private Transform sellElementContent;
        [SerializeField] private ScrollRect buyScrollRect;
        [SerializeField] private ScrollRect sellScrollRect;
        [SerializeField] private GlossMurShopElementPanelButton panelSwitchButton;
        [SerializeField] private GridLayoutGroup buyGridLayoutGroup;
        [SerializeField] private GridLayoutGroup sellGridLayoutGroup;
        private static RectTransform ScrollViewRectTransform;
        private static GlossMurShopElementPanelButton PanelSwitchButton;
        private static ScrollRect ScrollRect;
        private static int CurrentSelected;
        private static int ScrollRectTopOffset;
        private static float ScrollRectSpacing;
        private static bool ControllerEnabled;
        private static bool TabletEnabled;
        private static event Action OnChangeScrollRect;
        private static event Action OnRefreshElementSelect;
        private static readonly GameObject[] OBJECTS_TO_SELECT = new GameObject[2];
        private static Vector2 PreviousElementAnchor = Vector2.zero;

        public static void PublishOnRefreshElementSelect()
        {
            OnRefreshElementSelect?.Invoke();
        }
        
        public static void SelectSubcategory(GameObject _subcategoryToSelect)
        {
            OBJECTS_TO_SELECT[0] = _subcategoryToSelect;
        }
        
        public static void SelectElement(int _sign)
        {
            int nextSelection = Mathf.Clamp(CurrentSelected + _sign, 0, OBJECTS_TO_SELECT.Length-1);
            if(OBJECTS_TO_SELECT[nextSelection] is null) return;
            CurrentSelected = nextSelection;
            EventSystem.current.SetSelectedGameObject(OBJECTS_TO_SELECT[CurrentSelected]);
        }
        
        public static void AssignContentElement(GameObject _contentElement, bool _withSelection=false)
        {
            OBJECTS_TO_SELECT[1] = _contentElement;
            if (_withSelection)
            {
                PreviousElementAnchor = Vector2.zero;
                SelectElement(1);
            }
            
        }
        
        public static void GlossMurUpdateInputHandle()
        {
            if(!ControllerEnabled || !TabletEnabled) return;
            if (InputManager.Instance.inputActions.UI.MainAction.WasPressedThisFrame() && CurrentSelected == 0)
            {
                if(OBJECTS_TO_SELECT[0].TryGetComponent(out GlossMurSubcategoryButton button))
                {
                    button.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }
                
            if (InputManager.Instance.inputActions.UI.Shoulder_R.WasPressedThisFrame() && OBJECTS_TO_SELECT[1] is {})
            {
                SelectElement(1);
            }
            else if (InputManager.Instance.inputActions.UI.Shoulder_L.WasPressedThisFrame())
            {
                SelectElement(-1);
            }

            if (InputManager.Instance.inputActions.UI.SecondaryAction2.WasPressedThisFrame() && PanelSwitchButton.interactable)
            {
                PanelSwitchButton.OnPointerClick(new PointerEventData(EventSystem.current));
                OnChangeScrollRect?.Invoke();
            }

            ScrollRect.content.anchoredPosition = InputManager.Instance.GetGamepadVerticalScrollValue(ScrollRect.content.anchoredPosition);
        }
        
        private void Awake()
        {
            OnChangeScrollRect += SetScrollRect;
            OnRefreshElementSelect += RefreshSelectedSellElement;
            InputDeviceDetector.OnDeviceChange += DeviceChange;
            GameEvents.UIEvents.OnTabletGamepadEnable += SetControllerState;
            ControllerEnabled = false;
            TabletEnabled = false;
            PanelSwitchButton = panelSwitchButton;
            ScrollRect = buyScrollRect;
            ScrollRectTopOffset = buyGridLayoutGroup.padding.top;
            ScrollRectSpacing = buyGridLayoutGroup.spacing.y;
            OBJECTS_TO_SELECT[0] = defaultSubcategorySelection.gameObject;
            ScrollRect.transform.TryGetComponent(out ScrollViewRectTransform);
            foreach (Transform child in buyElementContent)
            {
                if (child.TryGetComponent(out OBJECTS_TO_SELECT[1]))
                {
                    return;
                }
            }
        }

        private void Start()
        {
            ControllerEnabled = InputDeviceDetector.GamepadActive;
        }

        private void OnDestroy()
        {
            OnChangeScrollRect -= SetScrollRect;
            InputDeviceDetector.OnDeviceChange -= DeviceChange;
            OnRefreshElementSelect -= RefreshSelectedSellElement;
            GameEvents.UIEvents.OnTabletGamepadEnable -= SetControllerState;
        }
        
        private void RefreshSelectedSellElement()
        {
            StartCoroutine(WaitForRefreshContent());
        }
        
        private void SetScrollRect()
        {
            bool sellContent = sellElementContent.gameObject.activeInHierarchy;
            ScrollRect = sellContent ? sellScrollRect : buyScrollRect;
            ScrollRectSpacing = sellContent ? sellGridLayoutGroup.spacing.y : buyGridLayoutGroup.spacing.y;
            ScrollRectTopOffset = sellContent ? sellGridLayoutGroup.padding.top : buyGridLayoutGroup.padding.top;
        }
        
        public void DefaultSelect()
        {
            EventSystem.current.SetSelectedGameObject(OBJECTS_TO_SELECT[CurrentSelected]);
        }
        
        private void DeviceChange(InputDeviceDetector.DeviceType _type)
        {
            ControllerEnabled = _type == InputDeviceDetector.DeviceType.Controller;
            if(!ControllerEnabled || !TabletEnabled) return;
            EventSystem.current.SetSelectedGameObject(OBJECTS_TO_SELECT[CurrentSelected]);
        }
        
        private IEnumerator WaitForRefreshContent()
        {
            yield return new WaitForEndOfFrame();
            if(sellElementContent.childCount <= 0) yield break;
            EventSystem.current.SetSelectedGameObject(sellElementContent.GetChild(0).gameObject);
            ShopElementPanelType currentPanelType =
                TabletContainer.Instance.Resolve<GlossMurShopElementPanelSwitcher>().GetSelectedPanel;
            switch (currentPanelType)
            {
                case ShopElementPanelType.Buy:
                    GlossMurShopSelectableGenerator.GenerateByTransform(sellElementContent);
                    break;
                case ShopElementPanelType.Sell:
                    GlossMurShopSellSelectableGenerator.GenerateByTransform(sellElementContent);
                    break;
                
            }
            GlossMurShopSelectableGenerator.GenerateByTransform(sellElementContent);
        }

        private void SetControllerState(bool _enable)
        {
            TabletEnabled = _enable && TabletStateMachine.Instance.CurrentState.GetState == TabletMachineState.GlossWall;
        }

        public static void ScrollToElement(RectTransform _element)
        {
            if(OBJECTS_TO_SELECT[1] is null) return;
            Canvas.ForceUpdateCanvases();
            
            Vector2 snapPosition = (Vector2)ScrollRect.transform.InverseTransformPoint(ScrollRect.content.position)
                                   - (Vector2)ScrollRect.transform.InverseTransformPoint(_element.position);
            float maxAnchorY = Mathf.Min(snapPosition.y - ScrollRectTopOffset,
                ScrollRect.content.sizeDelta.y - ScrollViewRectTransform.sizeDelta.y);
            Vector2 snapWithOffset = new Vector2(snapPosition.x, maxAnchorY);
            if (Mathf.Abs(PreviousElementAnchor.y - snapWithOffset.y) > (_element.sizeDelta.y + ScrollRectSpacing)*1.1f)
            {
                snapWithOffset.x = ScrollRect.content.anchoredPosition.x;
                ScrollRect.content.anchoredPosition = snapWithOffset;
                PreviousElementAnchor = snapWithOffset;
                return;
            }
            ScrollRect.content.DOAnchorPosY(snapWithOffset.y, 0.5f);
            PreviousElementAnchor = snapWithOffset;
        }
    }
}
