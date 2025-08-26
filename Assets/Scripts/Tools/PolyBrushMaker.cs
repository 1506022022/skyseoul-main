using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class PolybrushMeshSaver : EditorWindow
{
    private GameObject targetObject;
    private string savePath = "Assets/Polybrush_SavedMeshes";

    [MenuItem("Tools/Polybrush/Save Modified Mesh")]
    public static void ShowWindow()
    {
        GetWindow<PolybrushMeshSaver>("Polybrush Mesh Saver");
    }

    private void OnGUI()
    {
        GUILayout.Label("Save Modified Mesh", EditorStyles.boldLabel);

        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Save Modified Mesh to Asset"))
        {
            if (targetObject == null)
            {
                Debug.LogError("대상을 선택하세요.");
                return;
            }

            var mf = targetObject.GetComponent<MeshFilter>();
            if (mf == null)
            {
                Debug.LogError("MeshFilter가 없습니다.");
                return;
            }

            Mesh mesh = mf.sharedMesh;
            if (mesh == null)
            {
                Debug.LogError("Mesh가 없습니다.");
                return;
            }

            // 저장 경로 생성
            if (!AssetDatabase.IsValidFolder(savePath))
                AssetDatabase.CreateFolder("Assets", "Polybrush_SavedMeshes");

            string fileName = targetObject.name + "_SavedMesh.asset";
            string path = Path.Combine(savePath, fileName);
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            Mesh savedMesh = Object.Instantiate(mesh);
            AssetDatabase.CreateAsset(savedMesh, path);
            AssetDatabase.SaveAssets();

            // 적용
            mf.sharedMesh = savedMesh;

            Debug.Log($" 저장 완료: {path}");
        }
    }
}
