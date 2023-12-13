using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class GimmickFly : MonoBehaviourPunCallbacks
{
    const int START_WAIT_TIME = 20;//入力を受け付けない時間


    [SerializeField, Header("移動量")]
    private float MovePower;

    [SerializeField, Header("回転量")]
    private int MoveAngle;

    [SerializeField, Header("ゴール後の回転量")]
    private float correctionAngle;

    [SerializeField, Header("落下までのクールタイム")]
    private int CoolTime;

    [SerializeField, Header("ゴール後クリア表示までのフレーム")]
    private int GoalTime;


    private DataManager dataManager;        //データマネージャー

    private Rigidbody2D rigidbody;          //リジットボディ

    private GameObject player1;              //プレイヤーオブジェクト取得用
    private GameObject player2;

    private bool isStart = false;           //ロケット開始
    private bool isGoal = false;            //ゴール判定

    private int ownerTapNum = 0;            //それぞれのタップ回数
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;     //それぞれのタップをやめた後のクールタイム
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;   //それぞれクールタイム中か
    private bool isClientCoolTime = false;

    private float dis = 0;                  //タップ回数の差

    private bool isHit = false;             //そぞれぞのプレイヤーがロケットに触れているかどうか

    private bool isOwnerStart = false;      //そぞれぞのプレイヤーがロケット操作開始中かどうか
    private bool isClientStart = false;

    private int startWaitTimeCount = 0;     //ロケット開始時入力をいったん受け付けない
    private int goalCount = 0;              //ゴール後クリアを表示するまでのカウント

    //連続で反応しない
    private bool startFirst = true;
    private bool startOtherFirst = true;
    private bool ownerFirst = true;
    private bool clientFirst = true;
    private bool OwnerCoolTimeFirst = true;
    private bool ClientCoolTimeFirst = true;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        ownerCoolTimeCount = CoolTime;
        clientCoolTimeCount = CoolTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //データマネージャー取得
        dataManager = ManagerAccessor.Instance.dataManager;


        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //ロケットに触れている状態でB入力で発射待機状態
            if (dataManager.isOwnerInputKey_CB)
            {
                if (isHit)
                {
                    if (startFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, true);

                        //プレイヤーのパラメータ変更
                        ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(false);
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = true;
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = false;
                        ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform;


                        startFirst = false;
                    }
                }
            }
            else
                startFirst = true;
        }
        else
        {
            //ロケットに触れている状態でB入力で発射待機状態
            if (dataManager.isClientInputKey_CB)
            {
                if (isHit)
                {
                    if (startFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, true);

                        //プレイヤーのパラメータ変更
                        ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(false);
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = true;
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = false;
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform;

                        startFirst = false;
                    }
                }
            }
            else
                startFirst = true;
        }

        //それぞれロケット発射状態の時、別の画面の自身のオブジェクトも非表示にする
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (isClientStart)
            {
                if (startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(false);
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = true;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = false;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    startOtherFirst = false;
                }
            }
            else
            {
                if (!startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(true);
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = false;
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = true;
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
                    ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(false);
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = true;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = false;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    startOtherFirst = false;
                }
            }
            else
            {
                if (!startOtherFirst)
                {
                    //プレイヤーのパラメータ変更
                    ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(true);
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = false;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = true;
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

                    startOtherFirst = true;
                }
            }
        }

        //二人とも発射状態になるとロケットスタート
        if (isOwnerStart && isClientStart)
        {
            isStart = true;
            ManagerAccessor.Instance.dataManager.isFlyStart = true;

            //重力設定
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            //当たり判定設定
            GetComponent<BoxCollider2D>().isTrigger = false;
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

                            //プレイヤーのパラメータ変更
                            ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(true);
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = false;
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                            GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = ManagerAccessor.Instance.dataManager.player1.transform;

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

                            //プレイヤーのパラメータ変更
                            ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(true);
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = false;
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<SpriteRenderer>().enabled = true;
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                            GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = ManagerAccessor.Instance.dataManager.player2.transform;

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
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);
        if (isClientStart)
            transform.GetChild(1).gameObject.SetActive(true);
        else
            transform.GetChild(1).gameObject.SetActive(false);

        //オブジェクト取得更新
        ManagerAccessor.Instance.dataManager.flyPos = transform.position;

        if (!isGoal)
        {
            if (isStart)
            {
                startWaitTimeCount++;
                if (startWaitTimeCount >= START_WAIT_TIME)
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
                    }
                }

                //クールタイムカウント
                ownerCoolTimeCount++;
                clientCoolTimeCount++;

                //タップ回数の差
                dis = ownerTapNum - clientTapNum;
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
                float rad = -dis * Mathf.Deg2Rad; //角度をラジアン角に変換

                //移動方向設定
                Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);

                //移動量加算
                if (!(isOwnerCoolTime && isClientCoolTime))
                {
                    rigidbody.velocity = new Vector2(power.x, power.y);
                }

                if (isOwnerCoolTime)
                    transform.GetChild(3).gameObject.SetActive(false);
                else
                    transform.GetChild(3).gameObject.SetActive(true);

                if (isClientCoolTime)
                    transform.GetChild(2).gameObject.SetActive(false);
                else
                    transform.GetChild(2).gameObject.SetActive(true);

                //親の座標を共有する
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //角度の代入
                    transform.eulerAngles = new Vector3(0, 0, dis * correctionAngle);
                }
            }
        }
        else
        {
            //当たり判定設定
            GetComponent<BoxCollider2D>().isTrigger = true;

            if (dis <= -0.1f || dis >= 0.1f)
            {
                GetComponent<Rigidbody2D>().simulated = false;

                if (dis < 0)
                    dis += 0.1f;
                if (dis > 0)
                    dis -= 0.1f;


                //親の座標を共有する
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //角度の代入
                    transform.eulerAngles = new Vector3(0, 0, dis * 2);
                }
            }
            else
            {

                GetComponent<Rigidbody2D>().simulated = true;
                //本体を真っすぐにする
                transform.eulerAngles = new Vector3(0, 0, 0);
                //飛び立っていく
                rigidbody.velocity = new Vector2(0, 2);
                
                //ゴールしてから一定時間でリザルトを出す
                goalCount++;
                if (goalCount == GoalTime) 
                {
                    ManagerAccessor.Instance.dataManager.isClear = true;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
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

        if (collision.gameObject.tag == "Goal")
        {
            isGoal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
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

}
