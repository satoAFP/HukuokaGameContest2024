using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class GoalSystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text text;


    // Update is called once per frame
    void Update()
    {
        text.text = ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear + ":" + ManagerAccessor.Instance.dataManager.GetSetIsClientClear;
        //Debug.Log(ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear + ":" + ManagerAccessor.Instance.dataManager.GetSetIsClientClear);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            //オーナーの時
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                photonView.RPC(nameof(RpcClearCheck), RpcTarget.Others, 1);
            }
            else
            {
                photonView.RPC(nameof(RpcClearCheck), RpcTarget.Others, 0);
            }

            
        }
    }

    [PunRPC]
    private void RpcClearCheck(int master)
    {
        //オーナーの時
        if (master == 1)
        {
            ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear = true;
            Debug.Log("master");
        }
        else
        {
            ManagerAccessor.Instance.dataManager.GetSetIsClientClear = true;
            Debug.Log("not-master");
        }
    }

}
