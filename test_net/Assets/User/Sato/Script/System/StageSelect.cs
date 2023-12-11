using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageSelect : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("移動するシーン名")] private string sceneName;


    private bool isOwnerEnter = false;  //P1が入った時
    private bool isClientEnter = false; //P2が入った時
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //ステージに入る
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            MoveStage(isOwnerEnter, ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP);
        else
            MoveStage(isClientEnter, ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, true);
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //押すべきボタンの画像非表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, false);
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //押すべきボタンの画像非表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, false);
            }
        }
    }

    //シーン切り替え情報共有
    [PunRPC]
    private void RpcShareIsOwnerEnter(bool data)
    {
        isOwnerEnter = data;
    }

    //シーン切り替え情報共有
    [PunRPC]
    private void RpcShareIsClientEnter(bool data)
    {
        isClientEnter = data;
    }

    //シーン切り替え情報共有
    [PunRPC]
    private void RpcShareStart()
    {
        if (PhotonNetwork.IsMasterClient)
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName(sceneName);
    }

    /// <summary>
    /// ステージに移動
    /// </summary>
    /// <param name="player">ownerかclientか</param>
    private void MoveStage(bool player,bool input)
    {
        //ownerかclientか
        if (player)
        {
            //それぞれ入力しているか
            if (input)
            {
                //一回しか入らない
                if (first)
                {
                    photonView.RPC(nameof(RpcShareStart), RpcTarget.All);
                    first = false;
                }
            }
        }
    }
}
