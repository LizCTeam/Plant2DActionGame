using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : BasicBehaviour
{
    [SerializeField, Header("HPアイコン")]
    private GameObject playerIcon;

    [SerializeField] private Player player;
    private int beforeHp;
    

    protected override void OnAwake()
    {
        base.OnAwake();
        //プレイヤーのスクリプトを取得(多分)
        //player = GetComponent<Player>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnStart()
    {
        base.OnStart();
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
            GameObject playerHPObj = Instantiate(playerIcon, transform);
            //GameObject playerHPObj = Instantiate(playerIcon);
            //playerHPObj.transform.SetParent(transform);//親(HPオブジェクト)に設定
        }
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();
        ShowHPIcon();
    }

    private void ShowHPIcon()
    {
        int currentHp = player.GetHp();

        //HPが変化しなければ何もしない
        if (beforeHp == currentHp) return;

        //すべてのHPアイコンを取得
        Image[] icon = transform.GetComponentsInChildren<Image>();



        if (currentHp > beforeHp)
        {
            int diff = currentHp - beforeHp;
            for (int i = 0; i < diff; i++)
            {
                //HPが増えた場合、アイコンを追加生成
                GameObject playerHpObj = Instantiate(playerIcon, transform);
            }
        }

        //HPが減った場合
        //現在のHP以下のアイコンのみ表示、それ以外は非表示
        for (int i = 0; i < icon.Length; i++)
        {
            icon[i].gameObject.SetActive(i < currentHp);
        }

        //現在のHPを保存
        beforeHp = currentHp;
    }
}
