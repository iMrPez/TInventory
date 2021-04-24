using TInventory.Item;
using UnityEditor;
using UnityEngine;

namespace TInventory.Editor
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ItemData itemData = (ItemData) target;
            
            
            EditorGUILayout.BeginHorizontal();
                itemData.image = (Sprite)EditorGUILayout.ObjectField(itemData.image, typeof(Sprite), GUILayout.Height(100), GUILayout.Width(100));
                
                EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("Item Data",EditorStyles.boldLabel, GUILayout.MaxWidth(100));
                    itemData.itemName = EditorGUILayout.TextField("Name",itemData.itemName);
                    itemData.shortName = EditorGUILayout.TextField(new GUIContent("Short Name", "Short name is displayed when the full item name wont fit."), itemData.shortName);
                    itemData.id = EditorGUILayout.IntField(new GUIContent("Id", "This is the items unique identifier and should be unique to this item."), itemData.id);
                    itemData.itemPrefabType = (ItemPrefab.ItemPrefabType)EditorGUILayout.EnumPopup(itemData.itemPrefabType, GUILayout.MaxWidth(150));
                EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            itemData.maxImageSize = EditorGUILayout.Vector2Field("Max Image Size", itemData.maxImageSize);
            itemData.size = EditorGUILayout.Vector2Field(new GUIContent("Item Slot Size", "The amount of slots the item takes up."), itemData.size);
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.HelpBox("Setting the max item count to 1 will hide the item count when displayed.", MessageType.Info);
            itemData.maxCount = EditorGUILayout.IntField("Max Item Count", itemData.maxCount);
            if (itemData.maxCount < 1) itemData.maxCount = 1;
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("Description");
            itemData.description = EditorGUILayout.TextArea(itemData.description, GUILayout.MaxHeight(75));
            
            if (GUILayout.Button("Save Item"))
            {
                EditorUtility.SetDirty(itemData);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
