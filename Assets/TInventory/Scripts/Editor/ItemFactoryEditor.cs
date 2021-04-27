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
        private ReorderableList _itemPrefabs;

        private ReorderableList _items;

        private void OnEnable()
        {
            ItemFactory itemFactory = (ItemFactory) target;
            
            // Init
            _itemPrefabs = new ReorderableList(serializedObject, 
                serializedObject.FindProperty("itemPrefabs"), 
                true, true, true, true);
            _items = new ReorderableList(serializedObject, serializedObject.FindProperty("items"), 
                false, true, true, true);

            // Elements
            _itemPrefabs.drawElementCallback = (rect, index, isActive, isFocused) => DrawPrefabElements(index, rect);
            _items.drawElementCallback = (rect, index, isActive, isFocused) => DrawItemElements(index, rect);

            // Headers
            _itemPrefabs.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Item Prefabs");
            _items.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Items");
            
            // On Remove
            _itemPrefabs.onRemoveCallback = RemoveItem;
            _items.onRemoveCallback = RemoveItem;

            // Can Add
            _itemPrefabs.onCanAddCallback = reorderableList =>
                (_itemPrefabs.count < Enum.GetNames(typeof(ItemPrefab.ItemPrefabType)).Length);
            
            // Add
            _itemPrefabs.onAddCallback = reorderableList => AddItem(itemFactory);
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _itemPrefabs.DoLayoutList();
            _items.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawItemElements(int index, Rect rect)
        {
            var element = _items.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                element, GUIContent.none);
        }

        private void DrawPrefabElements(int index, Rect rect)
        {
            var element = _itemPrefabs.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(
                new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight)
                , ((ItemPrefab.ItemPrefabType) element.FindPropertyRelative("type").intValue).ToString());

            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y, rect.width - 90, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("prefab"), GUIContent.none);
        }
        

        
        private static void RemoveItem(ReorderableList l)
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete this?", "Yes", "No"))
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
    }
}
