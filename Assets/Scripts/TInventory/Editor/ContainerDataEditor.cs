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
        Vector2Int _newContainerSize = Vector2Int.zero;

        bool _show;
        
        int _groupNumber;

        private SerializedProperty _filterProperty;
        
        
        
        public void OnEnable()
        {
            ContainerData containerData = (ContainerData)target;

            /*var loadedObject = ObjectHandler.Load<ContainerDataModel>(containerData.GetInstanceID());
            if (!(loadedObject is null))
            {
                containerData.LoadModel(loadedObject);    
            }*/
            
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
            
            if (GUILayout.Button("Update Container Size"))
            {
                UpdateContainerSize(containerData);

            }
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginHorizontal();
            
            _groupNumber = EditorGUILayout.IntField("Group", math.clamp(_groupNumber, 0, 99));
            
            if (GUILayout.Button("Set To Group"))
            {
                SetToGroup(_groupNumber, containerData);
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            EditorGUILayout.HelpBox("Slot customization guide. \n0 Is an empty slot location. \n1 Is a single slot and will be spaced out from the other solo slots. \nAnything greater than 1 is a group. All groups must be in a square and will display an incorrect layout if groups are not properly set. A new number needs to be used for each group.", MessageType.Info);
            
            DisplayContainer(containerData);

            EditorGUILayout.Separator();
            
            if (GUILayout.Button("Save Container"))
            {
                EditorUtility.SetDirty(containerData);
                AssetDatabase.SaveAssets();
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
        private static void SetToGroup(int group, ContainerData containerData)
        {
            for (int x = 0; x < containerData.Width; x++)
            for (int y = 0; y < containerData.Height; y++)
            {
                containerData.Container[x + y * containerData.Width] = group;
            }
        }

        
        /// <summary>
        /// Update container size
        /// </summary>
        private void UpdateContainerSize(ContainerData containerData)
        {
            var oldContainer = containerData.Container;

            containerData.Container = new int[_newContainerSize.x * _newContainerSize.y];

            containerData.Container = CopyToContainer(containerData.Container, _newContainerSize,oldContainer, new Vector2(containerData.Width, containerData.Height));
            
            containerData.Width = _newContainerSize.x;
            containerData.Height = _newContainerSize.y;
        }

        /// <summary>
        /// Copy container data
        /// </summary>
        /// <param name="containerToModify">Container to be modified</param>
        /// <param name="dataToCopy">Container to copy</param>
        /// <returns>Container Matrix</returns>
        private int[] CopyToContainer(int[] containerToModify, Vector2 newSize, int[] dataToCopy, Vector2 oldSize)
        {

            for (int x = 0; x < newSize.x; x++)
            for (int y = 0; y < newSize.y; y++)
            {
                if (x < oldSize.x &&
                    y < oldSize.y)
                {
                    containerToModify[(int) (x + y * newSize.x)] = dataToCopy[(int) (x + y * oldSize.x)];
                }
            }

            return containerToModify;
        }

        /// <summary>
        /// Show Container
        /// </summary>
        /// <param name="container">Container Matrix</param>
        private void DisplayContainer(ContainerData containerData)
        {
            _show = EditorGUILayout.BeginFoldoutHeaderGroup(_show, "Show Container Data");

            if (_show)
            {
                for (int y = 0; y < containerData.Height; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < containerData.Width; x++)
                    {
                        containerData.Container[x + y * containerData.Width] = EditorGUILayout.IntField(containerData.Container[x + y * containerData.Width]);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
