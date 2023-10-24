using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class GoalSystem : CGimmick
{
    [SerializeField] private GameObject text;


    // Update is called once per frame
    void Update()
    {
        if (ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear &&
           ManagerAccessor.Instance.dataManager.GetSetIsClientClear)
        {
            text.SetActive(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER1);

        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER2);
        }
    }

    [PunRPC]
    protected void RpcClearCheck(int master)
    {
        //オーナーの時
        if (master == PLAYER1)
        {
            ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear = true;
        }
        else if (master == PLAYER2)
        {
            ManagerAccessor.Instance.dataManager.GetSetIsClientClear = true;
        }
    }

}
