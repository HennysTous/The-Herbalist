using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabThumbnailGenerator : EditorWindow
{
    private GameObject[] prefabs;
    private Vector2 scrollPos;

    [MenuItem("Tools/Generate UI Sprites from Prefabs")]
    private static void OpenWindow()
    {
        GetWindow<PrefabThumbnailGenerator>("Prefab → Sprite Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Thumbnail to UI Sprite Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Select Prefabs"))
        {
            string[] guids = Selection.assetGUIDs;
            prefabs = new GameObject[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                prefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
        }

        if (prefabs != null && prefabs.Length > 0)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100));
            foreach (var p in prefabs)
            {
                if (p != null)
                    EditorGUILayout.LabelField(p.name);
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Generate Sprites"))
            {
                GenerateSprites();
            }
        }
    }

    private void GenerateSprites()
    {
        string folderPath = "Assets/GeneratedSprites";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        foreach (var prefab in prefabs)
        {
            if (prefab == null) continue;

            Texture2D preview = AssetPreview.GetAssetPreview(prefab);
            if (preview == null)
            {
                Debug.LogWarning($"No preview available for {prefab.name}");
                continue;
            }

            // Save texture as PNG
            byte[] bytes = preview.EncodeToPNG();
            string filePath = Path.Combine(folderPath, $"{prefab.name}.png");
            File.WriteAllBytes(filePath, bytes);
            Debug.Log($"Saved Sprite: {filePath}");
        }

        AssetDatabase.Refresh();

        // Make the PNGs into sprites
        foreach (var prefab in prefabs)
        {
            string path = Path.Combine("Assets/GeneratedSprites", $"{prefab.name}.png");
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            if (ti != null)
            {
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Single;
                ti.mipmapEnabled = false;
                ti.SaveAndReimport();
            }
        }

        Debug.Log("All prefabs converted to UI sprites.");
    }
}
