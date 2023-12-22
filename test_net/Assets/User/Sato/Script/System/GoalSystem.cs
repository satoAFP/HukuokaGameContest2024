using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class GoalSystem : CGimmick
{
    [SerializeField, Header("フェード速度")] private int FeedSpeed;

    [SerializeField, Header("フェード終了後のシーン切り替え速度")] private int FeedEndSpeed;

    [SerializeField, Header("ゴールSE")] AudioClip goalSE;

    [SerializeField, Header("SEを鳴らす回数")] private int SEPlayNum;

    [SerializeField, Header("SEを鳴らす間隔")] private int SEInterbal;

    private AudioSource audioSource;

    //ゴールに触れているかどうか
    private bool isOwnerEnter = false;
    private bool isClientEnter = false;

    //フェードが終わったかどうか
    private bool isOwnerFadeEnd = false;
    private bool isClientFadeEnd = false;

    private int frameCount = 0;
    private int SECount = 0;

    private bool first = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //触れているとき十字キー上
        if (isOwnerEnter)
        {
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP)
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER1);
                    first = false;
                }
            }
        }
        if (isClientEnter)
        {
            if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP)
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER2);
                    first = false;
                }
            }
        }

        //ゴール後のフェード処理
        if (ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear)
        {
            //押すべきボタンの画像非表示
            ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(false);

            if (ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
            else
                isOwnerFadeEnd = true;

            if(PhotonNetwork.IsMasterClient)
            {
                SECount++;
                if (SECount <= SECount * SEPlayNum)
                {
                    if (SECount % SEInterbal == 0)
                    {
                        audioSource.PlayOneShot(goalSE);
                    }
                }
            }
        }

        if (ManagerAccessor.Instance.dataManager.GetSetIsClientClear)
        {
            //押すべきボタンの画像非表示
            ManagerAccessor.Instance.dataManager.player2.transform.GetChild(0).gameObject.SetActive(false);

            if (ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
            else
                isClientFadeEnd = true;

            //SE再生
            if (!PhotonNetwork.IsMasterClient)
            {
                SECount++;
                if (SECount <= SECount * SEPlayNum)
                {
                    if (SECount % SEInterbal == 0)
                    {
                        audioSource.PlayOneShot(goalSE);
                    }
                }
            }
        }

        //クリアするとリザルト画面表示
        if (isOwnerFadeEnd && isClientFadeEnd)
        {
            frameCount++;
            if (frameCount == FeedEndSpeed)
                ManagerAccessor.Instance.dataManager.isClear = true;
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
                isOwnerEnter = true;
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
                isClientEnter = true;
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

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, false);
                isOwnerEnter = false;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //押すべきボタンの画像非表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, false);
                isClientEnter = false;
            }
        }
    }

    [PunRPC]
    private void RpcClearCheck(int master)
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

    [PunRPC]
    protected void RpcShareIsNotOpenBox(bool isOwner, bool data)
    {
        if (isOwner)
            ManagerAccessor.Instance.dataManager.isOwnerNotOpenBox = data;
        else
            ManagerAccessor.Instance.dataManager.isClientNotOpenBox = data;
    }

}
