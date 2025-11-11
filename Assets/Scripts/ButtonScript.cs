using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    
    [SerializeField,Header("ÉVÅ[Éìñº")] private string sceneName;

    public void OnButtonPress()
    {
        SceneManager.LoadScene(sceneName);
    }
}
