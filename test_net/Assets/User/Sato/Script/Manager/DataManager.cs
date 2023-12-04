using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviourPunCallbacks
{
    [Header("ステージ数")] public int StageNum;

    [Header("ミスまでのフレーム")] public int MissFrame;

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

    //アンロックボタン操作中かどうか
    [System.NonSerialized] public bool isUnlockButtonStart = false;

    //コピー鍵出現中かどうか
    [System.NonSerialized] public bool isAppearCopyKey = false;

    //クリアフラグ
    [System.NonSerialized] public bool isClear = false;

    //死亡フラグ
    [System.NonSerialized] public bool isDeth = false;

    //ミスの回数
    /*[System.NonSerialized]*/ public int ownerMissCount = 0;
    /*[System.NonSerialized]*/ public int clientMissCount = 0;

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


    //それぞれのボタン入力状況
    [System.NonSerialized] public string ownerName = "";
    [System.NonSerialized] public string clientName = "";


    public Text text;

    public Text chat;

    public Text clear;

    private int ownerMemCount = 0;
    private int clientMemCount = 0;

    private bool ownerFirst = true;
    private bool clinetFirst = true;


    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.dataManager = this;
    }

    private void Update()
    {
        //2Pにミスデータ共有
        if(PhotonNetwork.IsMasterClient)
        {
            if (ownerMemCount != ManagerAccessor.Instance.dataManager.ownerMissCount)
            {
                photonView.RPC(nameof(RpcShareOwnerMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.ownerMissCount);
                ownerMemCount = ManagerAccessor.Instance.dataManager.ownerMissCount;
            }
            if (ownerMemCount != ManagerAccessor.Instance.dataManager.clientMissCount)
            {
                photonView.RPC(nameof(RpcShareClientMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.clientMissCount);
                clientMemCount = ManagerAccessor.Instance.dataManager.clientMissCount;
            }
        }
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

    [PunRPC]
    private void RpcShareOwnerMissCount(int miss)
    {
        ManagerAccessor.Instance.dataManager.ownerMissCount = miss;
    }

    [PunRPC]
    private void RpcShareClientMissCount(int miss)
    {
        ManagerAccessor.Instance.dataManager.clientMissCount = miss;
    }
}
