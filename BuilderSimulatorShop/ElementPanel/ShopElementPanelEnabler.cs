using UI.Game.ReworkTablet.Interfaces;
using UnityEngine;

namespace UI.Game.ReworkTablet.ElementPanel
{
    public class ShopElementPanelEnabler
    {
        private readonly GameObject[] panel;
        private readonly IShopForceSpawnElements spawner;

        public ShopElementPanelEnabler(GameObject[] _panel, IShopForceSpawnElements _spawner)
        {
            panel = _panel;
            spawner = _spawner;
        }

        public void SetVisibility(bool _state, bool _withSpawn = true)
        {
            if(!_state)
                spawner.CancelAsyncSpawner();
            for (int i = 0; i < panel.Length; i++)
            {
                panel[i].SetActive(_state);
            }
            if (_state && _withSpawn)
            {
                spawner.ForceSpawn();
            }
            
        }
    }
}
