using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviourPunCallbacks
{
    [Header("ステージ数")] public int StageNum;

    [Header("ミスまでのフレーム")] public int MissFrame;

    [Header("BGM")] public GameObject BGM;

    [Header("星生成エフェクト")] public GameObject StarEffect;

    //それぞれのクリア状況
    [System.NonSerialized] public bool isOwnerClear = false;
    [System.NonSerialized] public bool isClientClear = false;

    public bool GetSetIsOwnerClear
    {
        get { return isOwnerClear; }
        set { isOwnerClear = value; }
    }

    public bool GetSetIsClientClear
    {
        get { return isClientClear; }
        set { isClientClear = value; }
    }


    //プレイヤーオブジェクト取得
    [System.NonSerialized] public GameObject player1 = null;
    [System.NonSerialized] public GameObject player2 = null;
    [System.NonSerialized] public GameObject copyKey = null;
    [System.NonSerialized] public GameObject board = null;

    //アンロックボタン操作中かどうか
    [System.NonSerialized] public bool isUnlockButtonStart = false;

    //Locket操作中かどうか
    [System.NonSerialized] public bool isFlyStart = false;
    [System.NonSerialized] public Vector3 flyPos = Vector3.zero;

    //コピー鍵出現中かどうか
    [System.NonSerialized] public bool isAppearCopyKey = false;

    //宝箱を開けれない状態
    [System.NonSerialized] public bool isNotOpenBox = false;
    [System.NonSerialized] public bool isOwnerNotOpenBox = false;
    [System.NonSerialized] public bool isClientNotOpenBox = false;

    //ステージ移動中フラグ
    [System.NonSerialized] public bool isStageMove = false;

    //クリアフラグ
    [System.NonSerialized] public bool isClear = false;

    //クリアフラグ
    [System.NonSerialized] public bool isEnterGoal = false;

    //死亡フラグ
    [System.NonSerialized] public bool isDeth = false;

    //ポーズフラグ
    [System.NonSerialized] public bool isPause = false;

    //死亡したプレイヤーの名前を取得
    [System.NonSerialized] public string DeathPlayerName = null;

    //ミスの回数
    [System.NonSerialized] public int ownerMissCount = 0;
    [System.NonSerialized] public int clientMissCount = 0;

    //左右下判定
    [System.NonSerialized] public bool isOwnerHitRight = false;
    [System.NonSerialized] public bool isClientHitRight = false;
    [System.NonSerialized] public bool isOwnerHitLeft = false;
    [System.NonSerialized] public bool isClientHitLeft = false;
    [System.NonSerialized] public bool isOwnerHitDown = false;
    [System.NonSerialized] public bool isClientHitDown = false;


    //キー入力情報(マスター)
    //キーボード入力
    [System.NonSerialized] public bool isOwnerInputKey_A = false;
    [System.NonSerialized] public bool isOwnerInputKey_D = false;
    [System.NonSerialized] public bool isOwnerInputKey_W = false;
    [System.NonSerialized] public bool isOwnerInputKey_S = false;
    [System.NonSerialized] public bool isOwnerInputKey_B = false;
    //マウス入力
    [System.NonSerialized] public bool isOwnerInputKey_LM = false;
    //コントローラーボタン
    [System.NonSerialized] public bool isOwnerInputKey_CA = false;
    [System.NonSerialized] public bool isOwnerInputKey_CB = false;
    [System.NonSerialized] public bool isOwnerInputKey_CX = false;
    [System.NonSerialized] public bool isOwnerInputKey_CY = false;
    //コントローラー左スティック
    [System.NonSerialized] public bool isOwnerInputKey_C_L_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_LEFT  = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_UP    = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_DOWN  = false;
    //コントローラー右スティック
    [System.NonSerialized] public bool isOwnerInputKey_C_R_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R_LEFT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R_UP = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R_DOWN = false;
    //コントローラー十字キー
    [System.NonSerialized] public bool isOwnerInputKey_C_D_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_LEFT  = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_UP    = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_DOWN  = false;
    //コントローラー上ボタン＆トリガー
    [System.NonSerialized] public bool isOwnerInputKey_C_R1 = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R2 = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L1 = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L2 = false;
    //コントローラーポーズ
    [System.NonSerialized] public bool isOwnerInputKeyPause = false;

    //キー入力情報(クライアント)
    //キーボード入力
    [System.NonSerialized] public bool isClientInputKey_A = false;
    [System.NonSerialized] public bool isClientInputKey_D = false;
    [System.NonSerialized] public bool isClientInputKey_W = false;
    [System.NonSerialized] public bool isClientInputKey_S = false;
    [System.NonSerialized] public bool isClientInputKey_B = false;
    //マウス入力
    [System.NonSerialized] public bool isClientInputKey_LM = false;
    //コントローラーボタン
    [System.NonSerialized] public bool isClientInputKey_CA = false;
    [System.NonSerialized] public bool isClientInputKey_CB = false;
    [System.NonSerialized] public bool isClientInputKey_CX = false;
    [System.NonSerialized] public bool isClientInputKey_CY = false;
    //コントローラー左スティック
    [System.NonSerialized] public bool isClientInputKey_C_L_RIGHT = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_LEFT  = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_UP    = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_DOWN  = false;
    //コントローラー右スティック
    [System.NonSerialized] public bool isClientInputKey_C_R_RIGHT = false;
    [System.NonSerialized] public bool isClientInputKey_C_R_LEFT = false;
    [System.NonSerialized] public bool isClientInputKey_C_R_UP = false;
    [System.NonSerialized] public bool isClientInputKey_C_R_DOWN = false;
    //コントローラー十字キー
    [System.NonSerialized] public bool isClientInputKey_C_D_RIGHT = false;//パッドの十字キー
    [System.NonSerialized] public bool isClientInputKey_C_D_LEFT  = false;
    [System.NonSerialized] public bool isClientInputKey_C_D_UP    = false;
    [System.NonSerialized] public bool isClientInputKey_C_D_DOWN  = false;
    //コントローラー上ボタン＆トリガー
    [System.NonSerialized] public bool isClientInputKey_C_R1 = false;
    [System.NonSerialized] public bool isClientInputKey_C_R2 = false;
    [System.NonSerialized] public bool isClientInputKey_C_L1 = false;
    [System.NonSerialized] public bool isClientInputKey_C_L2 = false;
    //コントローラーポーズ
    [System.NonSerialized] public bool isClientInputKeyPause = false;


    //それぞれのボタン入力状況
    [System.NonSerialized] public string ownerName = "";
    [System.NonSerialized] public string clientName = "";


    public Text text;

    public Text chat;

    public Text clear;




    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.dataManager = this;
    }

    private void Update()
    {
        if (ManagerAccessor.Instance.dataManager.player1 != null &&
            ManagerAccessor.Instance.dataManager.player2 != null)
        {
            //スタート、ゴールにいるとき宝箱があかないようにする
            if (ManagerAccessor.Instance.dataManager.isOwnerNotOpenBox ||
                ManagerAccessor.Instance.dataManager.isClientNotOpenBox)
                ManagerAccessor.Instance.dataManager.isNotOpenBox = true;
            else
                ManagerAccessor.Instance.dataManager.isNotOpenBox = false;
        }

        //if(Input.GetMouseButton(0))
        //{
        //    Instantiate(ManagerAccessor.Instance.dataManager.StarEffect);
        //}
    }


    //プレイヤー取得用関数
    public GameObject GetPlyerObj(string name)
    {
        //プレイヤー取得
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");

        //それぞれ名前が一致したら返す
        for (int i = 0; i < p.Length; i++) 
        {
            if (p[i].name == name)
                return p[i];
        }

        return null;
    }

    
}
