using System.Collections.Generic;
using System.Linq;
using Data.Furnishing;
using UI.Game.ReworkTablet.GlossMur.Tutorial;

namespace UI.Game.ReworkTablet.GlossMur.Spawners
{
    public class TutorialGlossMurShopBuyElementSpawner : GlossMurShopBuyElementSpawner
    {
        protected override void DelaySpawn(IList<KeyValuePair<string, FurnituresSet>> _furnituresToSpawn)
        {
            IList<KeyValuePair<string, FurnituresSet>> onTopElementsList = new List<KeyValuePair<string, FurnituresSet>>();
            for (int i = 0; i < _furnituresToSpawn.Count; i++)
            {
                if(_furnituresToSpawn[i].Value.furnitures.Any(_x=>GlossMurShopIndicatorKeeper.ObjectsToBuyIndicate.Contains(_x.assetReference.AssetGUID)))
                {
                    onTopElementsList.Add(_furnituresToSpawn[i]);
                }
            }

            for (int i = 0; i < onTopElementsList.Count; i++)
            {
                _furnituresToSpawn.Remove(onTopElementsList[i]);
                _furnituresToSpawn.Insert(0,onTopElementsList[i]);
            }
            base.DelaySpawn(_furnituresToSpawn);
        }
    }
}