using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class GoalSystem : CGimmick
{

    // Update is called once per frame
    void Update()
    {
        //�N���A����ƃ��U���g��ʕ\��
        if (ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear &&
           ManagerAccessor.Instance.dataManager.GetSetIsClientClear)
        {
            ManagerAccessor.Instance.dataManager.isDeth = true;
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
        //�I�[�i�[�̎�
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
