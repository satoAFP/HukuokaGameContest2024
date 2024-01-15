using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�|�[�Y���j���[")] private GameObject pauseMenu;

    private bool isMenuOpen = false;    //���j���[���J���Ă��邩�ǂ���

    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //�I�[�i�[�̃{�^�����͏���
        if (PhotonNetwork.IsMasterClient)
        {
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKeyPause)
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcShareIsMenuOpen), RpcTarget.All, !isMenuOpen);
                    first = false;
                }
            }
            else
                first = true;
        }
        //�N���C�A���g�̃{�^�����͏���
        else
        {
            if (ManagerAccessor.Instance.dataManager.isClientInputKeyPause ||
                ManagerAccessor.Instance.dataManager.isClear &&
                ManagerAccessor.Instance.dataManager.isDeth) 
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcShareIsMenuOpen), RpcTarget.All, !isMenuOpen);
                    first = false;
                }
            }
            else
                first = true;
        }

        //�|�[�Y��ʂ̕\��
        if (isMenuOpen)
        {
            pauseMenu.SetActive(true);
            ManagerAccessor.Instance.dataManager.isPause = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            ManagerAccessor.Instance.dataManager.isPause = false;
        }
    }

    [PunRPC]
    private void RpcShareIsMenuOpen(bool data)
    {
        if (!ManagerAccessor.Instance.dataManager.isClear &&
            !ManagerAccessor.Instance.dataManager.isDeth)
            isMenuOpen = data;
        else
            isMenuOpen = false;
    }
}
