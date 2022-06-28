using System;
using System.Collections;
using Core.Events;
using Core.Managers;
using DG.Tweening;
using UI.Game.ReworkTablet.BuilderShop.Buttons;
using UI.Game.ReworkTablet.BuilderShop.Spawners;
using UI.Game.Tablet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.BuilderShop.Gamepad
{
    /// <summary>
    /// Build24 class for handling gamepad behaviour
    /// </summary>
    public class BuilderShopGamepadInputHandler : MonoBehaviour
    {
        [SerializeField] private BuilderShopSubcategoryButton defaultSubcategorySelection;
        [SerializeField] private Transform buyElementContent;
        [SerializeField] private Transform sellElementContent;
        [SerializeField] private ScrollRect buyScrollRect;
        [SerializeField] private ScrollRect sellScrollRect;
        [SerializeField] private BuilderShopElementPanelButton panelSwitchButton;
        [SerializeField] private GridLayoutGroup buyGridLayoutGroup;
        [SerializeField] private GridLayoutGroup sellGridLayoutGroup;
        private static RectTransform ScrollViewRectTransform;
        private static BuilderShopElementPanelButton PanelSwitchButton;
        private static ScrollRect ScrollRect;
        private static readonly GameObject[] ObjectsToSelect = new GameObject[2];
        private static int CurrentSelected = 0;
        public static bool BlockSwitch;
        private static int ScrollRectTopOffset;
        private static float ScrollRectSpacing;
        private static bool ControllerEnabled;
        private static bool TabletEnabled;
        private static Vector2 PreviousElementAnchor = Vector2.zero;
        private static event Action OnChangeScrollRect;
        private static event Action OnRefreshElementSelect;

        public static void PublishOnRefreshElementSelect()
        {
            OnRefreshElementSelect?.Invoke();
        }

        private void Awake()
        {
            OnChangeScrollRect += SetScrollRect;
            OnRefreshElementSelect += RefreshSelectedSellElement;
            InputDeviceDetector.OnDeviceChange += DeviceChange;
            GameEvents.UIEvents.OnTabletGamepadEnable += SetControllerState;
            TabletEnabled = false;
            ControllerEnabled = false;
            PanelSwitchButton = panelSwitchButton;
            ScrollRect = buyScrollRect;
            ScrollRectTopOffset = buyGridLayoutGroup.padding.top;
            ScrollRectSpacing = buyGridLayoutGroup.spacing.y;
            ObjectsToSelect[0] = defaultSubcategorySelection.gameObject;
            ScrollRect.transform.TryGetComponent(out ScrollViewRectTransform);
            foreach (Transform child in buyElementContent)
            {
                if (child.TryGetComponent(out ObjectsToSelect[1]))
                {
                    return;
                }
            }
        }
        
        private void Start()
        {
            ControllerEnabled = InputDeviceDetector.GamepadActive;
        }

        public void DefaultSelect()
        {
            EventSystem.current.SetSelectedGameObject(ObjectsToSelect[CurrentSelected]);
        }

        private void OnDestroy()
        {
            OnChangeScrollRect -= SetScrollRect;
            InputDeviceDetector.OnDeviceChange -= DeviceChange;
            OnRefreshElementSelect -= RefreshSelectedSellElement;
            GameEvents.UIEvents.OnTabletGamepadEnable -= SetControllerState;
        }

        /// <summary>
        /// Handling gamepad buttons behaviour
        /// </summary>
        public static void BuildUpdateInputHandle()
        {
            if(!ControllerEnabled || !TabletEnabled) return;
            if (InputManager.Instance.inputActions.UI.MainAction.WasPressedThisFrame() && CurrentSelected == 0)
            {
                if(ObjectsToSelect[0].TryGetComponent(out BuilderShopSubcategoryButton button))
                {
                    button.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }
                
            if (InputManager.Instance.inputActions.UI.Shoulder_R.WasPressedThisFrame() && ObjectsToSelect[1] is {} && !BlockSwitch)
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

        private void DeviceChange(InputDeviceDetector.DeviceType _type)
        {
            ControllerEnabled = _type == InputDeviceDetector.DeviceType.Controller;
            if(!ControllerEnabled || !TabletEnabled) return;
            EventSystem.current.SetSelectedGameObject(ObjectsToSelect[CurrentSelected]);
        }

        private void SetScrollRect()
        {
            bool sellContent = sellElementContent.gameObject.activeInHierarchy;
            ScrollRect = sellContent ? sellScrollRect : buyScrollRect;
            ScrollRectSpacing = sellContent ? sellGridLayoutGroup.spacing.y : buyGridLayoutGroup.spacing.y;
            ScrollRectTopOffset = sellContent ? sellGridLayoutGroup.padding.top : buyGridLayoutGroup.padding.top;
        }

        public static void SelectSubcategory(GameObject _subcategoryToSelect)
        {
            ObjectsToSelect[0] = _subcategoryToSelect;
        }

        private void RefreshSelectedSellElement()
        {
            StartCoroutine(WaitForRefreshContent());
        }

        private IEnumerator WaitForRefreshContent()
        {
            yield return new WaitForEndOfFrame();
            if(sellElementContent.childCount <= 0) yield break;
            EventSystem.current.SetSelectedGameObject(sellElementContent.GetChild(0).gameObject);
            BuilderShopSelectableGenerator.GenerateByTransform(sellElementContent);
        }

        public static void SelectElement(int _sign)
        {
            CurrentSelected = Mathf.Clamp(CurrentSelected + _sign, 0, ObjectsToSelect.Length-1);
            EventSystem.current.SetSelectedGameObject(ObjectsToSelect[CurrentSelected]);
        }

        public static void AssignContentElement(GameObject _contentElement)
        {
            ObjectsToSelect[1] = _contentElement;
        }
        
        private void SetControllerState(bool _enable)
        {
            TabletEnabled =
                _enable && TabletStateMachine.Instance.CurrentState.GetState == TabletMachineState.Castorama;
        }
        
        /// <summary>
        /// Scrolls rect content to selected element by gamepad, selected element is always on screen
        /// </summary>
        /// <param name="_element"></param>
        public static void ScrollToElement(RectTransform _element)
        {
            if(ObjectsToSelect[1] is null) return;
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