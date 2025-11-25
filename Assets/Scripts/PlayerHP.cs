using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField, Header("HPアイコン")]
    private GameObject playerIcon;

    [SerializeField] private Player player;
    private int beforeHp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーのスクリプトを取得(多分)
        //player = GetComponent<Player>();
        beforeHp = player.GetHp();
        CreateHPIcon();
    }

    private void CreateHPIcon()
    {
        //プレイヤーのHP分アイコンを生成
        for (int i = 0; i < player.GetHp(); i++)
        {
            GameObject playerHPObj = Instantiate(playerIcon,transform);
            //GameObject playerHPObj = Instantiate(playerIcon);
            //playerHPObj.transform.SetParent(transform);//親(HPオブジェクト)に設定
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowHPIcon();
    }

    private void ShowHPIcon()
    {


        //HPが変化しなければ何もしない
        if (beforeHp == player.GetHp()) return;

        //HPが減った場合
        //すべてのHPアイコンを取得
        Image[] icon = transform.GetComponentsInChildren<Image>();

        //現在のHP以下のアイコンのみ表示、それ以外は非表示
        for (int i = 0; i < icon.Length; i++)
        {
            icon[i].gameObject.SetActive(i < player.GetHp());
        }

        //現在のHPを保存
        beforeHp = player.GetHp();
    }
}
