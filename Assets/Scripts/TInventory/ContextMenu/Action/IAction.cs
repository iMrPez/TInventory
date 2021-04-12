using TInventory.Item;
using UnityEngine;
using UnityEngine.Events;

namespace TInventory.ContextMenu.Action
{
    public interface IAction
    {   
        public string GetName();
        
        public bool CanAct();
        
        public void Act();

        public void SetTarget(AItem item);
    }
}
