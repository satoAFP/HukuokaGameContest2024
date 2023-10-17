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

    //�v���C���[�擾�p�֐�
    public GameObject GetPlyerObj(string name)
    {
        //�v���C���[�擾
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");

        //���ꂼ�ꖼ�O����v������Ԃ�
        if (p[0].name == name)
            return p[0];
        else if (p[1].name == name)
            return p[1];
        else
            return null;
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
