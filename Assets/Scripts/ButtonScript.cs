using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonScript : BasicBehaviour
{
    [HideInInspector]
    [SerializeField] private string sceneName;

    //#if UNITY_EDITOR ～ #endif で囲まれた部分はエディタ上でのみ有効になる
#if UNITY_EDITOR
    //インスペクタに表示するためのSceneAsset型変数
    [Header("遷移先のシーン選択")]//インスペクタに見出しを表示
    //ここにシーンファイルをドラッグ＆ドロップする
    [SerializeField] private SceneAsset _sceneAsset;
#endif

    public void OnButtonPress()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            if (Time.timeScale == 0) Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("遷移先のシーン名が設定されていません!");
        }
    }

    //OnValidateは、インスペクタ上で値が変更されたときに呼び出される
#if UNITY_EDITOR
    private void OnValidate()
    {
        //sceneAssetが設定されていれば、その名前をsceneNameに代入
        if (_sceneAsset != null)
        {
            sceneName = _sceneAsset.name;
        }
        else
        {
            sceneName = "";
        }
    }
#endif

    //コンテニュウ用のメソッド
    public void OnCotinueButtonClick()
    {
        ContinuationChange.BackToBeforeScene();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        //Unityエディター上での動作
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //実際のゲーム終了処理
        Application.Quit();
#endif
    }
}
