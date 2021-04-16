using System.Collections.Generic;
using System.Linq;
using TInventory.Container;
using TInventory.Filter;
using TInventory.Item;
using UnityEngine;
using static TInventory.Inventory;

namespace Prefabs.Inventory.Example
{
    public class LootBox : MonoBehaviour
    {
        [SerializeField] private ContainerData _containerData;

        [SerializeField] private Filter _lootFilter;

        [SerializeField] private int _maxItemsToAdd = 5;
        
        private List<(ItemData item, float rarity)> _lootList;

        
        private void Start()
        {
            _lootList = ItemFactory.GetFilteredItemList(_lootFilter).ToList();
        }

        public void OpenLootBox()
        {
            var window = CreateNewWindow("Loot Window", new Vector2(600, 600));

            if (window is null) return;

            var container = CreateNewContainer();
            
            container.InitializeContainer(_containerData);
            
            window.AddContent(container.RectTransform);

            for (var i = 0; i < _maxItemsToAdd; i++)
            {
                var itemData = ItemFactory.GetRandomItemFromList(_lootList);

                if (itemData is null)
                {
                    Debug.LogError("Item ID not found!");
                    return;
                }
                
                var itemToAdd = ItemFactory.Instance.CreateItem(itemData);

                container.AddItem(itemToAdd);
            }
        }
    }
}
