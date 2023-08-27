using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviourPunCallbacks
{
    //���g���I�[�i�[���ǂ���
    [System.NonSerialized] public bool isOwner = false;

    //���ꂼ��̃N���A��
    [System.NonSerialized] public bool isOwnerClear = false;
    [System.NonSerialized] public bool isClientClear = false;

    public bool GetSetIsOwnerClear
    {
        get { return isOwnerClear; }
        set { isOwnerClear = value; RequestOwner(); }
    }

    public bool GetSetIsClientClear
    {
        get { return isClientClear; }
        set { isClientClear = value; RequestOwner(); }
    }



    public Text text;

    public Text chat;

    public Text clear;

    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.dataManager = this;
    }

    //�I�[�i�[�łȂ��Ă��f�[�^�ύX�\�ɂ���
    public void RequestOwner()
    {
        if (this.photonView.IsMine == false)
        {
            if (this.photonView.OwnershipTransfer != OwnershipOption.Request)
                Debug.LogError("OwnershipTransfer��Request�ɕύX���Ă��������B");
            else
                this.photonView.RequestOwnership();
        }
    }
}
