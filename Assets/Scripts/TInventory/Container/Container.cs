using System.Collections.Generic;
using System.Linq;
using Inventory.Item;
using TInventory.Item;
using TMPro;
using UnityEngine;

namespace TInventory.Container
{
    public class Container : MonoBehaviour
    {
        /// <summary>
        /// Container window title text field.
        /// </summary>
        [SerializeField] 
        private TextMeshProUGUI titleText;
        
        /// <summary>
        /// Container content, used as parent for slots and items.
        /// </summary>
        private RectTransform rectTransform;

        [SerializeField]
        private ContainerData containerData;
        
        /// <summary>
        /// List of items in container.
        /// </summary>
        public List<AItem> items = new List<AItem>();

        /// <summary>
        /// List of Container groups
        /// </summary>
        private List<ContainerGroup> containerGroups;

        public event ItemMovedDelegate ItemAddedHandler;

        public event ItemMovedDelegate ItemRemovedHandler;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            // TODO remove create item for testing.
            if (InputHandler.GetSecondaryButtonDown())
            {
                if (Inventory.GetContainerAt(Input.mousePosition) == this)
                {
                    var item = ItemFactory.instance.CreateBasicItem(5);
                    if(!AddItem(item)) item.Destroy();
                }
            }
        }

        // TODO ADD SUMMARY
        public void Initialize(ContainerData containerData)
        {
            this.containerData = containerData;
            CreateContainer(containerData);
        }
        
        /// <summary>
        /// Initializes the Container with a determined by the x|y * slot size. 
        /// </summary>
        /// <param name="containerData"></param>
        public void CreateContainer(ContainerData containerData)
        {
            ClearItems();

            var slotSize = Inventory.instance.slotSize;

            var padding = Inventory.instance.padding;

            var margin = Inventory.instance.margin;

            rectTransform.sizeDelta = new Vector2( (containerData.Width * (slotSize + padding + margin)) + margin,
                (containerData.Height * (slotSize + padding + margin)) + margin);

            containerGroups = GetContainerGroups(containerData);
            
            foreach (var containerGroup in containerGroups)
            {
                CreateContainerGroup(containerGroup);
            }
            
        }

        // TODO ADD SUMMARY
        private void CreateContainerGroup(ContainerGroup containerGroup)
        {
            
            var slotSize = Inventory.instance.slotSize;
            var padding = Inventory.instance.padding;
            var margin = Inventory.instance.margin;
            
            var groupSize = slotSize + padding;

            var slot = Instantiate(Inventory.instance.slotPrefab, containerGroup.rectTransform).GetComponent<RectTransform>();

            // Move group to correct slot position
            containerGroup.rectTransform.position += new Vector3(
                margin +((groupSize + margin) * containerGroup.position.x) + (margin / 1.5f) * (containerGroup.size.x - 1), 
                -margin + -(((groupSize + margin) * containerGroup.position.y) + (margin / 1.5f) * (containerGroup.size.y - 1)));

            // Add padding to slot
            slot.position += new Vector3(padding / 2, -padding / 2);
            
            // Set slot size
            slot.sizeDelta = new Vector2(containerGroup.size.x * slotSize, containerGroup.size.y * slotSize);
            
            // Set group size
            containerGroup.rectTransform.sizeDelta = new Vector2(containerGroup.size.x * slotSize + padding, containerGroup.size.y * slotSize + padding);
        }


        // TODO ADD SUMMARY AND NEEDS TESTING
        private List<ContainerGroup> GetContainerGroups(ContainerData containerData)
        {
            var containerGroups = new List<ContainerGroup>();
            
            for (int x = 0; x < containerData.Width; x++)
            for (int y = 0; y < containerData.Height; y++)
            {
                var slot = containerData.Container[x, y];

                // Check if slot is a single slot or the slot is empty
                switch (slot)
                {
                    case 0: // Empty Slot
                        continue;
                    case 1: // Single Slot
                        var group = Instantiate(Inventory.instance.containerGroupPrefab, rectTransform)
                            .GetComponent<ContainerGroup>();
                        
                        group.Init(slot, new Vector2(x, y), new Vector2(1, 1), this);
                        
                        containerGroups.Add(group);
                        continue;
                }

                
                // Check for matching group of slots
                ContainerGroup match = containerGroups.FirstOrDefault(g => g.id == slot);

                
                if (match != null)
                {
                    // Add slot if their is a matching group of slots
                    match.AddPosition(x,y);
                }
                else
                {
                    var group = Instantiate(Inventory.instance.containerGroupPrefab, rectTransform)
                        .GetComponent<ContainerGroup>();
                        
                    group.Init(slot, new Vector2(x, y), new Vector2(1, 1), this);
                        
                    containerGroups.Add(group);
                }
            }

            
            return containerGroups;
        }
        
        
        /// <summary>
        /// Returns the slot at the supplied position
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Returns slot x and y from Container where supplied position lies.</returns>
        public (Vector2 Slot, ContainerGroup ContainerGroup) GetSlotFromPosition(Vector3 position)
        {
            var containerGroup = Inventory.GetContainerGroupAt(position);

            if (containerGroup == null) return (new Vector2(-1, -1), null);
            
            position = containerGroup.rectTransform.position - position;
        
            var size = Inventory.instance.slotSize;

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
        /// <param name="slot">Slot to check at</param>
        /// <param name="slotGroup">TODO ADD</param>
        /// <param name="containerGroup">TODO ADD</param>
        /// <param name="item">Item to try to place</param>
        /// <returns>Returns true if Item can fit at Slot.</returns>
        /// TODO TEST
        public bool CanPlaceItemAt(Vector2 slot, ContainerGroup containerGroup, AItem item)
        {
            return (IsSlotInsideContainer(slot, containerGroup) && CanItemFitAt(slot, containerGroup, item));

        }


        /// <summary>
        /// Places Item at given Slot.
        /// </summary>
        /// <param name="slot">Slot to place item at</param>
        /// <param name="containerGroup"></param>
        /// <param name="item">Item to be placed</param>
        /// TODO TEST
        public void PlaceItemAt(Vector2 slot, ContainerGroup containerGroup, AItem item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
            
            item.transform.SetParent(containerGroup.rectTransform);

            item.containerGroup = containerGroup;
            
            item.slotPosition = slot;

            var padding = Inventory.instance.padding / 2;

            item.transform.localPosition = (new Vector2(slot.x, -slot.y) * Inventory.instance.slotSize) + new Vector2(padding, -padding);

            item.transform.SetAsLastSibling();
            
            // Trigger Events
            ItemMover.OnItemPlaced(item);
            
            OnItemAddedHandler(item);
            
            item.OnItemPlaced();
        }

        public void RemoveItem(AItem item)
        {
            items.Remove(item);
            OnItemRemovedHandler(item);
        }
        
        
        /// <summary>
        /// Check if Slot exists in theContainer.
        /// </summary>
        /// <param name="slot">Slot to check</param>
        /// <param name="containerGroup"></param>
        /// <returns>Returns true if Slot exists in Container</returns>
        /// TODO TEST
        public bool IsSlotInsideContainer(Vector2 slot, ContainerGroup containerGroup)
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
        /// TODO TEST
        public bool CanItemFitAt(Vector2 slot, ContainerGroup containerGroup, AItem item)
        {
            if (containerGroup is null || item is null) return false;

            if (slot.x + item.size.x <= containerGroup.size.x && slot.y + item.size.y <= containerGroup.size.y)
            {
                for (float x = slot.x; x < slot.x + item.size.x; x++)
                {
                    for (float y = slot.y; y < slot.y + item.size.y; y++)
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
        /// <param name="containerGroup">TODO ADD</param>
        /// <returns>Returns true if an Item exists at Slot</returns>
        public bool IsSlotInUse(Vector2 slot, ContainerGroup containerGroup)
        {
            return IsSlotInUse(slot.x, slot.y, containerGroup);
        }


        /// <summary>
        /// Check if Slot already has an Item at it.
        /// </summary>
        /// <param name="x">Slot x location</param>
        /// <param name="y">Slot y location</param>
        /// <param name="containerGroup">TODO ADD</param>
        /// <param name="itemToIgnore">Item to ignore</param>
        /// <returns>Returns true if an Item exists at Slot</returns>
        public bool IsSlotInUse(float x, float y, ContainerGroup containerGroup, GameObject itemToIgnore = null)
        {

            if (containerGroup is null) return true;

            foreach (var item in items)
            {
                if (item.gameObject == itemToIgnore || item.containerGroup != containerGroup) continue;
                
                if ((x >= item.slotPosition.x && y >= item.slotPosition.y) && (x <= item.slotPosition.x + item.size.x -1) && (y <= item.slotPosition.y + item.size.y -1))
                {
                    return true;
                }
            }

            return false;
        }
    
        /// <summary>
        /// Clears all items in Container.
        /// </summary>
        private void ClearItems()
        {
            foreach (var item in items)
            {
                item.Destroy();
            }
            
            items.Clear();
        }

    
        /// <summary>
        /// Adds item to Container if there is room.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Returns true if Item fits and is successfully added to Container</returns>
        public bool AddItem(AItem item)
        {
            foreach (var containerGroup in containerGroups)
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

        protected virtual void OnItemAddedHandler(AItem item)
        {
            ItemAddedHandler?.Invoke(item);
        }

        protected virtual void OnItemRemovedHandler(AItem item)
        {
            ItemRemovedHandler?.Invoke(item);
        }
    }
}
