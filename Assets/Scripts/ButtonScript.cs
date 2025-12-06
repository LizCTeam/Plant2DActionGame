using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : BasicBehaviour
{
    
    [SerializeField,Header("シーン名")] private string sceneName;

    public void OnButtonPress()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        //unityエディターでの動作
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //実際のゲーム終了処理
        Application.Quit();
#endif
    }
}
