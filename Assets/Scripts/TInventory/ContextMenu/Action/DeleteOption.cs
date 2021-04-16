using UnityEngine;

namespace TInventory.ContextMenu.Action
{
    public class DeleteOption : IOption
    {
        private readonly Item.Item _target;

        public DeleteOption(Item.Item target)
        {
            _target = target;
        }

        public string GetName()
        {
            return "Delete";
        }

        public bool CanAct()
        {
            if (_target is null)
            {
                Debug.LogError("No target set!");
                return false;
            }

            return true;
        }

        public void Act()
        {
            _target.Destroy();
        }
        
    }
}
