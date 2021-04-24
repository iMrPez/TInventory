using System;
using System.Collections.Generic;
using System.Linq;
using TInventory.Item;
using TMPro;
using UnityEngine;

namespace TInventory.Container
{
    public abstract class Container : MonoBehaviour, IManageable
    {

        public TextMeshProUGUI TitleText;

        public int containerId;
        
        [SerializeField] private ContainerData _containerData;
        
        [SerializeField] private List<Item.Item> _items = new List<Item.Item>();
        
        private List<ContainerGroup> _containerGroups;

        public List<Item.Item> Items => _items;

        public RectTransform RectTransform { get; private set; }
        
        // Events
        public event ItemMovedDelegate ItemAddedHandler;
        public event ItemMovedDelegate ItemRemovedHandler;
        
        
        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Creates container based on the containerData
        /// </summary>
        /// <param name="containerData"></param>
        public virtual void InitializeContainer(ContainerData containerData)
        {

            _containerData = containerData;
            
            ClearItems();

            _containerGroups = GetContainerGroups(containerData);
            
            
            foreach (var containerGroup in _containerGroups)
            {
                CreateContainerGroup(containerGroup);
            }
            
            RectTransform.sizeDelta = GetContainerSize(_containerGroups);

        }

        /// <summary>
        /// Gets total size of container
        /// </summary>
        /// <param name="groups">Container Groups</param>
        /// <returns>Size of container</returns>
        protected virtual Vector2 GetContainerSize(List<ContainerGroup> groups)
        {
            
            var groupSize = Inventory.Instance.slotSize + Inventory.Instance.padding;

            var margin = Inventory.Instance.margin;
            
            Vector2 finalSize = Vector2.zero;
            
            foreach (var containerGroup in groups)
            {
                var localPosition = containerGroup.transform.localPosition;
                
                var xTest = localPosition.x + (containerGroup.size.x * groupSize);
                var yTest = localPosition.y - (containerGroup.size.y * groupSize);
                
                if (xTest > finalSize.x)
                    finalSize.x = xTest;
                
                if (yTest < finalSize.y)
                    finalSize.y = yTest;
            }
            
            return new Vector2(finalSize.x + margin, Mathf.Abs(finalSize.y) + margin);
        }

        /// <summary>
        /// Creates Container Group
        /// </summary>
        /// <param name="containerGroup"></param>
        protected virtual void CreateContainerGroup(ContainerGroup containerGroup)
        {
            var slotSize = Inventory.Instance.slotSize;
            var padding = Inventory.Instance.padding;
            var margin = Inventory.Instance.margin;
            
            var groupSize = slotSize + padding;

            var slot = Instantiate(Inventory.Instance.SlotPrefab, containerGroup.rectTransform).GetComponent<RectTransform>();
            
            // Move group to correct slot position
            containerGroup.rectTransform.position += new Vector3(
                margin + ((groupSize + margin) * containerGroup.position.x) + (margin / 1.5f) * (containerGroup.size.x - 1), 
                -margin + -(((groupSize + margin) * containerGroup.position.y) + (margin / 1.5f) * (containerGroup.size.y - 1)));

            // Add padding to slot
            slot.position += new Vector3(padding / 2, -padding / 2);
            
            // Set slot size
            slot.sizeDelta = new Vector2(containerGroup.size.x * slotSize, containerGroup.size.y * slotSize);
            
            // Set group size
            containerGroup.rectTransform.sizeDelta = new Vector2(containerGroup.size.x * slotSize + padding, containerGroup.size.y * slotSize + padding);
        }


        /// <summary>
        /// Gets container groups from containerData
        /// </summary>
        /// <param name="containerData">Container Data</param>
        /// <returns>List of container groups</returns>
        protected virtual List<ContainerGroup> GetContainerGroups(ContainerData containerData)
        {
            var groups = new List<ContainerGroup>();
            
            for (int x = 0; x < containerData.Width; x++)
            for (int y = 0; y < containerData.Height; y++)
            {
                var slot = containerData.Container[x + y * containerData.Width];

                // Check if slot is a single slot or the slot is empty
                switch (slot)
                {
                    case 0: // Empty Slot
                        continue;
                    case 1: // Single Slot
                        var group = CreateNewGroup(slot, x, y);

                        groups.Add(group);
                        continue;
                }
                
                // Check for matching group of slots
                ContainerGroup match = groups.FirstOrDefault(g => g.id == slot);

                if (match != null)
                {
                    // Add slot if their is a matching group of slots
                    match.AddPosition(x,y);
                }
                else
                {
                    var group = CreateNewGroup(slot, x, y);

                    groups.Add(group);
                }
            }
            
            return groups;
        }

        /// <summary>
        /// Creates new container group
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Container Group</returns>
        protected virtual ContainerGroup CreateNewGroup(int groupId, int x, int y)
        {
            var group = Instantiate(Inventory.Instance.ContainerGroupPrefab, RectTransform)
                .GetComponent<ContainerGroup>();

            group.Initialize(groupId, new Vector2(x, y), new Vector2(1, 1), this);
            return group;
        }


        /// <summary>
        /// Returns the slot at the supplied position
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Returns slot x and y from Container where supplied position lies.</returns>
        public virtual (Vector2 Slot, ContainerGroup ContainerGroup) GetSlotFromPosition(Vector3 position)
        {
            var containerGroup = InventoryUtility.GetContainerGroupAt(position);

            if (containerGroup == null) return (new Vector2(-1, -1), null);
            
            position = containerGroup.rectTransform.position - position;
        
            var size = Inventory.Instance.slotSize;

            var slot = new Vector2(
                -Mathf.Ceil(position.x / size),
                Mathf.Floor(position.y / size));

            if (slot.x < 0 || slot.y < 0)
            {
                return (new Vector2(-1, -1), null);
            }

            return (slot, containerGroup);
        }

        /// <summary>
        /// Checks if an Item can be placed at a given Container Slot.
        /// </summary>
        /// <param name="slot">Slot position</param>
        /// <param name="containerGroup">Container Group</param>
        /// <param name="item">Item to place</param>
        /// <returns>If item can be placed at given position</returns>
        public virtual bool CanPlaceItemAt(Vector2 slot, ContainerGroup containerGroup, Item.Item item)
        {
            if (_containerData.filter != null)
            {
                if (!_containerData.filter.IsMatching(item)) return false;
            }

            return (IsSlotInsideContainer(slot, containerGroup) && CanItemFitAt(slot, containerGroup, item));
        }


        /// <summary>
        /// Places Item at given Slot.
        /// </summary>
        /// <param name="slot">Slot to place item at</param>
        /// <param name="containerGroup"></param>
        /// <param name="item">Item to be placed</param>
        public virtual void PlaceItemAt(Vector2 slot, ContainerGroup containerGroup, Item.Item item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
            
            item.transform.SetParent(containerGroup.rectTransform);

            item.UpdatePlacementInfo(slot, containerGroup, null);

            item.ResetColor();

            var padding = Inventory.Instance.padding / 2;

            item.transform.localPosition = (new Vector2(slot.x, -slot.y) * Inventory.Instance.slotSize) + new Vector2(padding, -padding);

            item.transform.SetAsLastSibling();

            ItemAddedHandler?.Invoke(item);
        }

        
        /// <summary>
        /// Removes item from list
        /// </summary>
        /// <param name="item"></param>
        public virtual void RemoveItem(Item.Item item)
        {
            _items.Remove(item);
            ItemRemovedHandler?.Invoke(item);
        }
        
        
        /// <summary>
        /// Check if Slot exists in theContainer.
        /// </summary>
        /// <param name="slot">Slot to check</param>
        /// <param name="containerGroup"></param>
        /// <returns>Returns true if Slot exists in Container</returns>
        public virtual bool IsSlotInsideContainer(Vector2 slot, ContainerGroup containerGroup)
        {
            if ((slot.x < 0) || (slot.y < 0)) return false;
            
            if (slot.x <= containerGroup.size.x && slot.y <= containerGroup.size.y)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Check if Item can fit at Slot.
        /// </summary>
        /// <param name="slot">Slot to check at</param>
        /// <param name="containerGroup"></param>
        /// <param name="item">Item to check with</param>
        /// <returns>Returns true if Item fits at Slot</returns>
        public virtual bool CanItemFitAt(Vector2 slot, ContainerGroup containerGroup, Item.Item item)
        {
            if (containerGroup is null || item is null) return false;

            if (slot.x + item.Size.x <= containerGroup.size.x && slot.y + item.Size.y <= containerGroup.size.y)
            {
                for (float x = slot.x; x < slot.x + item.Size.x; x++)
                {
                    for (float y = slot.y; y < slot.y + item.Size.y; y++)
                    {
                        if (IsSlotInUse(x, y, containerGroup, item.gameObject))
                        {
                            return false;
                        }
                    } 
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if Slot already has an Item at it.
        /// </summary>
        /// <param name="slot">Slot to check</param>
        /// <param name="containerGroup">Container Group</param>
        /// <returns>Returns true if an Item exists at Slot</returns>
        public virtual bool IsSlotInUse(Vector2 slot, ContainerGroup containerGroup)
        {
            return IsSlotInUse(slot.x, slot.y, containerGroup);
        }


        /// <summary>
        /// Check if Slot already has an Item at it.
        /// </summary>
        /// <param name="x">Slot x location</param>
        /// <param name="y">Slot y location</param>
        /// <param name="containerGroup">Container Group</param>
        /// <param name="itemToIgnore">Item to ignore</param>
        /// <returns>Returns true if an Item exists at Slot</returns>
        public virtual bool IsSlotInUse(float x, float y, ContainerGroup containerGroup, GameObject itemToIgnore = null)
        {

            if (containerGroup is null) return true;

            foreach (var item in _items)
            {
                if (item.gameObject == itemToIgnore || item.ContainerGroup != containerGroup) continue;
                
                if ((x >= item.SlotPosition.x && y >= item.SlotPosition.y) && (x <= item.SlotPosition.x + item.Size.x -1) && (y <= item.SlotPosition.y + item.Size.y -1))
                {
                    return true;
                }
            }

            return false;
        }
    
        /// <summary>
        /// Clears all items in Container.
        /// </summary>
        public virtual void ClearItems()
        {
            foreach (var item in _items)
            {
                item.Destroy();
            }
            
            _items.Clear();
        }

    
        /// <summary>
        /// Adds item to Container if there is room.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Returns true if Item fits and is successfully added to Container</returns>
        public virtual bool AddItem(Item.Item item)
        {
            foreach (var containerGroup in _containerGroups)
            {
                for (int y = 0; y < containerGroup.size.y; y++)
                {
                    for (int x = 0; x < containerGroup.size.x; x++)
                    {
                        var slot = new Vector2(x, y);
                        
                        if (CanPlaceItemAt(slot, containerGroup, item))
                        {
                            PlaceItemAt(slot, containerGroup, item);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        public virtual bool CanAddItem(Item.Item item, out (Vector2, ContainerGroup) location)
        {
            foreach (var containerGroup in _containerGroups)
            {
                for (int y = 0; y < containerGroup.size.y; y++)
                {
                    for (int x = 0; x < containerGroup.size.x; x++)
                    {
                        var slot = new Vector2(x, y);
                        
                        if (CanPlaceItemAt(slot, containerGroup, item))
                        {
                            location = (slot, containerGroup);
                            return true;
                        }
                    }
                }
            }

            location = (Vector2.zero, null);
            
            return false;
        }

        public virtual ContainerGroup GetContainerGroupFromId(int id)
        {
            return _containerGroups.Find(c => c.id == id);
        }
        
        public virtual object GetModel()
        {
            var itemList = new List<string>();

            foreach (var item in _items)
            {
                itemList.Add(JsonUtility.ToJson(item.GetModel()));
            }

            return new ContainerModel(itemList.ToArray(), _containerData);
        }

        public virtual bool LoadModel(string modelJson)
        {
            var m = JsonUtility.FromJson<ContainerModel>(modelJson);

            foreach (var s in m.itemJson)
            {
                var itemModelWrapper = JsonUtility.FromJson<ItemModelWrapper>(s);

                var item = ItemFactory.Instance.CreateItem(itemModelWrapper.itemID);

                item.LoadModel(itemModelWrapper.model);
                
                PlaceItemAt(itemModelWrapper.slot, GetContainerGroupFromId(itemModelWrapper.containerGroupId), item);
            }
            
            return false;
        }
    }
}
