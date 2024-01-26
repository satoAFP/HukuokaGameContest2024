using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("ポーズメニュー")] private GameObject pauseMenu;

    [SerializeField, Header("決定SE")] AudioClip pouseSE;

    private bool isMenuOpen = false;    //メニューが開いているかどうか

    AudioSource audioSource;

    private bool first = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        //オーナーのボタン入力処理
        if (PhotonNetwork.IsMasterClient)
        {
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKeyPause)
            {
                if (first)
                {
                    audioSource.PlayOneShot(pouseSE);
                    photonView.RPC(nameof(RpcShareIsMenuOpen), RpcTarget.All, !isMenuOpen);
                    first = false;
                }
            }
            else
                first = true;
        }
        //クライアントのボタン入力処理
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

        //ポーズ画面の表示
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
