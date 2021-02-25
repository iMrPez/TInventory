using Sirenix.OdinInspector;
using UnityEngine;

namespace TInventory.Container
{
    [CreateAssetMenu(fileName = "ContainerData", menuName = "Inventory/Container")]
    public class ContainerData : SerializedScriptableObject
    {
        [ShowInInspector]
        public int[,] Container;
        
        // TODO EDIT
        public float Width => Container.GetLength(0);
        public float Height => Container.GetLength(1);
    }

}
