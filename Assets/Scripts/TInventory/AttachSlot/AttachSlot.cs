using System;
using TInventory.Item;
using UnityEngine;

namespace TInventory.AttachSlot
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class AttachSlot : MonoBehaviour
    {
        private AItem attachedItem = null;

        public Filter.Filter filter;
        
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
            if (attachedItem is null)
            {
                if (filter is null) return true;
                
                if (filter.IsMatching(item)) return true;
            }

            return false;
        }
        
        public virtual void Attach(AItem item)
        {
            if (item.IsRotated()) item.Rotate();

            item.transform.position = transform.position;
            
            item.attachedSlot = this;
            
            item.containerGroup = null;

            attachedItem = item;
            
            oldSize = item.SetItemSize(new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y));
            oldColor = item.SetBackgroundColor(equippedColor);

            item.UpdateImageSize(rectTransform.sizeDelta);
            
            ItemAttachedHandler?.Invoke(item);
        }

        

        public virtual void Detach(AItem item)
        {
            attachedItem = null;
            item.SetItemSizeBySlots(oldSize);
            item.SetBackgroundColor(oldColor);

            item.UpdateImageSize(item.rectTransform.sizeDelta);
            
            ItemDetachHandler?.Invoke(item);
        }

        protected abstract void OnItemAttached(AItem item);

        protected abstract void OnItemDetach(AItem item);
    }
}
