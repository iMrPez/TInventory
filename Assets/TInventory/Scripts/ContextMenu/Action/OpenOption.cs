using TInventory.Item;

namespace TInventory.ContextMenu.Action
{
    public class OpenOption : IOption
    {
        private ContainerItem _target;

        public OpenOption(ContainerItem target)
        {
            _target = target;
        }

        public string GetName()
        {
            return "Open";
        }

        public bool CanAct()
        {
            return true;
        }

        public void Act()
        {
            _target.Show();
        }
    }
}