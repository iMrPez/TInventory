using System;
using TInventory.Item;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TInventory.Editor
{
    [CustomEditor(typeof(ItemFactory))]
    public class ItemFactoryEditor : UnityEditor.Editor
    {
        private ReorderableList list;

        private void OnEnable()
        {
            ItemFactory itemFactory = (ItemFactory) target;
            
            list = new ReorderableList(serializedObject, 
                serializedObject.FindProperty("itemPrefabs"), 
                true, true, true, true);

            list.drawElementCallback = (rect, index, isActive, isFocused) => DrawElements(index, rect);

            list.drawHeaderCallback = GetHeaderTitle;
            
            list.onRemoveCallback = RemoveItem;

            list.onCanAddCallback = reorderableList =>
                (list.count < Enum.GetNames(typeof(ItemPrefab.ItemPrefabType)).Length);
            
            list.onAddCallback = reorderableList => AddItem(itemFactory);
        }

        private void DrawElements(int index, Rect rect)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(
                new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight)
                , ((ItemPrefab.ItemPrefabType) element.FindPropertyRelative("type").intValue).ToString());

            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y, rect.width - 90, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("prefab"), GUIContent.none);
        }

        
        private static void GetHeaderTitle(Rect rect)
        {
            EditorGUI.LabelField(rect, "Item Prefabs");
        }

        
        private static void RemoveItem(ReorderableList l)
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete this item prefab?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        }

        
        private static void AddItem(ItemFactory itemFactory)
        {
            ItemPrefab.ItemPrefabType type = ItemPrefab.ItemPrefabType.Basic;

            foreach (var prefabType in (ItemPrefab.ItemPrefabType[]) Enum.GetValues(typeof(ItemPrefab.ItemPrefabType)))
            {
                bool matchFound = false;

                foreach (var itemPrefab in (itemFactory.itemPrefabs))
                {
                    if (itemPrefab.type == prefabType)
                    {
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    type = prefabType;
                    break;
                }
            }
            itemFactory.itemPrefabs.Add(new ItemPrefab(type, null));
        }

        
        public override void OnInspectorGUI()
        {   
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
