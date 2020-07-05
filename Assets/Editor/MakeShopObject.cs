using UnityEngine;
using UnityEditor;
public class MakeShopObject
{
    [MenuItem("Assets/Create/ShopObject")]
    public static void CreateShopObject()
    {
        ShopItem asset = ScriptableObject.CreateInstance<ShopItem>();
        AssetDatabase.CreateAsset(asset, "Assets/NewShopItem.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }       
}
