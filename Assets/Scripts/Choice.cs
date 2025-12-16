using UnityEngine;
using UnityEngine.UI;

public class Choice : BasicBehaviour
{
    [SerializeField] private Button choiceButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnStart()
    {
        base.OnStart();
        //ボタンが選択された状態になる
        choiceButton.Select();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
