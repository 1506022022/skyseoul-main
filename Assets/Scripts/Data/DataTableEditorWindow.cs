using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SODataTableEditor : EditorWindow
{
    private ScriptableObject targetSO;
    private Vector2 scrollPos;
    private FieldInfo rowsField;
    private IList rowsList;
    private Type rowType;

    [MenuItem("Tools/SO DataTable Editor")]
    public static void Init()
    {
        GetWindow<SODataTableEditor>("SO Data Table");
    }

    private void OnGUI()
    {
        DrawTargetSelector();

        if (targetSO != null && rowsList != null && rowType != null)
        {
            DrawToolbar();
            GUILayout.Space(5);
            DrawTable();
        }
    }
    //private void OnDestroy()
    //{
    //    if (targetSO != null && rowsField != null && rowsList != null)
    //    {
    //        rowsList.Clear();
    //        rowsField.SetValue(targetSO, rowsList);

    //        EditorUtility.SetDirty(targetSO);
    //        AssetDatabase.SaveAssets();
    //    }

    //    targetSO = null;
    //    rowsField = null;
    //    rowsList = null;
    //    rowType = null;
    //}
    private void DrawTargetSelector()
    {
        var newSO = (ScriptableObject)EditorGUILayout.ObjectField("Target SO", targetSO, typeof(ScriptableObject), false);

        if (newSO != targetSO)
        {
            targetSO = newSO;
            if (targetSO != null)
            {
                rowsField = targetSO.GetType().GetField("rows", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (rowsField != null)
                {
                    rowsList = rowsField.GetValue(targetSO) as IList;
                    if (rowsList == null)
                    {
                        Debug.LogWarning("rows 필드가 비어 있어 초기화합니다.");
                        var listType = rowsField.FieldType;
                        rowsList = (IList)Activator.CreateInstance(listType);
                        rowsField.SetValue(targetSO, rowsList);
                    }

                    if (rowsList.Count > 0)
                    {
                        rowType = rowsList[0].GetType();
                    }
                    else
                    {
                        
                        if (rowsField.FieldType.IsArray)
                            rowType = rowsField.FieldType.GetElementType();
                        else if (rowsField.FieldType.IsGenericType)
                            rowType = rowsField.FieldType.GetGenericArguments()[0];
                    }
                }
            }
        }
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Load CSV", EditorStyles.toolbarButton))
        {
            string path = EditorUtility.OpenFilePanel("Load CSV", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                var method = typeof(CsvLoader).GetMethod("LoadCsv").MakeGenericMethod(rowType);
                var loadedList = method.Invoke(null, new object[] { path }) as IList;

                rowsList.Clear();
                foreach (var item in loadedList)
                    rowsList.Add(item);
            }
        }

        if (GUILayout.Button("Save CSV", EditorStyles.toolbarButton))
        {
            string path = EditorUtility.SaveFilePanel("Save CSV", "", targetSO.name + ".csv", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                var method = typeof(CsvLoader).GetMethod("SaveCsv").MakeGenericMethod(rowType);
                method.Invoke(null, new object[] { path, rowsList });
            }
        }

        if (GUILayout.Button("+ Add Row", EditorStyles.toolbarButton))
        {
            rowsList.Add(Activator.CreateInstance(rowType));
        }

        if (GUILayout.Button("Save SO", EditorStyles.toolbarButton))
        {
            EditorUtility.SetDirty(targetSO);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTable()
    {
        if (rowsList == null || rowType == null) return;

        var fields = rowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

     
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(30));
        foreach (var f in fields)
        {
            if (!f.IsPublic && f.GetCustomAttribute<SerializeField>() == null)
                continue;
            GUILayout.Label(f.Name, EditorStyles.boldLabel, GUILayout.Width(150));
        }
        GUILayout.Label("Delete", EditorStyles.boldLabel, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        
        int removeIndex = -1;
        for (int i = 0; i < rowsList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(i.ToString(), GUILayout.Width(30)); 

            var row = rowsList[i];
            foreach (var f in fields)
            {
                if (!f.IsPublic && f.GetCustomAttribute<SerializeField>() == null)
                    continue;

                object value = f.GetValue(row);
                object newValue = DrawFieldCell(value, f.FieldType, 150);
                if (!Equals(value, newValue))
                {
                    f.SetValue(row, newValue);
                }
            }

            if (GUILayout.Button("X", GUILayout.Width(50)))
            {
                removeIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex >= 0)
            rowsList.RemoveAt(removeIndex);

        EditorGUILayout.EndScrollView();
    }

    private object DrawFieldCell(object value, Type type, float width)
    {

        if (type == typeof(int))
            return EditorGUILayout.IntField((int)(value ?? 0), GUILayout.Width(width));
        if (type == typeof(float))
            return EditorGUILayout.FloatField((float)(value ?? 0f), GUILayout.Width(width));
        if (type == typeof(string))
            return EditorGUILayout.TextField((string)(value ?? ""), GUILayout.Width(width));
        if (type == typeof(bool))
            return EditorGUILayout.Toggle((bool)(value ?? false), GUILayout.Width(width));
        if (type.IsEnum)
            return EditorGUILayout.EnumPopup((Enum)(value ?? Activator.CreateInstance(type)), GUILayout.Width(width));
        if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            return EditorGUILayout.ObjectField((UnityEngine.Object)value, type, false, GUILayout.Width(width));

     
        if (!type.IsPrimitive && !type.IsEnum && !typeof(UnityEngine.Object).IsAssignableFrom(type))
        {
            if (value == null)
                value = Activator.CreateInstance(type);

            EditorGUILayout.BeginVertical(GUILayout.Width(width));
            var nestedFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var nf in nestedFields)
            {
                if (!nf.IsPublic && nf.GetCustomAttribute<SerializeField>() == null)
                    continue;
                object nestedValue = nf.GetValue(value);
                object newNestedValue = DrawFieldCell(nestedValue, nf.FieldType, width - 10);
                if (!Equals(nestedValue, newNestedValue))
                    nf.SetValue(value, newNestedValue);
            }
            EditorGUILayout.EndVertical();
            return value;
        }

        GUILayout.Label($"(Unsupported)", GUILayout.Width(width));
        return value;
    }
   

}
