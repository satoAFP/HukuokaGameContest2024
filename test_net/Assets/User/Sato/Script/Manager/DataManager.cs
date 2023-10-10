using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviourPunCallbacks
{
    //���ꂼ��̃N���A��
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

    //�L�[���͏��
    [System.NonSerialized] public bool isOwnerInputKey_A = false;
    [System.NonSerialized] public bool isOwnerInputKey_D = false;
    [System.NonSerialized] public bool isOwnerInputKey_W = false;
    [System.NonSerialized] public bool isOwnerInputKey_S = false;
    [System.NonSerialized] public bool isClientInputKey_A = false;
    [System.NonSerialized] public bool isClientInputKey_D = false;
    [System.NonSerialized] public bool isClientInputKey_W = false;
    [System.NonSerialized] public bool isClientInputKey_S = false;





    //���ꂼ��̃{�^�����͏�
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


}
