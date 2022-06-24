using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class HierarchySeparator : MonoBehaviour
{
    public Color OutlineColor = Color.white;
    public Color BarColor = Color.black;
    public Color TextColor = Color.white;
    public int OutlineSize = 1;

#if UNITY_EDITOR
    static HierarchySeparator()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    [MenuItem("GameObject/Separator", false, 30)]
    public static void CreateSeparator(MenuCommand menuCommand)
    {
        GameObject separator = new GameObject("Separator");
        separator.AddComponent<HierarchySeparator>();
        GameObjectUtility.SetParentAndAlign(separator, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(separator, "Create " + separator.name);
        Selection.activeObject = separator;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        
        if (gameObject == null) return;
        if (!gameObject.TryGetComponent(out HierarchySeparator hierarchy)) return;

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = hierarchy.TextColor;
        guiStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUI.DrawRect(selectionRect, hierarchy.OutlineColor);
        EditorGUI.DrawRect(new Rect(selectionRect.x + hierarchy.OutlineSize, selectionRect.y + hierarchy.OutlineSize, selectionRect.width - (hierarchy.OutlineSize * 2), selectionRect.height - (hierarchy.OutlineSize * 2)), hierarchy.BarColor);
        EditorGUI.DropShadowLabel(selectionRect, $"{gameObject.name.ToUpperInvariant()}", guiStyle);
    }
	
	void OnValidate()
    {
        EditorApplication.RepaintHierarchyWindow();
    }
#endif
}