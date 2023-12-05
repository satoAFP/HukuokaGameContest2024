using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageSelect : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�ړ�����V�[����")] private string sceneName;


    private bool isOwnerEnter = false;  //P1����������
    private bool isClientEnter = false; //P2����������
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //�X�e�[�W�ɓ���
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            MoveStage(isOwnerEnter, ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP);
        else
            MoveStage(isClientEnter, ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1") 
        {
            photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, true);
        }

        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, false);
        }

        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, false);
        }
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareIsOwnerEnter(bool data)
    {
        isOwnerEnter = data;
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareIsClientEnter(bool data)
    {
        isClientEnter = data;
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareStart()
    {
        if (PhotonNetwork.IsMasterClient)
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName(sceneName);
    }

    /// <summary>
    /// �X�e�[�W�Ɉړ�
    /// </summary>
    /// <param name="player">owner��client��</param>
    private void MoveStage(bool player,bool input)
    {
        //owner��client��
        if (player)
        {
            //���ꂼ����͂��Ă��邩
            if (input)
            {
                //��񂵂�����Ȃ�
                if (first)
                {
                    photonView.RPC(nameof(RpcShareStart), RpcTarget.All);
                    first = false;
                }
            }
        }
    }
}
