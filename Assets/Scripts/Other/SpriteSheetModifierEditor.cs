#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SpriteSheetModifierEditor : EditorWindow
{
    
    public Texture2D spriteSheet;
    public Vector2 pivotInPixels = new Vector2(32, 32);
    public bool setCustomPivot = true;
    public bool setBorders = false;
    public Vector4 spriteBorders = new Vector4(0, 0, 0, 0);
    public string customSpriteName = "MySprite";

    [MenuItem("Tools/Sprite Sheet Modifier")]
    public static void ShowWindow()
    {
        GetWindow<SpriteSheetModifierEditor>("Sprite Sheet Modifier");
    }

    void OnGUI()
    {
        GUILayout.Label("Sprite Sheet Modifier", EditorStyles.boldLabel);

        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);
        pivotInPixels = EditorGUILayout.Vector2Field("Pivot in Pixels", pivotInPixels);
        setCustomPivot = EditorGUILayout.Toggle("Set Custom Pivot", setCustomPivot);
        setBorders = EditorGUILayout.Toggle("Set Borders", setBorders);
        spriteBorders = EditorGUILayout.Vector4Field("Sprite Borders", spriteBorders);
        customSpriteName = EditorGUILayout.TextField("Custom Sprite Name", customSpriteName);

        if (GUILayout.Button("Modify Sprites"))
        {
            ModifySprites();
        }
    }

    public void ModifySprites()
    {
        if (spriteSheet == null)
        {
            Debug.LogError("No sprite sheet assigned.");
            return;
        }

        string path = AssetDatabase.GetAssetPath(spriteSheet);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        if (textureImporter == null)
        {
            Debug.LogError("Couldn't load texture importer.");
            return;
        }

        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        SpriteMetaData[] spritesData = textureImporter.spritesheet;

        for (int i = 0; i < spritesData.Length; i++)
        {
            if (setCustomPivot)
            {
                Vector2 pivotNormalized = new Vector2(
                    pivotInPixels.x / spritesData[i].rect.width,
                    pivotInPixels.y / spritesData[i].rect.height
                );
                spritesData[i].pivot = pivotNormalized;
            }

            if (setBorders)
            {
                spritesData[i].border = spriteBorders;
            }

            spritesData[i].name = $"{customSpriteName}_{i}";
        }

        textureImporter.spritesheet = spritesData;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log("Sprite sheet modified successfully.");
    }
}
#endif
