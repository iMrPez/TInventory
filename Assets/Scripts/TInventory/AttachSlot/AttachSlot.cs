using TInventory.Item;
using UnityEngine;

namespace TInventory.AttachSlot
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class AttachSlot : MonoBehaviour
    {
        private Item.Item _attachedItem;

        public Filter.Filter filter;
        
        public event ItemMovedDelegate ItemAttachedHandler;
        public event ItemMovedDelegate ItemDetachHandler;

        
        public Color equippedColor;
        
        private RectTransform _rectTransform;
        
        private Vector2 _oldSize = Vector2.zero;
        private Color _oldColor = Color.white;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            ItemAttachedHandler += OnItemAttached;
            ItemDetachHandler += OnItemDetach;
        }

        /// <summary>
        /// Can Attach Item
        /// </summary>
        /// <param name="item">Item to attach</param>
        /// <returns>If item can be attached</returns>
        public virtual bool CanAttach(Item.Item item)
        {
            if (_attachedItem is null)
            {
                if (filter is null) return true;
                
                if (filter.IsMatching(item)) return true;
            }

            return false;
        }
        
        /// <summary>
        /// Attach Item
        /// </summary>
        /// <param name="item">Item to attach</param>
        public virtual void Attach(Item.Item item)
        {
            if (item.IsRotated) item.Rotate();

            item.transform.position = transform.position;
            
            item.UpdatePlacementInfo(attachSlot:this);

            _attachedItem = item;
            
            _oldSize = item.SetItemSize(new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y));
            _oldColor = item.SetBackgroundColor(equippedColor);

            item.UpdateImageSize(_rectTransform.sizeDelta);
            
            ItemAttachedHandler?.Invoke(item);
        }

        
        /// <summary>
        /// Detach item
        /// </summary>
        /// <param name="item">Item to detach</param>
        public virtual void Detach()
        {
            if (_attachedItem == null) return;
            
            _attachedItem.SetItemSizeBySlots(_oldSize);
            _attachedItem.SetBackgroundColor(_oldColor);

            _attachedItem.UpdateImageSize(_attachedItem.RectTransform.sizeDelta);
            
            ItemDetachHandler?.Invoke(_attachedItem);
            _attachedItem = null;
        }

        protected abstract void OnItemAttached(Item.Item item);

        protected abstract void OnItemDetach(Item.Item item);
    }
}
