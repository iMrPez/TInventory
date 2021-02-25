using TInventory.Container;

namespace Inventory.Item
{
    public class WeaponItem : AItem
    {
        /// <summary>
        /// Initializes Weapon's data and sets items container.
        /// </summary>
        /// <param name="weaponItemData"></param>
        /// <param name="containerGroup"></param>
        public void Initialize(WeaponItemData weaponItemData, ContainerGroup containerGroup)
        {
            base.Initialize(weaponItemData.itemData, containerGroup);
        }
    }
}