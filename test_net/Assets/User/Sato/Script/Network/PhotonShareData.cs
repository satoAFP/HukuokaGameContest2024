using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotonShareData : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView photonView;

    public string _text;

    public string Text
    {
        get { return _text; }
        set { _text = value; ManagerAccessor.Instance.dataManager.RequestOwner(); }
    }

    void Awake()
    {
        this.photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        ManagerAccessor.Instance.dataManager.chat.text = Text;
        ManagerAccessor.Instance.dataManager.clear.text = ManagerAccessor.Instance.dataManager.isOwnerClear + ":" + ManagerAccessor.Instance.dataManager.isClientClear;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("aaa");
        // オーナーの場合
        if (stream.IsWriting)
        {
            stream.SendNext(this._text);
            stream.SendNext(ManagerAccessor.Instance.dataManager.isOwnerClear);
            stream.SendNext(ManagerAccessor.Instance.dataManager.isClientClear);
        }
        // オーナー以外の場合
        else
        {
            this._text = (string)stream.ReceiveNext();
            ManagerAccessor.Instance.dataManager.isOwnerClear = (bool)stream.ReceiveNext();
            ManagerAccessor.Instance.dataManager.isClientClear = (bool)stream.ReceiveNext();
        }
    }

    
}

