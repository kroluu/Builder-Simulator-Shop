using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Audio;
using Core.Data;
using Core.Events;
using Core.Managers;
using Data.Furnishing;
using Gameplay.SkillsSystem;
using UI.Game.ReworkTablet.Container;
using UI.Game.ReworkTablet.Enums;
using UI.Game.ReworkTablet.GlossMur.Buttons;
using UI.Game.ReworkTablet.GlossMur.Interfaces;
using UI.Game.ReworkTablet.Popups;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.ReworkTablet.GlossMur.Shop
{
    public class GlossMurShopBuyElement : GlossMurShopElement, IShopFurnitureInfo, IShopFurnitureColorIndex
    {
        [SerializeField]
        private List<GlossMurFurnitureColorColorButton> colorButtons = new List<GlossMurFurnitureColorColorButton>();
        [SerializeField] private GlossMurShopSelectable glossMurShopSelectable;
        public FurnitureRoomCategory Category { get; set; }
        public string Subcategory { get; set; }
        
        public FurnituresSet FurnituresSet { get; set; }
        private int InventoryElementLimit => Config.MAX_AMOUNT_OF_ITEM_IN_INVENTORY;
        protected override void Awake()
        {
            InjectForColorButtons();
            base.Awake();
        }

        private void InjectForColorButtons()
        {
            for (int i = 0; i < colorButtons.Count; i++)
            {
                colorButtons[i].InjectFurniture(this);
            }
        }

        private void InitColorIndexes()
        {
            for (int i = colorButtons.Count-1; i>=0; i--)
            {
                if (colorButtons[i].TryGetComponent(out IFurnitureColorPick colorPick))
                {
                    colorPick.ColorIndex = i;
                    colorButtons[i].SetColorPreview();
                }
            }
        }

        public void RemoveColor(GlossMurFurnitureColorColorButton _colorButton)
        {
            colorButtons.Remove(_colorButton);
        }

        protected override void Start()
        {
            base.Start();
            try
            {
                colorButtons[0].OnPointerClick(new PointerEventData(EventSystem.current));
            }
            catch (Exception e)
            {
                Debug.LogError($"No color attached to furniture variant, at least one color must be assigned to furniture set. Throwing an exception in {nameof(GlossMurShopBuyElement)} class");
            }
            
        }
        
        public void AssignFurniture(FurnitureRoomCategory _category, string _subcategory, FurnituresSet _furniture)
        {
            Category = _category;
            Subcategory = _subcategory;
            FurnituresSet = _furniture;
            InjectForColorButtons();
            InitColorIndexes();
            glossMurShopSelectable.InjectColorsToNavigate(colorButtons.Where(_x=>_x).ToList());
        }

        public virtual void ChangeColor(int _colorIndexToShow)
        {
            if(SelectedIndex == _colorIndexToShow) return;
            MainButtonSound();
            colorButtons[SelectedIndex].DeselectColor();
            SelectedIndex = _colorIndexToShow;
            colorButtons[SelectedIndex].SelectColor();
            RefreshValues();
            MasterAudioManager.Instance.SFXManager.PlaySFX(SFXType.UI, "UI_Tablet_SwitchColor");
        }

        public override void RefreshValues()
        {
            Preview.sprite = FurnituresSet.furnitures[SelectedIndex].render;
            Quantity = ScenesCommunicator.GetGameData.equipmentData.GetFurnitureAmount(Name, SelectedIndex);
            base.RefreshValues();
        }

        protected override void OnMainButtonBehaviour()
        {
            if(!CanBuyFurniture()) return;
            if(IsEquipmentFull())
            {
                if(!TabletPopupManager.IsFiring)
                    TabletContainer.Instance.Resolve<TabletPopupManager>().Fire(TabletPopupType.EquipmentFull);
                return;
            }
            BuyFurniture(); 
            MainButtonSound();
            MainButtonClickAnimation();
        }

        protected override void OnSecondButtonBehaviour()
        {
        }

        private void BuyFurniture()
        {
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            equipmentData.PlayerCash -= BuyCost;
            equipmentData.AddFurnitureToEquipment(Category, Subcategory, Name,SelectedIndex, BuyCost);
            Quantity++;
            RefreshValues();
            GameEvents.UIEvents.PublishOnGlossMurBuyElement(FurnituresSet.furnitures[SelectedIndex].assetReference.AssetGUID);
            GameEvents.PublishOnExperienceIncrease(Skill.LoyalCustomer);
        }

        private bool CanBuyFurniture()
        {
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            return equipmentData.CanAffordToBuy(BuyCost)
                   && equipmentData.GetFurnitureAmount(Name, SelectedIndex) <= InventoryElementLimit;
        }

        private bool IsEquipmentFull()
        {
            EquipmentData equipmentData = ScenesCommunicator.GetGameData.equipmentData;
            return equipmentData.IsFurnitureEquipmentFull() && !equipmentData.HaveFurniture(Name,SelectedIndex);
        }

        public int SelectedIndex { get; set; }
    }
}