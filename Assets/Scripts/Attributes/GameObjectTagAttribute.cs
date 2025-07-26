#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEngine;
using UnityEditor; 

/// <summary>
/// 문자열 필드를 태그 드롭다운으로 보여주기 위한 어트리뷰트
/// </summary>
public class GameObjectTagAttribute : PropertyAttribute
{
}


[CustomPropertyDrawer(typeof(GameObjectTagAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "TagSelector only valid on string.");
            return;
        }

        string[] tags = InternalEditorUtility.tags;
        string currentTag = property.stringValue;

        int selectedIndex = Mathf.Max(0, System.Array.IndexOf(tags, currentTag));
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, tags);

        property.stringValue = tags[selectedIndex];
    }
}
#endif