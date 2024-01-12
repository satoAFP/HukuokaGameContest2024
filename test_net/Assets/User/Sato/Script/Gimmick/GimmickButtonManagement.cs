using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickButtonManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    [SerializeField, Header("どのギミックにするか")]
    [Header("0:オブジェクト消失 / 1:オブジェクト出現")]
    private int gimmickNum;

    [SerializeField, Header("成功SE")] AudioClip successSE;
    [SerializeField, Header("失敗SE")] AudioClip failureSE;

    private AudioSource audioSource;

    //成功判定
    private bool isSuccess = false;
    //失敗判定
    private bool isFailure = false;

    //フレームカウント用
    private int count = 0;

    //一回しか入らない
    private bool first = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Gimmickによって扉の開閉を決める
        if (gimmickNum == 0)
            door.SetActive(true);
        if (gimmickNum == 1)
            door.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isSuccess)
        {
            //どちらか片方が入力開始でカウント開始
            if (gimmickButton[0].GetComponent<GimmickButton>().isButton ||
                gimmickButton[1].GetComponent<GimmickButton>().isButton)
            {
                //両方触れている場合
                if ((gimmickButton[0].GetComponent<GimmickButton>().isOwnerHit && gimmickButton[1].GetComponent<GimmickButton>().isClientHit) ||
                    (gimmickButton[1].GetComponent<GimmickButton>().isOwnerHit && gimmickButton[0].GetComponent<GimmickButton>().isClientHit))
                {
                    //失敗した時はいれない
                    if (!isFailure)
                    {
                        count++;
                        //失敗までのフレームまで
                        if (count <= ManagerAccessor.Instance.dataManager.MissFrame)
                        {
                            //同時入力成功でGimmick起動
                            if (gimmickButton[0].GetComponent<GimmickButton>().isButton &&
                                gimmickButton[1].GetComponent<GimmickButton>().isButton)
                            {
                                isSuccess = true;

                                //SE再生
                                audioSource.PlayOneShot(successSE);
                            }
                        }
                        else
                        {
                            //失敗情報送信
                            if (PhotonNetwork.IsMasterClient)
                            {
                                if (gimmickButton[0].GetComponent<GimmickButton>().isButton)
                                {
                                    if (gimmickButton[0].GetComponent<GimmickButton>().isOwnerOnButton)
                                        ManagerAccessor.Instance.dataManager.clientMissCount++;
                                    if (gimmickButton[0].GetComponent<GimmickButton>().isClientOnButton)
                                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                                }
                                if (gimmickButton[1].GetComponent<GimmickButton>().isButton)
                                {
                                    if (gimmickButton[1].GetComponent<GimmickButton>().isOwnerOnButton)
                                        ManagerAccessor.Instance.dataManager.clientMissCount++;
                                    if (gimmickButton[1].GetComponent<GimmickButton>().isClientOnButton)
                                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                                }
                            }

                            //制限時間内にできなければ失敗
                            isFailure = true;
                            count = 0;

                            //SE再生
                            audioSource.PlayOneShot(failureSE);
                        }
                    }
                }
            }
            else
            {
                //両方の入力解除で再度入力受付
                isFailure = false;
            }
        }


        //同時押しが成功すると、扉が開く
        if (isSuccess) 
        {
            if (first)
            {
                if (gimmickNum == 0)
                    door.SetActive(false);
                if (gimmickNum == 1)
                    door.SetActive(true);

                gameObject.transform.Find("Button1").transform.localScale = new Vector2(-1, 1);
                gameObject.transform.Find("Button2").transform.localScale = new Vector2(-1, 1);

                //エフェクト生成
                Instantiate(ManagerAccessor.Instance.dataManager.StarEffect, gameObject.transform.Find("Button1"));
                Instantiate(ManagerAccessor.Instance.dataManager.StarEffect, gameObject.transform.Find("Button2"));

                first = false;
            }
        }

    }


    //ボタン入力情報を相手に送信
    [PunRPC]
    protected void RpcShareIsSuccess()
    {
        isSuccess = true;
    }



}
