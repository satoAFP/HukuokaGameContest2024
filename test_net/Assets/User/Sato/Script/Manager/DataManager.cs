using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviourPunCallbacks
{
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


    //キー入力情報
    [System.NonSerialized] public bool isOwnerInputKey_A = false;
    [System.NonSerialized] public bool isOwnerInputKey_D = false;
    [System.NonSerialized] public bool isOwnerInputKey_W = false;
    [System.NonSerialized] public bool isOwnerInputKey_S = false;
    [System.NonSerialized] public bool isOwnerInputKey_B = false;

    [System.NonSerialized] public bool isOwnerInputKey_LM = false;

    [System.NonSerialized] public bool isOwnerInputKey_CA = false;
    [System.NonSerialized] public bool isOwnerInputKey_CB = false;
    [System.NonSerialized] public bool isOwnerInputKey_CX = false;
    [System.NonSerialized] public bool isOwnerInputKey_CY = false;

    [System.NonSerialized] public bool isOwnerInputKey_C_L_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_LEFT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_UP = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_DOWN  = false;

    [System.NonSerialized] public bool isOwnerInputKey_C_D_RIGHT = false;//パッドの十字キー
    [System.NonSerialized] public bool isOwnerInputKey_C_D_LEFT  = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_UP    = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_DOWN  = false;


    [System.NonSerialized] public bool isClientInputKey_A = false;
    [System.NonSerialized] public bool isClientInputKey_D = false;
    [System.NonSerialized] public bool isClientInputKey_W = false;
    [System.NonSerialized] public bool isClientInputKey_S = false;
    [System.NonSerialized] public bool isClientInputKey_B = false;

    [System.NonSerialized] public bool isClientInputKey_LM = false;

    [System.NonSerialized] public bool isClientInputKey_CA = false;
    [System.NonSerialized] public bool isClientInputKey_CB = false;
    [System.NonSerialized] public bool isClientInputKey_CX = false;
    [System.NonSerialized] public bool isClientInputKey_CY = false;

    [System.NonSerialized] public bool isClientInputKey_C_L_RIGHT = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_LEFT = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_UP = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_DOWN = false;

    [System.NonSerialized] public bool isClientInputKey_C_D_RIGHT = false;//パッドの十字キー
    [System.NonSerialized] public bool isClientInputKey_C_D_LEFT  = false;
    [System.NonSerialized] public bool isClientInputKey_C_D_UP    = false;
    [System.NonSerialized] public bool isClientInputKey_C_D_DOWN  = false;




    //それぞれのボタン入力状況
    [System.NonSerialized] public string ownerName = "";
    [System.NonSerialized] public string clientName = "";

    //public bool GetSetIsOwnerButton
    //{
    //    get { return isOwnerButton; }
    //    set { isOwnerButton = value; }
    //}

    //public bool GetSetIsClientButton
    //{
    //    get { return isClientButton; }
    //    set { isClientButton = value; }
    //}



    public Text text;

    public Text chat;

    public Text clear;


    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.dataManager = this;
    }


    //プレイヤー取得用関数
    public GameObject GetPlyerObj(string name)
    {
        //プレイヤー取得
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");

        //それぞれ名前が一致したら返す
        if (p[0].name == name)
            return p[0];
        else if (p[1].name == name)
            return p[1];
        else
            return null;
    }


}
