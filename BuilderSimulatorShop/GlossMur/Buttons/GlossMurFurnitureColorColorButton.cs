using DG.Tweening;
using UI.Game.ReworkTablet.GlossMur.Interfaces;
using UI.Game.ReworkTablet.GlossMur.Shop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.GlossMur.Buttons
{
    public class GlossMurFurnitureColorColorButton : Button, IFurnitureColorPick, IFurnitureColorSelection, IFurnitureColorInspector
    {
        private GlossMurShopBuyElement furniture;
        [field: SerializeField] public Image ColorImage { get; set; }
        [SerializeField] private Image borderImage;
        private static readonly Color SELECT_COLOR = new Color(1, 0, 0.4f, 1f);
        private static readonly Color NORMAL_COLOR = new Color(0.75f, 0.75f, 0.75f, 1f);
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
        public int ColorIndex { get; set; }
        public void SetColor(int _colorIndex)
        {
            furniture.ChangeColor(ColorIndex);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            SetColor(ColorIndex);
        }

        public void InjectFurniture(GlossMurShopBuyElement _furniture)
        {
            if(furniture != null && furniture == _furniture) return;
            furniture = _furniture;
        }

        public void SetColorPreview()
        {
            Data.Furnishing.Furniture[] furnitures = furniture.FurnituresSet.furnitures;
            if (ColorIndex >= furnitures.Length)
            {
                furniture.RemoveColor(this);
                Destroy(borderImage.transform.parent.gameObject);
                return;
            }
            ColorImage.sprite = furnitures[ColorIndex].color;
        }

        public void SelectColor()
        {
            borderImage.DOColor(SELECT_COLOR, 0.15f);
        }

        public void DeselectColor()
        {
            borderImage.DOColor(NORMAL_COLOR, 0.15f);
        }
    }
}
