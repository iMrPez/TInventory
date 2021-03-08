using System;
using TInventory.Container;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace TInventory.Editor
{
    [CustomEditor(typeof(ContainerData))]
    public class ContainerEditor : UnityEditor.Editor
    {

        public int[,] container;
        
        Vector2Int newContainerSize = Vector2Int.zero;

        bool show;
        
        int groupNumber;
        
        public void OnEnable()
        {
            ContainerData containerData = (ContainerData)target;
            
            container = containerData.Container;
            
            newContainerSize = new Vector2Int((int) containerData.Width, (int) containerData.Height);
        }

        public override void OnInspectorGUI()
        {

            ContainerData containerData = (ContainerData)target;

            containerData.containerName = EditorGUILayout.TextField("Container Name", containerData.containerName);
            
            EditorGUILayout.Separator();
            
            DisplayContainerSize();

            EditorGUILayout.Separator();
            
            // Updates the container size data
            if (GUILayout.Button("Update Container Size"))
            {
                UpdateContainerSize();
            }
            
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginHorizontal();
            
            groupNumber = EditorGUILayout.IntField("Group", math.clamp(groupNumber, 0, 99));
            
            if (GUILayout.Button("Set To Group"))
            {
                SetToGroup(container, groupNumber);
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            EditorGUILayout.HelpBox("Slot customization guide. \n0 Is an empty slot location. \n1 Is a single slot and will be spaced out from the other solo slots. \nAnything greater than 1 is a group. All groups must be in a square and will display an incorrect layout if groups are not properly set. A new number needs to be used for each group.", MessageType.Info);
            
            DisplayContainer(container);

            EditorGUILayout.Separator();
            
            if (GUILayout.Button("Save Container"))
            {
                containerData.SetContainer(container);
                Debug.Log("Container data saved!");
            }
        }

        private void DisplayContainerSize()
        {
            newContainerSize = EditorGUILayout.Vector2IntField("Container Size", newContainerSize);

            newContainerSize.x = newContainerSize.x < 1 ? 1 : newContainerSize.x;
            newContainerSize.y = newContainerSize.y < 1 ? 1 : newContainerSize.y;
        }

        private static void SetToGroup(int[,] containerToModify, int group)
        {
            for (int x = 0; x < containerToModify.GetLength(0); x++)
            for (int y = 0; y < containerToModify.GetLength(1); y++)
            {
                containerToModify[x, y] = group;
            }
        }

        private void UpdateContainerSize()
        {
            var oldContainer = container;
            
            container = new int[newContainerSize.x, newContainerSize.y];

            container = CopyToContainer(container, oldContainer);
        }

        private int[,] CopyToContainer(int[,] containerToModify, int[,] dataToCopy)
        {
            for (int x = 0; x < containerToModify.GetLength(0); x++)
            for (int y = 0; y < containerToModify.GetLength(1); y++)
            {
                if (x < dataToCopy.GetLength(0) &&
                    y < dataToCopy.GetLength(1))
                {
                    containerToModify[x, y] = dataToCopy[x, y];
                }
            }

            return containerToModify;
        }

        private void DisplayContainer(int[,] container)
        {
            show = EditorGUILayout.BeginFoldoutHeaderGroup(show, "Show Container Data");
            
            if (show)
            {
                for (int y = 0; y < container.GetLength(1); y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < container.GetLength(0); x++)
                    {
                        container[x, y] = EditorGUILayout.IntField(container[x, y]);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
