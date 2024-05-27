using UnityEditor;
using UnityEngine;

public class BatterAttributesCreator
{
    [MenuItem("Baseball/Vreate BatterAttributes")]
    public static void CreateBatterAttributes()
    {
        BatterAttributes asset = ScriptableObject.CreateInstance<BatterAttributes>();
        AssetDatabase.CreateAsset(asset, "Assets/BatterScriptable/Attributes/BatterAttributes.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
