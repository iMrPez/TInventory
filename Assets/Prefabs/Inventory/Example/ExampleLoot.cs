using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Item;
using TInventory.Container;
using TInventory.Filter;
using TInventory.Item;
using UnityEngine;

namespace Prefabs.Inventory.Example
{
    public class ExampleLoot : MonoBehaviour
    {
        public ContainerData containerData;

        public Filter lootFilter;

        public int maxItemsToAdd = 5;
        
        private List<(ItemData item, float rarity)> lootList;

        
        private void Start()
        {
            lootList = ItemFactory.GetFilteredItemList(lootFilter).ToList();
        }

        public void OpenLootBox()
        {
            var window = TInventory.Inventory.CreateNewWindow("Loot Window Test", new Vector2(600, 600));

            if (window is null) return;

            var container = TInventory.Inventory.CreateNewContainer();
            
            container.Initialize(containerData);
            
            window.AddContent(container.rectTransform);

            for (var i = 0; i < 5; i++)
            {
                var itemData = ItemFactory.GetRandomItemFromList(lootList);

                if (itemData is null)
                {
                    Debug.LogError("Item ID not found!");
                    return;
                }
                
                var itemToAdd = ItemFactory.instance.CreateBasicItem(itemData);

                container.AddItem(itemToAdd);
            }
            
            

        }
    }
}
