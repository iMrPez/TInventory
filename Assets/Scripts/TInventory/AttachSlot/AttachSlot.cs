using System;
using TInventory.Item;
using UnityEngine;

namespace TInventory.AttachSlot
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class AttachSlot : MonoBehaviour
    {
        private AItem attachedItem = null;
        
        public event ItemMovedDelegate ItemAttachedHandler;

        public event ItemMovedDelegate ItemDetachHandler;

        public Color equippedColor;
        
        private RectTransform rectTransform;
        
        private Vector2 oldSize = Vector2.zero;
        private Color oldColor = Color.white;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            ItemAttachedHandler += OnItemAttached;
            ItemDetachHandler += OnItemDetach;

        }

        public virtual bool CanAttach(AItem item)
        {
            return attachedItem is null;
        }
        
        public virtual void Attach(AItem item)
        {
            if (item.IsRotated()) item.Rotate();

            item.transform.position = transform.position;
            
            item.SetImageModeToCenter();

            item.attachedSlot = this;
            
            item.containerGroup = null;

            oldSize = item.SetItemSize(new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y));
            oldColor = item.SetBackgroundColor(equippedColor);

            if (!(item.iconRect is null) && item.data.maxImageSize != Vector2.zero)
            {
                item.iconRect.sizeDelta = new Vector2(
                    rectTransform.sizeDelta.x > item.data.maxImageSize.x
                        ? item.data.maxImageSize.x
                        : rectTransform.sizeDelta.x,
                    
                    rectTransform.sizeDelta.y > item.data.maxImageSize.y
                        ? item.data.maxImageSize.y
                        : rectTransform.sizeDelta.y);

            }
            
            ItemAttachedHandler?.Invoke(item);
        }

        public virtual void Detach(AItem item)
        {
            item.SetItemSizeBySlots(oldSize);
            item.SetBackgroundColor(oldColor);
            
            item.iconRect.sizeDelta = oldSize;
            item.SetImageModeToFit();
            
            ItemDetachHandler?.Invoke(item);
        }

        protected abstract void OnItemAttached(AItem item);

        protected abstract void OnItemDetach(AItem item);
    }
}
