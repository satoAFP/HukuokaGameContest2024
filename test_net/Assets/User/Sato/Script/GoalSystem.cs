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
        //text.text = ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear + ":" + ManagerAccessor.Instance.dataManager.GetSetIsClientClear;
        //Debug.Log(ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear + ":" + ManagerAccessor.Instance.dataManager.GetSetIsClientClear);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, 1);

        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, 0);
        }
    }

    [PunRPC]
    private void RpcClearCheck(int master)
    {
        //�I�[�i�[�̎�
        if (master == 1)
        {
            ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear = true;
            text.text = "�I�[�i�[�ŃN���A";
            Debug.Log("c");
        }
        else
        {
            ManagerAccessor.Instance.dataManager.GetSetIsClientClear = true;
            text.text = "�I�[�i�[����Ȃ��N���A";
            Debug.Log("d");
        }
    }

}
