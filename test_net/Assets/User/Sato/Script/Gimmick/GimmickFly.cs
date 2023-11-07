using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickFly : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("プレイヤー1の画像")]
    private Sprite p1Img;

    [SerializeField, Header("プレイヤー2の画像")]
    private Sprite p2Img;

    [SerializeField, Header("移動量")]
    private float MovePower;

    [SerializeField, Header("回転量")]
    private int MoveAngle;

    [SerializeField, Header("落下までのクールタイム")]
    private int CoolTime;

    [SerializeField, Header("重力加速度")]
    private float Gravity;

    [SerializeField, Header("重力加速度最大値")]
    private float GravityMax;

    private DataManager dataManager;        //データマネージャー

    private GameObject player1;              //プレイヤーオブジェクト取得用
    private GameObject player2;

    private bool isStart = false;           //ロケット開始

    private int ownerTapNum = 0;            //それぞれのタップ回数
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;     //それぞれのタップをやめた後のクールタイム
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;   //それぞれクールタイム中か
    private bool isClientCoolTime = false;

    private float gravity = 0;              //重力

    private bool isHit = false;             //そぞれぞのプレイヤーがロケットに触れているかどうか

    private bool isOwnerStart = false;      //そぞれぞのプレイヤーがロケット操作開始中かどうか
    private bool isClientStart = false;

    private bool isRight = false;           //ロケットの左右に乗り込んでいるか
    private bool isLeft = false;

    //連続で反応しない
    private bool startFirst = true;
    private bool startOtherFirst = true;
    private bool ownerFirst = true;
    private bool clientFirst = true;
    private bool OwnerCoolTimeFirst = true;
    private bool ClientCoolTimeFirst = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //データマネージャー取得
        dataManager = ManagerAccessor.Instance.dataManager;

        //ロケットに触れている状態でB入力で発射待機状態
        if (dataManager.isOwnerInputKey_CB)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (isHit)
                {
                    if (startFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, true);
                        transform.GetChild(2).gameObject.SetActive(true);

                        //プレイヤーのパラメータ変更
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = true;
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = false;
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<SpriteRenderer>().enabled = false;
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        ManagerAccessor.Instance.dataManager.player1.transform.Find("Main Camera").gameObject.SetActive(false);


                        startFirst = false;
                    }
                }
            }
            else
            {
                if (isHit)
                {
                    if (startFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, true);
                        transform.GetChild(2).gameObject.SetActive(true);

                        //プレイヤーのパラメータ変更
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = true;
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = false;
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<SpriteRenderer>().enabled = false;
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        ManagerAccessor.Instance.dataManager.player2.transform.Find("Main Camera").gameObject.SetActive(false);

                        startFirst = false;
                    }
                }
            }
        }
        else
            startFirst = true;

        //それぞれロケット発射状態の時、別の画面の自身のオブジェクトも非表示にする
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if(isClientStart)
            {
                if (startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = true;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = false;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<SpriteRenderer>().enabled = false;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    startOtherFirst = false;
                }
            }
            else
            {
                if (!startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = false;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = true;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<SpriteRenderer>().enabled = true;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

                    startOtherFirst = true;
                }
            }
        }
        else
        {
            if (isOwnerStart)
            {
                if (startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = true;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = false;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<SpriteRenderer>().enabled = false;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    startOtherFirst = false;
                }
            }
            else
            {
                if (!startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = false;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = true;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<SpriteRenderer>().enabled = true;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

                    startOtherFirst = true;
                }
            }
        }

        //二人とも発射状態になるとロケットスタート
        if (isOwnerStart && isClientStart)
        {
            isStart = true;
        }
        else
        {
            //片方だけ発射状態の時は降りることも出来る
            if (dataManager.isOwnerInputKey_CA)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    if (isHit)
                    {
                        if (startFirst)
                        {
                            photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, false);
                            transform.GetChild(2).gameObject.SetActive(false);

                            //プレイヤーのパラメータ変更
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = false;
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<SpriteRenderer>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                            ManagerAccessor.Instance.dataManager.player1.transform.Find("Main Camera").gameObject.SetActive(true);

                            startFirst = false;
                        }
                    }
                }
                else
                {
                    if (isHit)
                    {
                        if (startFirst)
                        {
                            photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, false);
                            transform.GetChild(2).gameObject.SetActive(false);

                            //プレイヤーのパラメータ変更
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = false;
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<SpriteRenderer>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                            ManagerAccessor.Instance.dataManager.player2.transform.Find("Main Camera").gameObject.SetActive(true);

                            startFirst = false;
                        }
                    }
                }
            }
            else
                startFirst = true;
        }

        //乗っているときの画像の表示
        if (isOwnerStart)
        {
            if (!isRight)
            {
                if (ManagerAccessor.Instance.dataManager.isHitRight)
                {
                    transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = p1Img;
                    transform.GetChild(1).gameObject.SetActive(true);

                    photonView.RPC(nameof(RpcShareIsRight), RpcTarget.All, true);
                }
            }
            else
            {
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = p1Img;
                transform.GetChild(0).gameObject.SetActive(true);
            }

            if (!isLeft)
            {
                if (ManagerAccessor.Instance.dataManager.isHitLeft)
                {
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = p1Img;
                    transform.GetChild(0).gameObject.SetActive(true);

                    photonView.RPC(nameof(RpcShareIsLeft), RpcTarget.All, false);
                }
            }
            else
            {
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = p1Img;
                transform.GetChild(1).gameObject.SetActive(true);
            }

        }
        else
        {
            if (isRight)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                photonView.RPC(nameof(RpcShareIsRight), RpcTarget.All, false);
            }

            if (isLeft)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                photonView.RPC(nameof(RpcShareIsLeft), RpcTarget.All, false);
            }
        }


        if (isOwnerStart)
        {
            if (!isRight)
            {
                if (ManagerAccessor.Instance.dataManager.isHitRight)
                {
                    transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = p2Img;
                    transform.GetChild(1).gameObject.SetActive(true);

                    photonView.RPC(nameof(RpcShareIsRight), RpcTarget.All, true);
                }
            }
            else
            {
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = p2Img;
                transform.GetChild(0).gameObject.SetActive(true);
            }

            if (!isLeft)
            {
                if (ManagerAccessor.Instance.dataManager.isHitLeft)
                {
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = p2Img;
                    transform.GetChild(0).gameObject.SetActive(true);

                    photonView.RPC(nameof(RpcShareIsLeft), RpcTarget.All, false);
                }
            }
            else
            {
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = p2Img;
                transform.GetChild(1).gameObject.SetActive(true);
            }

        }
        else
        {
            if (isRight)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                photonView.RPC(nameof(RpcShareIsRight), RpcTarget.All, false);
            }

            if (isLeft)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                photonView.RPC(nameof(RpcShareIsLeft), RpcTarget.All, false);
            }
        }


        if (isStart)
        {
            //それぞれの連打処理
            if (dataManager.isOwnerInputKey_CB)
            {
                if (ownerFirst)
                {
                    ownerTapNum += MoveAngle;
                    ownerFirst = false;
                    ownerCoolTimeCount = 0;
                }
            }
            else
            {
                ownerFirst = true;
                ownerCoolTimeCount++;
            }

            if (dataManager.isClientInputKey_CB)
            {
                if (clientFirst)
                {
                    clientTapNum += MoveAngle;
                    clientFirst = false;
                    clientCoolTimeCount = 0;
                }
            }
            else
            {
                clientFirst = true;
                clientCoolTimeCount++;
            }

            //タップ回数の差
            int dis = ownerTapNum - clientTapNum;
            //移動量の倍率
            float mag = 0;

            //クールタイム内に連打しないと落ちる
            if (ownerCoolTimeCount >= CoolTime)
            {
                if (OwnerCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, true);
                    OwnerCoolTimeFirst = false;
                }
            }
            else
            {
                if (!OwnerCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, false);
                    OwnerCoolTimeFirst = true;
                }
            }

            if (clientCoolTimeCount >= CoolTime)
            {
                if (ClientCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, true);
                    ClientCoolTimeFirst = false;
                }
            }
            else
            {
                if (!ClientCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, false);
                    ClientCoolTimeFirst = true;
                }
            }

            //倍率設定
            if (!isOwnerCoolTime && !isClientCoolTime)
                mag = 2;
            else if (!isOwnerCoolTime || !isClientCoolTime) 
                mag = 1;
            else
                mag = 0;

            //角度設定
            float rad = dis * Mathf.Deg2Rad; //角度をラジアン角に変換

            //移動方向設定
            Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);
            Vector2 input;
            input.x = transform.position.x + power.x * MovePower;
            input.y = transform.position.y + power.y * MovePower;

            //お互い入力が無いとき落下する
            if (isOwnerCoolTime && isClientCoolTime)
            {
                //重力加速の最大値設定
                if (gravity < GravityMax)
                    gravity += Gravity;

                //重力加算
                input.y = transform.position.y - gravity;
            }
            else
            {
                gravity = 0;
            }

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //移動量、角度の代入
                transform.position = input;
                transform.eulerAngles = new Vector3(0, 0, -dis);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

                isHit = true;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

                isHit = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                if (!isOwnerStart)
                    isHit = false;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                if (!isClientStart)
                    isHit = false;
            }
        }
    }



    [PunRPC]
    private void RpcShareIsOwnerCoolTime(bool data)
    {
        isOwnerCoolTime = data;
    }

    [PunRPC]
    private void RpcShareIsClientCoolTime(bool data)
    {
        isClientCoolTime = data;
    }

    [PunRPC]
    private void RpcShareIsOwnerStart(bool data)
    {
        isOwnerStart = data;
    }

    [PunRPC]
    private void RpcShareIsClientStart(bool data)
    {
        isClientStart = data;
    }

    [PunRPC]
    private void RpcShareIsRight(bool data)
    {
        isRight = data;
    }

    [PunRPC]
    private void RpcShareIsLeft(bool data)
    {
        isLeft = data;
    }

    //[PunRPC]
    //private void RpcSetP1Img(int objNum)
    //{
    //    isLeft = data;
    //}
}
