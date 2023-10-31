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

    [SerializeField, Header("回転量")]
    private int MoveAngle;

    private DataManager dataManager;        //データマネージャー

    private bool isStart = false;           //ロケット開始

    private int ownerTapNum = 0;            //それぞれのタップ回数
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;     //それぞれのタップをやめた後のクールタイム
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;   //それぞれクールタイム中か
    private bool isClientCoolTime = false;

    //連続で反応しない
    private bool ownerFirst = true;
    private bool clientFirst = true;
    private bool OwnerCoolTimeFirst = true;
    private bool ClientCoolTimeFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //データマネージャー取得
        dataManager = ManagerAccessor.Instance.dataManager;

        if (dataManager.isOwnerInputKey_CB)
        {
            isStart = true;
        }

        if (isStart)
        {
            //それぞれの連打処理
            if (dataManager.isOwnerInputKey_CB)
            {
                if (ownerFirst)
                {
                    ownerTapNum += MoveAngle;
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
                    clientTapNum += MoveAngle;
                    clientFirst = false;
                    clientCoolTimeCount = 0;
                    Debug.Log("bbb");
                }
            }
            else
            {
                clientFirst = true;
                clientCoolTimeCount++;
            }

            //タップ回数の差
            int dis = ownerTapNum - clientTapNum;
            //移動量の倍率
            float mag = 0;

            //クールタイム内に連打しないと落ちる
            if (ownerCoolTimeCount >= CoolTime)
            {
                if (OwnerCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, true);
                    OwnerCoolTimeFirst = false;
                }
            }
            else
            {
                if (!OwnerCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, false);
                    OwnerCoolTimeFirst = true;
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
                if (!ClientCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, false);
                    ClientCoolTimeFirst = true;
                    Debug.Log("ccc");
                }
            }

            //倍率設定
            if (!isOwnerCoolTime && !isClientCoolTime)
                mag = 2;
            else if (!isOwnerCoolTime)
                mag = 1;
            else if (!isClientCoolTime)
                mag = 1;
            else
                mag = 0;

            //角度設定
            float rad = dis * Mathf.Deg2Rad; //角度をラジアン角に変換

            Debug.Log(isOwnerCoolTime+":"+ isClientCoolTime);

            //移動方向設定
            Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);
            Vector2 input;
            input.x = transform.position.x + power.x * MovePower;
            input.y = transform.position.y + power.y * MovePower;

            if (isOwnerCoolTime && isClientCoolTime)
            {
                input.y = transform.position.y - 0.005f;
                Debug.Log("aaa");
            }

            //移動量、角度の代入
            transform.position = input;
            transform.eulerAngles = new Vector3(0, 0, -dis);
        }
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
