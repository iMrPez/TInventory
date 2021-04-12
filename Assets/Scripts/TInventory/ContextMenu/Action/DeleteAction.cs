using TInventory.Item;
using UnityEngine;
using UnityEngine.Events;

namespace TInventory.ContextMenu.Action
{
    public class DeleteAction : IAction
    {
        private AItem target;

        public DeleteAction(AItem target)
        {
            this.target = target;
        }

        public string GetName()
        {
            return "Delete";
        }

        public bool CanAct()
        {
            if (target is null)
            {
                Debug.LogError("No target set!");
                return false;
            }

            return true;
        }

        public void Act()
        {
            target.Destroy();
        }

        public void SetTarget(AItem item)
        {
            this.target = item;
        }
    }
}
