using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickRotateBomb : MonoBehaviourPunCallbacks
{
    //データマネージャー取得用
    DataManager dataManager = null;

    [SerializeField, Header("移動量")] private float MovePower;

    [SerializeField, Header("座標更新タイミング")] private int PosUpData;

    [SerializeField, Header("ボタン表示終了までのフレーム")] private int DisplayTime;

    [SerializeField, Header("一つの入力にいれるのフレーム")] private int rotateSpeed;

    //コピーキーのみ取得用
    private GameObject copyKeyObj = null;
    private string hitObjName = null;

    //1P、2Pがそれぞれ当たっている判定
    private bool hitOwner = false;
    private bool hitClient = false;

    //現在の入力している方向
    private bool isRight = false;
    private bool isLeft = false;
    private bool isUp = false;
    private bool isDown = false;

    //入力されていない状況
    private bool isStop = false;

    //順番に入力されるとカウントされる
    private int count = 0;

    //移動方向
    private Vector3 movePower = Vector3.zero;

    //移動開始
    private bool isOwnerMoveStart = false;
    private bool isClientMoveStart = false;

    //frameカウント
    private int frameCount = 0;

    //点滅しないためのボタン表示が消えるまでのラグカウント
    private int displayTimeCount = 0;

    //回転速度を図るためのカウント
    private int rotateSpeedCount = 0;

    //連続で入らないため
    private bool first = true;
    private bool first1 = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //データマネージャー取得
        dataManager = ManagerAccessor.Instance.dataManager;


        //入力されていない時全てを初期化する
        if (!dataManager.isOwnerInputKey_C_R_RIGHT && !dataManager.isOwnerInputKey_C_R_LEFT &&
            !dataManager.isOwnerInputKey_C_R_UP && !dataManager.isOwnerInputKey_C_R_DOWN)
        {
            if (first1)
            {
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
                else
                    photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);
                first1 = false;
            }

            count = 0;
            rotateSpeedCount = 0;
            isStop = true;
            first = true;

            isRight = false;
            isLeft = false;
            isUp = false;
            isDown = false;
        }
        else
        {
            first1 = true;
        }

        //入力されていないとき、最初に入力された方向から回転が始まる
        if (isStop)
        {
            if (dataManager.isOwnerInputKey_C_R_RIGHT)
            {
                if (first)
                {
                    isRight = true;
                    isStop = false;
                    first = false;
                }
            }

            if (dataManager.isOwnerInputKey_C_R_LEFT)
            {
                if (first)
                {
                    isLeft = true;
                    isStop = false;
                    first = false;
                }
            }

            if (dataManager.isOwnerInputKey_C_R_UP)
            {
                if (first)
                {
                    isUp = true;
                    isStop = false;
                    first = false;
                }
            }

            if (dataManager.isOwnerInputKey_C_R_DOWN)
            {
                if (first)
                {
                    isDown = true;
                    isStop = false;
                    first = false;
                }
            }
        }

        //前フレームのcount記憶用
        int memCount = count;

        //回転入力情報取得処理
        if (hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitRight && dataManager.isOwnerInputKey_C_L_RIGHT && dataManager.isClientInputKey_C_L_RIGHT)
        {
            RightRotate();
            movePower.x = MovePower;
        }
        else if (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitLeft && dataManager.isOwnerInputKey_C_L_LEFT && dataManager.isClientInputKey_C_L_LEFT)
        {
            LeftRotate();
            movePower.x = -MovePower;
        }

        //想定の反対方向に回転した時リセット用
        if (count - memCount == 2)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
            else
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);

            count = 0;
        }

        Debug.Log("isOwnerMoveStart" + isOwnerMoveStart + "isClientMoveStart" + isClientMoveStart);

        //正しく回転した時
        if (count >= 4)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, true);
            else
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, true);

            count = 0;
        }

        //移動開始
        if (isOwnerMoveStart && isClientMoveStart) 
        {
            transform.position += movePower;
            frameCount++;
        }

        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //座標同期
            if (frameCount == PosUpData)
            {
                photonView.RPC(nameof(RpcSharePos), RpcTarget.Others, transform.position.x, transform.position.y);
                frameCount = 0;
            }

            //画像の非表示のラグを発生させる
            if(!hitOwner)
            {
                displayTimeCount++;
                if (displayTimeCount == DisplayTime)
                {
                    if (PhotonNetwork.IsMasterClient)
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
                    else
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);

                    if (hitObjName == "Player1")
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(1).gameObject.SetActive(false);
                    if (hitObjName == "CopyKey")
                        copyKeyObj.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            //画像の非表示のラグを発生させる
            if (!hitClient)
            {
                displayTimeCount++;
                if (displayTimeCount == DisplayTime)
                {
                    if (PhotonNetwork.IsMasterClient)
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
                    else
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);

                    ManagerAccessor.Instance.dataManager.player2.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //押すべきボタンの画像表示
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (dataManager.isOwnerHitRight)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickRight;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateR;
                }

                if (dataManager.isOwnerHitLeft)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickLeft;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateL;
                }

            }

            hitOwner = true;
            displayTimeCount = 0;
        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像表示
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {

                if (dataManager.isClientHitRight)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickRight;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateR;
                }

                if (dataManager.isClientHitLeft)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickLeft;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateL;
                }
            }

            hitClient = true;
            displayTimeCount = 0;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            hitOwner = false;
            hitObjName = collision.gameObject.name;
        }

        if (collision.gameObject.name == "CopyKey") 
        {
            copyKeyObj = collision.gameObject;
        }

        if (collision.gameObject.name == "Player2")
        {
            hitClient = false;
        }
    }

    //右回転処理
    private void RightRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT)
        {
            if (isRight)
            {
                count++;
                rotateSpeedCount = 0;

                isRight = false;
                isDown = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_DOWN)
        {
            if (isDown)
            {
                count++;
                rotateSpeedCount = 0;

                isDown = false;
                isLeft = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT)
        {
            if (isLeft)
            {
                count++;
                rotateSpeedCount = 0;

                isLeft = false;
                isUp = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_UP)
        {
            if (isUp)
            {
                count++;
                rotateSpeedCount = 0;

                isUp = false;
                isRight = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }
    }

    //左回転処理
    private void LeftRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT)
        {
            if (isRight)
            {
                count++;
                rotateSpeedCount = 0;

                isRight = false;
                isUp = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_UP)
        {
            if (isUp)
            {
                count++;
                rotateSpeedCount = 0;

                isUp = false;
                isLeft = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT)
        {
            if (isLeft)
            {
                count++;
                rotateSpeedCount = 0;

                isLeft = false;
                isDown = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }


        if (dataManager.isOwnerInputKey_C_R_DOWN)
        {
            if (isDown)
            {
                count++;
                rotateSpeedCount = 0;

                isDown = false;
                isRight = true;
            }

            //一定時間動かさないと回転していないとみなされる
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }
    }

    [PunRPC]
    private void RpcSharePos(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    [PunRPC]
    private void RpcShareIsMoveStart(bool isManage, bool data)
    {
        if (isManage)
            isOwnerMoveStart = data;
        else
            isClientMoveStart = data;
    }
}
