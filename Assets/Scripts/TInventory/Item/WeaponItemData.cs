using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Inventory/Item/Weapon Item Data")]
    public class WeaponItemData : ScriptableObject
    {
        /// <summary>
        /// Weapon item data.
        /// </summary>
        public ItemData itemData;

        /// <summary>
        /// Weapon fire rate.
        /// </summary>
        public float fireRate;
        
        /// <summary>
        /// How fast the weapon recovers from recoil
        /// </summary>
        public float recoilRecoverySpeed;
        
        /// <summary>
        /// Amount of recoil
        /// </summary>
        public float recoil;
        
        /// <summary>
        /// Bullet prefab
        /// </summary>
        public GameObject bulletPrefab;
    }
}
