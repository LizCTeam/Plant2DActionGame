using UnityEngine;
using UnityEngine.SceneManagement;

public static class ContinuationChange
{
    public static string _sceneName;

    //「このメソッドが実行された時に開いているシーンの名前」を取得する。
    public static void CurrentSceneName()
    {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    //上記のメソッドで取得されたシーンに戻す。
    public static void BackToBeforeScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
