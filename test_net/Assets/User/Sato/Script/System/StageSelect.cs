using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageSelect : MonoBehaviourPunCallbacks
{

    [SerializeField, Header("フェード速度")] private int FeedSpeed;

    [SerializeField, Header("移動するシーン名")] private string sceneName;


    private bool isOwnerEnter = false;  //P1が入った時
    private bool isClientEnter = false; //P2が入った時

    //ゴールに触れているかどうか
    private bool isOwnerFadeStart = false;
    private bool isClientFadeStart = false;

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //ステージに入る
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            MoveStage(isOwnerEnter, ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP);
        else
            MoveStage(isClientEnter, ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP);

        //ゴール後のフェード処理
        if (isOwnerFadeStart)
        {
            //押すべきボタンの画像非表示
            ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("aaa");
            if (ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
        }

        if (isClientFadeStart)
        {
            //押すべきボタンの画像非表示
            ManagerAccessor.Instance.dataManager.player2.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("aaa");
            if (ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
        }
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

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, true);
                photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, true);
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, true);
                photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //押すべきボタンの画像非表示
            collision.transform.GetChild(0).gameObject.SetActive(false);

            photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, false);
            photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, false);

        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像非表示
            collision.transform.GetChild(0).gameObject.SetActive(false);

            photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, false);
            photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, false);

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

    //feed共有
    [PunRPC]
    private void RpcShareFadeStart(bool isOwner)
    {
        if (isOwner)
        {
            isOwnerFadeStart = true;
        }
        else
        {
            isClientFadeStart = true;
        }
    }


    [PunRPC]
    private void RpcShareIsNotOpenBox(bool isOwner, bool data)
    {
        if (isOwner)
            ManagerAccessor.Instance.dataManager.isOwnerNotOpenBox = data;
        else
            ManagerAccessor.Instance.dataManager.isClientNotOpenBox = data;
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
                    //シーン移動開始
                    photonView.RPC(nameof(RpcShareStart), RpcTarget.All);

                    //フェード開始
                    if (PhotonNetwork.IsMasterClient)
                        photonView.RPC(nameof(RpcShareFadeStart), RpcTarget.All, true);
                    else
                        photonView.RPC(nameof(RpcShareFadeStart), RpcTarget.All, false);

                    first = false;
                }
            }
        }
    }
}
