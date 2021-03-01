using System;
using System.Collections.Generic;
using TInventory.Container;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Item
{
        /// <summary>
        /// Item abstract class, used to add basic functionality to item.
        /// </summary>
        [RequireComponent(typeof(Image))]
        public abstract class AItem : MonoBehaviour
        {
                /// <summary>
                /// Item data scriptable object prefab.
                /// </summary>
                [Header("Item Data")] 
                public ItemData data;
                
                /// <summary>
                /// item's current slot size.
                /// </summary>
                public Vector2 size;
                
                /// <summary>
                /// Item's current slot position inside its Container Group.
                /// </summary>
                public Vector2 slotPosition;
                
                /// <summary>
                /// Items current count
                /// </summary>
                private int count;

                public ContainerGroup containerGroup;
                
                /// <summary>
                /// List of possible actions to be performed when item is released 
                /// </summary>
                public List<IItemAction> itemReleaseActions = new List<IItemAction>();
                
                /// <summary>
                /// UI - Icon to display the items sprite.
                /// </summary>
                [SerializeField] 
                private Image iconRenderer;

                /// <summary>
                /// UI - Items name text field.
                /// </summary>
                [SerializeField] 
                private TextMeshProUGUI itemNameText;

                /// <summary>
                /// UI - Items count text field.
                /// </summary>
                [SerializeField] 
                private TextMeshProUGUI itemCountText;

                /// <summary>
                /// Items RectTransform.
                /// </summary>
                private RectTransform RectTransform;
                
                /// <summary>
                /// Items background.
                /// </summary>
                private Image background;

                /// <summary>
                /// Is the item rotated.
                /// </summary>
                private bool isRotated = false;

                /// <summary>
                /// Check if the item is rotated.
                /// </summary>
                /// <returns>Returns true if the item has been rotated.</returns>
                public bool IsRotated() => isRotated;
                
                private void Awake()
                {
                        RectTransform = GetComponent<RectTransform>();
                        background = GetComponent<Image>();
                }

                /// <summary>
                /// Sets the item data with the supplied item data.
                /// </summary>
                /// <param name="itemData">Item data.</param>
                /// <param name="container">Container the item is in.</param>
                protected void SetItemInfo(ItemData itemData, ContainerGroup containerGroup)
                {
                        data = itemData;
                        
                        SetName(itemData.itemName);

                        SetItemSize(itemData.size);

                        iconRenderer.sprite = itemData.image;

                        this.containerGroup = containerGroup;
                }

                /// <summary>
                /// Initialize the item data.
                /// </summary>
                /// <param name="itemData">Item data.</param>
                /// <param name="container">Container the item is in.</param>
                public void Initialize(ItemData itemData, ContainerGroup containerGroup)
                {
                        SetItemInfo(itemData, containerGroup);
                }
                
                /// <summary>
                /// Sets the items text field.
                /// </summary>
                /// <param name="itemName">Name to set the item's name to.</param>
                private void SetName(string itemName)
                {
                        if (size.x > 2 && itemName.Length < 15)
                        {
                                itemNameText.text = itemName;
                        }
                        else if (size.x < 2 && itemName.Length < 6)
                        {
                                itemNameText.text = itemName;
                        }

                        itemNameText.text = "";
                }

                
                /// <summary>
                /// Rotate the item.
                /// </summary>
                public void Rotate()
                {
                        SetItemSize(new Vector2(size.y, size.x));
                        iconRenderer.transform.Rotate(new Vector3(0,0, isRotated? -90 : 90));
                        isRotated = !isRotated;
                }

                /// <summary>
                /// Sets the item's size.
                /// </summary>
                /// <param name="itemSize">Size to set item to.</param>
                private void SetItemSize(Vector2 itemSize)
                {
                        size = itemSize;
                        
                        var slotSize = TInventory.Inventory.Instance.slotSize;
                        RectTransform.sizeDelta = new Vector2(slotSize * itemSize.x, slotSize * itemSize.y);
                }
                
                
                /// <summary>
                /// Sets the items background color.
                /// </summary>
                /// <param name="color">Color to set background to.</param>
                public virtual void SetBackgroundColor(Color color)
                {
                        background.color = color;
                }

                /// <summary>
                /// Update the item's count. Leaving the param empty sets the item to its max count.
                /// </summary>
                /// <param name="newCount">Count to set the item amount to.</param>
                public virtual void SetCount(int newCount = 9999)
                {
                        newCount = newCount > data.maxCount ? data.maxCount : newCount;
                        
                        if (newCount == 0) Destroy();
                        
                        count = newCount;
                        
                        itemCountText.text = newCount.ToString();
                }

                /// <summary>
                /// Retrieves item's current count.
                /// </summary>
                /// <returns>Item's count</returns>
                public virtual int GetCount() => count;

                /// <summary>
                /// Destroy the item.
                /// </summary>
                public void Destroy()
                {
                        containerGroup?.parentContainer.items.Remove(this);
                        Destroy(gameObject);
                }
        }
}