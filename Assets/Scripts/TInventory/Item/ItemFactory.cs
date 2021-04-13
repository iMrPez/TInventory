using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Item;
using TInventory.Container;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace TInventory.Item
{
    public enum ItemPrefabType
    {
        Basic = 0
    }
    
    public class ItemFactory : MonoBehaviour
    {
        /// <summary>
        /// Item factory singleton instance.
        /// </summary>
        public static ItemFactory instance;

        // TODO REDO prefab holding and instantiation.
        /// <summary>
        /// Basic item prefab.
        /// </summary>
        [SerializeField]
        private GameObject basicItemPrefab;

        public Dictionary<ItemPrefabType, GameObject> itemPrefabs = new Dictionary<ItemPrefabType, GameObject>();
        
        /// <summary>
        /// Basic item data.
        /// </summary>
        [SerializeField]
        private List<ItemData> items;
        

        private void Awake()
        {
            instance = this;
            itemPrefabs.Add(ItemPrefabType.Basic, basicItemPrefab);
        }
        
        /// <summary>
        /// Retrieve item by id.
        /// </summary>
        /// <param name="id">Id of item to lookup</param>
        /// <returns>Returns item if an item with the id is found./</returns>
        public ItemData GetItemById(int id)
        {
            try
            {
                return items.First(i => i.id == id);
            }
            catch
            {
                Debug.LogError($"No item with ID({id} found!", this);
                throw;
            }
        }

        /// <summary>
        /// Create basic items based on id.
        /// </summary>
        /// <param name="prefabType">Type of prefab to create</param>
        /// <param name="id">Id of item to lookup</param>
        /// <param name="container">Container the item will be contained in</param>
        /// <returns>Returns initialized item</returns>
        public AItem CreateItem(int id)
        {
            var itemData = GetItemById(id);

            return CreateItem(itemData);
        }
        
        public AItem CreateItem(ItemData itemData)
        {
            if (itemData != null)
            {
                Debug.Log($"Item({itemData.id}) Created - {itemData}", itemData);
                
                var item = Instantiate(itemPrefabs[itemData.itemPrefabType]).GetComponent<BasicItem>();
            
                item.Initialize(itemData, null);

                item.SetCount(1);
                
                return item;
            }

            Debug.LogError($"Error Creating Item with ID({itemData.id}!", this);

            return null;
        }

        public static IEnumerable<(ItemData item, float rarity)> GetFilteredItemList(Filter.Filter filter)
        {
            foreach (var item in instance.items)
            {
                var rarity = filter.allowedCategory.GetRarity(item);
                
                if (rarity > 0)
                {
                    yield return (item, rarity);
                }
            }
        }

        public static ItemData GetRandomItemFromList(List<(ItemData item, float rarity)> items)
        {
            var totalRarity = items.Sum(x => x.rarity);
            
            var randomNumber = 1 + Math.Round(new Random(DateTime.Now.Millisecond).NextDouble() * (totalRarity - 1));

            var item = items.OrderBy(x => x.item.id).Select(x => new
            {
                x.item,
                
                MinRarity = items.Where(y => y.item.id <= x.item.id).Sum(y => y.rarity) - x.rarity + 1,
                MaxRarity = items.Where(y => y.item.id <= x.item.id).Sum(y => y.rarity)
            }).Single(x => x.MinRarity <= randomNumber && x.MaxRarity >= randomNumber);

            return item.item;
        }
    }
}