using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Access to the specific object
        ItemData itemData = (ItemData)target;

        // Basic data
        EditorGUILayout.LabelField("ID", EditorStyles.boldLabel);
        itemData.uniqueID = EditorGUILayout.TextField("Unique ID", itemData.uniqueID);
        itemData.itemName = EditorGUILayout.TextField("Item Name", itemData.itemName);
        itemData.scientificName = EditorGUILayout.TextField("Scientific Name", itemData.scientificName);
        itemData.itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemData.itemType);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Visual", EditorStyles.boldLabel);
        itemData.icon = (Sprite)EditorGUILayout.ObjectField("Icon", itemData.icon, typeof(Sprite), false);
       

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Economy", EditorStyles.boldLabel);
        itemData.sellValue = EditorGUILayout.IntField("Sell Value", itemData.sellValue);

        EditorGUILayout.Space();

        // Show Aditional Data
        if (itemData.itemType == ItemType.Consumable)
        {
            EditorGUILayout.LabelField("Consumable Properties", EditorStyles.boldLabel);
            itemData.speedMultiplier = (float)EditorGUILayout.FloatField("Speed Multiplier", itemData.speedMultiplier);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
            itemData.description = EditorGUILayout.TextArea(itemData.description, GUILayout.Height(60));
        }

        // Save Changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(itemData);
        }
    }
}
