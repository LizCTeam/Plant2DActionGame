using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.ProjectWindowCallback;

public static class BasicBehaviourCreator
{
    // メニューに「Create > BasicBehaviour」を追加
    [MenuItem("Assets/Create/BasicBehaviour Script", false, 80)]
    private static void CreateBasicBehaviourScript()
    {
        // 選択中のフォルダパスを取得（なければAssets直下）
        string path = GetSelectedPath();
        string defaultName = "NewBasicBehaviour.cs";

        // 名前入力ダイアログ（リネーム状態）を表示
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            ScriptableObject.CreateInstance<DoCreateBasicBehaviourScript>(),
            Path.Combine(path, defaultName),
            EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
            null
        );
    }

    // 選択中のフォルダパスを取得
    private static string GetSelectedPath()
    {
        string path = "Assets";
        foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}

// 名前入力後に呼ばれる処理
public class DoCreateBasicBehaviourScript : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        // ファイル名からクラス名を抽出
        string fileName = Path.GetFileNameWithoutExtension(pathName);
        string scriptContent = GetScriptTemplate(fileName);

        // ファイルを生成
        File.WriteAllText(pathName, scriptContent);
        AssetDatabase.ImportAsset(pathName);

        // Projectウィンドウで選択状態にする
        var asset = AssetDatabase.LoadAssetAtPath<Object>(pathName);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }

    private string GetScriptTemplate(string className)
    {
        return
$@"using UnityEngine;

public class {className} : BasicBehaviour
{{
    protected override void OnStart()
    {{
        base.OnStart();
    }}

    protected override void OnUpdate()
    {{
        base.OnUpdate();
    }}

    protected override void OnFixedUpdate()
    {{
        base.OnFixedUpdate();
    }}
}}";
    }
}
