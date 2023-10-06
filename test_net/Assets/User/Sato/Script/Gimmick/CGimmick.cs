using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CGimmick : MonoBehaviourPunCallbacks
{
    protected const int PLAYER1 = 1;
    protected const int PLAYER2 = 2;

    protected bool p1_Button = false;
    protected bool p2_Button = false;


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
