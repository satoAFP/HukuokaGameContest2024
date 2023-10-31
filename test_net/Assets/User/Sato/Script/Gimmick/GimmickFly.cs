using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickFly : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("落下までのクールタイム")]
    private int CoolTime;

    [SerializeField, Header("移動量")]
    private float MovePower;

    private DataManager dataManager;

    private int ownerTapNum = 0;
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;
    private bool isClientCoolTime = false;

    private bool ownerFirst = true;
    private bool clientFirst = true;
    private bool OwnerCoolTimeFirst = true;
    private bool ClientCoolTimeFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dataManager = ManagerAccessor.Instance.dataManager;

        if (dataManager.isOwnerInputKey_CB)
        {
            if (ownerFirst)
            {
                ownerTapNum++;
                ownerFirst = false;
                ownerCoolTimeCount = 0;
            }
        }
        else
        {
            ownerFirst = true;
            ownerCoolTimeCount++;
        }

        if (dataManager.isClientInputKey_CB)
        {
            if (clientFirst)
            {
                clientTapNum++;
                clientFirst = false;
                clientCoolTimeCount = 0;
            }
        }
        else
        {
            clientFirst = true;
            clientCoolTimeCount++;
        }


        int dis = ownerTapNum - clientTapNum;
        float mag = 0;

        if(ownerCoolTimeCount>=CoolTime)
        {
            if (OwnerCoolTimeFirst)
            {
                photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, true);
                ClientCoolTimeFirst = false;
            }
        }
        else
        {
            if (!OwnerCoolTimeFirst)
            {
                photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, false);
                ClientCoolTimeFirst = true;
            }
        }

        if (clientCoolTimeCount >= CoolTime)
        {
            if (ClientCoolTimeFirst)
            {
                photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, true);
                ClientCoolTimeFirst = false;
            }
        }
        else
        {
            if (!OwnerCoolTimeFirst)
            {
                photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, false);
                ClientCoolTimeFirst = true;
            }
        }

        if (isOwnerCoolTime && isClientCoolTime) 
            mag = 2;
        else if (isOwnerCoolTime)
            mag = 1;
        else if (isClientCoolTime)
            mag = 1;
        else
            mag = 0;

        float rad = dis * Mathf.Deg2Rad; //角度をラジアン角に変換

        Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);
        Vector2 input;
        input.x = transform.position.x + power.x * MovePower;
        input.y = transform.position.y + power.y * MovePower;

        transform.position = input;
        transform.eulerAngles = new Vector3(0, 0, dis);
    }


    [PunRPC]
    private void RpcShareIsOwnerCoolTime(bool data)
    {
        isOwnerCoolTime = data;
    }

    [PunRPC]
    private void RpcShareIsClientCoolTime(bool data)
    {
        isClientCoolTime = data;
    }

}
