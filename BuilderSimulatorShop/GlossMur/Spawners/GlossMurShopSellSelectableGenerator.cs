using System.Collections;
using System.Collections.Generic;
using UI.Game.ReworkTablet.GlossMur.Gamepad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    public class GlossMurShopSellSelectableGenerator : MonoBehaviour
    {
        private static readonly List<Selectable> AsyncSelectables = new List<Selectable>();
        private static GlossMurShopSellSelectableGenerator instance;
        private static Coroutine Coroutine;
        private void Awake()
        {
            instance = this;
        }

        public static void ClearSelectables()
        {
            AsyncSelectables.Clear();
        }

        public static void GenerateByTransform(Transform _parent)
        {
            AsyncSelectables.Clear();
            foreach (Transform child in _parent)
            {
                if(child.TryGetComponent(out Selectable selectable))
                    AsyncSelectables.Add(selectable);
            }
            int elementsInRow = 0;
            for (int i = 0; i < AsyncSelectables.Count; i++)
            {
                if (Mathf.Abs(AsyncSelectables[i].transform.position.y - AsyncSelectables[Mathf.Min(AsyncSelectables.Count-1, i + 1)].transform.position.y) > 0.01f)
                {
                    elementsInRow = i + 1;
                    break;
                }
            }
            if(Coroutine is {})
                instance.StopCoroutine(Coroutine);
            Coroutine = instance.StartCoroutine(AsyncGenerate(elementsInRow));
        }
        
        public static void AddSelectable(Selectable _selectable)
        {
            AsyncSelectables.Add(_selectable);
            if (AsyncSelectables.Count == 1)
            {
                GlossMurShopGamepadInputHandler.AssignContentElement(AsyncSelectables[0].gameObject);
                return;
            }
            
            int elementsInRow = 0;
            for (int i = 0; i < AsyncSelectables.Count; i++)
            {
                if (Mathf.Abs(AsyncSelectables[i].transform.position.y - AsyncSelectables[Mathf.Min(AsyncSelectables.Count-1, i + 1)].transform.position.y) > 0.01f)
                {
                    elementsInRow = i + 1;
                    break;
                }
            }
            if(Coroutine is {})
                instance.StopCoroutine(Coroutine);
            Coroutine = instance.StartCoroutine(AsyncGenerate(elementsInRow));
        }

        private static IEnumerator AsyncGenerate(int _rowCount)
        {
            for (int i = 0; i < AsyncSelectables.Count; i++)
            {
                Navigation nav = AsyncSelectables[i].navigation;
                nav.mode = Navigation.Mode.Explicit;

                if (i + 1 < AsyncSelectables.Count)
                    nav.selectOnRight = AsyncSelectables[i + 1];
                if (i - 1 >= 0)
                    nav.selectOnLeft = AsyncSelectables[i - 1];
                if (i - _rowCount >= 0)
                    nav.selectOnUp = AsyncSelectables[i - _rowCount];
                if (i + _rowCount < AsyncSelectables.Count)
                    nav.selectOnDown = AsyncSelectables[i + _rowCount];
                else if (i / _rowCount != (AsyncSelectables.Count - 1) / _rowCount)
                    nav.selectOnDown = AsyncSelectables[AsyncSelectables.Count - 1];

                AsyncSelectables[i].navigation = nav;
            }

            yield return null;
        }
    }
}