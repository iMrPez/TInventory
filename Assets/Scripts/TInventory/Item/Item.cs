using System.Collections.Generic;
using System.IO;
using TInventory.Container;
using TInventory.ContextMenu.Action;
using TInventory.Item.Action;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TInventory.Item
{
        public delegate void ItemMovedDelegate(Item item);
        
        /// <summary>
        /// Item abstract class, used to add basic functionality to item.
        /// </summary>
        [RequireComponent(typeof(Image), typeof(RectTransform))]
        public abstract class Item : MonoBehaviour, IManageable
        {
                
                public RectTransform RectTransform { get; private set; }

                /// <summary>
                /// Item data scriptable object prefab.
                /// </summary>
                [Header("Item Data")] protected ItemData data;
                public ItemData Data => data;
                
                /// <summary>
                /// item's current slot size.
                /// </summary>
                private Vector2 _size;
                
                public Vector2 Size => _size;
                
                public Vector2 SlotPosition { get; private set; }
                
                public ContainerGroup ContainerGroup { get; private set; }

                public AttachSlot.AttachSlot AttachedSlot { get; private set; }

                private int _count;

                [SerializeField]
                private Color _defaultColor;

                [SerializeField] private Image _iconImage;
                [SerializeField] private TextMeshProUGUI _itemNameText;
                [SerializeField] private TextMeshProUGUI _itemCountText;

                private Image _background;
                
                private bool _isRotated;
                public bool IsRotated => _isRotated;

                private void Awake()
                {
                        RectTransform = GetComponent<RectTransform>();
                        _background = GetComponent<Image>();
                }
                
                public abstract List<IItemAction> GetReleaseActions();
                
                public abstract List<IOption> GetContextMenuActions();

                public abstract IOption GetDoubleClickAction();

                /// <summary>
                /// Sets the item data with the supplied item data.
                /// </summary>
                /// <param name="itemData">Item data.</param>
                /// <param name="container">Container the item is in.</param>
                private void SetItemInfo(ItemData itemData, ContainerGroup containerGroup)
                {
                        data = itemData;
                        
                        SetName(itemData.itemName);

                        SetItemSizeBySlots(itemData.size);

                        _iconImage.sprite = itemData.image;

                        ContainerGroup = containerGroup;
                        
                        UpdateImageSize(RectTransform.sizeDelta);

                }
                
                private void UpdateNameDisplay()
                {
                        _itemNameText.text = data.itemName.Length > 5 * _size.x ? data.shortName : data.itemName;
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
                /// Updates the image size to fit current size
                /// </summary>
                /// <param name="itemSize">Size of item</param>
                public void UpdateImageSize(Vector2 itemSize)
                {
                        RectTransform rect = _iconImage.GetComponent<RectTransform>();

                        rect.sizeDelta = new Vector2(
                                itemSize.x > data.maxImageSize.x
                                        ? data.maxImageSize.x
                                        : itemSize.x,
                                itemSize.y > data.maxImageSize.y
                                        ? data.maxImageSize.y
                                        : itemSize.y);
                        
                }

                
                /// <summary>
                /// Helper method for updating items location information
                /// </summary>
                /// <param name="slot">Slot</param>
                /// <param name="containerGroup">Container Group</param>
                /// <param name="attachSlot">Attach Slot</param>
                public void UpdatePlacementInfo(Vector2 slot = default, ContainerGroup containerGroup = null, AttachSlot.AttachSlot attachSlot = null)
                {
                        ContainerGroup = containerGroup;

                        AttachedSlot = attachSlot;

                        SlotPosition = slot;
                }
                
                
                /// <summary>
                /// Sets the items text field.
                /// </summary>
                /// <param name="itemName">Name to set the item's name to.</param>
                private void SetName(string itemName)
                {
                        if (_size.x > 2 && itemName.Length < 15)
                        {
                                _itemNameText.text = itemName;
                        }
                        else if (_size.x < 2 && itemName.Length < 6)
                        {
                                _itemNameText.text = itemName;
                        }

                        _itemNameText.text = "";
                }

                
                /// <summary>
                /// Rotate the item.
                /// </summary>
                public void Rotate()
                {
                        SetItemSizeBySlots(new Vector2(_size.y, _size.x));
                        _iconImage.transform.Rotate(new Vector3(0,0, _isRotated? -90 : 90));
                        _isRotated = !_isRotated;
                }


                /// <summary>
                /// Sets the item's size.
                /// </summary>
                /// <param name="itemSize">Size to set item to.</param>
                public Vector2 SetItemSizeBySlots(Vector2 itemSize)
                {
                        var oldSize = _size;
                        _size = itemSize;
                        
                        var slotSize = Inventory.Instance.slotSize;
                        RectTransform.sizeDelta = new Vector2(slotSize * itemSize.x, slotSize * itemSize.y);

                        UpdateNameDisplay();
                        
                        return oldSize;
                }
                
                /// <summary>
                /// Sets the item's size.
                /// </summary>
                /// <param name="itemSize">Size to set item to.</param>
                public Vector2 SetItemSize(Vector2 itemSize)
                {
                        var oldSize = _size;
                        _size = itemSize;
 
                        RectTransform.sizeDelta = itemSize;
                        return oldSize;
                }
                
                
                /// <summary>
                /// Sets the items background color.
                /// </summary>
                /// <param name="color">Color to set background to.</param>
                public virtual Color SetBackgroundColor(Color color)
                {
                        var oldColor = _background.color;
                        _background.color = color;

                        return oldColor;
                }

                
                /// <summary>
                /// Resets the background color to the default item color
                /// </summary>
                public virtual void ResetColor()
                {
                        SetBackgroundColor(_defaultColor);
                }

                /// <summary>
                /// Update the item's count. Leaving the param empty sets the item to its max count.
                /// </summary>
                /// <param name="newCount">Count to set the item amount to.</param>
                public virtual void SetCount(int newCount = 9999)
                {
                        newCount = newCount > data.maxCount ? data.maxCount : newCount;
                        
                        if (newCount == 0) Destroy();
                        
                        _count = newCount;
                        
                        _itemCountText.text = newCount.ToString();
                }

                /// <summary>
                /// Retrieves item's current count.
                /// </summary>
                /// <returns>Item's count</returns>
                public virtual int GetCount() => _count;

                
                /// <summary>
                /// Destroy the item.
                /// </summary>
                public void Destroy()
                {
                        ContainerGroup?.parentContainer.RemoveItem(this);
                        Destroy(gameObject);
                }

                public virtual object GetModel()
                {
                        var itemModel = new ItemModel(data.id, _isRotated, _count);

                        return new ItemModelWrapper(data.id, itemModel, ContainerGroup.id, SlotPosition);
                }
                
                public virtual bool LoadModel(string modelJson)
                {
                        var m = JsonUtility.FromJson<ItemModel>(modelJson);
                        /*var m = (ItemModel) model;*/

                        if (m.isRotated) Rotate();
                        
                        SetCount(m.count);
                        
                        return true;
                }
        }
}