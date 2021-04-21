using System;
using System.IO;
using TInventory.Container;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace TInventory.Editor
{
    [CustomEditor(typeof(ContainerData))]
    public class ContainerDataEditor : UnityEditor.Editor
    {
        private int[,] _container;
        
        Vector2Int _newContainerSize = Vector2Int.zero;

        bool _show;
        
        int _groupNumber;

        private SerializedProperty _filterProperty;
        
        public void OnEnable()
        {
            ContainerData containerData = (ContainerData)target;

            var loadedObject = ObjectHandler.Load<ContainerDataModel>(containerData.GetInstanceID());
            if (!(loadedObject is null))
            {
                containerData.LoadModel(loadedObject);    
            }
            
            _container = containerData.Container;
            
            _newContainerSize = new Vector2Int(containerData.Width, containerData.Height);

            _filterProperty = serializedObject.FindProperty("filter");
        }

        public override void OnInspectorGUI()
        {

            ContainerData containerData = (ContainerData)target;

            containerData.containerName = EditorGUILayout.TextField("Container Name", containerData.containerName);

            EditorGUILayout.PropertyField(_filterProperty, new GUIContent("Filter"));
            
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
            
            _groupNumber = EditorGUILayout.IntField("Group", math.clamp(_groupNumber, 0, 99));
            
            if (GUILayout.Button("Set To Group"))
            {
                SetToGroup(_container, _groupNumber);
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            EditorGUILayout.HelpBox("Slot customization guide. \n0 Is an empty slot location. \n1 Is a single slot and will be spaced out from the other solo slots. \nAnything greater than 1 is a group. All groups must be in a square and will display an incorrect layout if groups are not properly set. A new number needs to be used for each group.", MessageType.Info);
            
            DisplayContainer(_container);

            EditorGUILayout.Separator();
            
            if (GUILayout.Button("Save Container"))
            {
                containerData.SetContainer(_container);
                ObjectHandler.Save(containerData, containerData.GetInstanceID());
            }

            serializedObject.ApplyModifiedProperties();
        }

        
        /// <summary>
        /// Display container Size
        /// </summary>
        private void DisplayContainerSize()
        {
            _newContainerSize = EditorGUILayout.Vector2IntField("Container Size", _newContainerSize);

            _newContainerSize.x = _newContainerSize.x < 1 ? 1 : _newContainerSize.x;
            _newContainerSize.y = _newContainerSize.y < 1 ? 1 : _newContainerSize.y;
        }

        
        /// <summary>
        /// Sets the container to specific group
        /// </summary>
        /// <param name="containerToModify">Container to be modified</param>
        /// <param name="group">Group to set container to</param>
        private static void SetToGroup(int[,] containerToModify, int group)
        {
            for (int x = 0; x < containerToModify.GetLength(0); x++)
            for (int y = 0; y < containerToModify.GetLength(1); y++)
            {
                containerToModify[x, y] = group;
            }
        }

        
        /// <summary>
        /// Update container size
        /// </summary>
        private void UpdateContainerSize()
        {
            var oldContainer = _container;
            
            _container = new int[_newContainerSize.x, _newContainerSize.y];

            _container = CopyToContainer(_container, oldContainer);
        }

        /// <summary>
        /// Copy container data
        /// </summary>
        /// <param name="containerToModify">Container to be modified</param>
        /// <param name="dataToCopy">Container to copy</param>
        /// <returns>Container Matrix</returns>
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

        /// <summary>
        /// Show Container
        /// </summary>
        /// <param name="container">Container Matrix</param>
        private void DisplayContainer(int[,] container)
        {
            _show = EditorGUILayout.BeginFoldoutHeaderGroup(_show, "Show Container Data");
            
            if (_show)
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
