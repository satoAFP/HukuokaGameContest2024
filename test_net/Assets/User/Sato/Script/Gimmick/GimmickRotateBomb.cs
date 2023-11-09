using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRotateBomb : MonoBehaviour
{
    //データマネージャー取得用
    DataManager dataManager = null;

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


    //連続で入らないため
    private bool first = true;

    

    // Update is called once per frame
    void Update()
    {
        //データマネージャー取得
        dataManager = ManagerAccessor.Instance.dataManager;

        Debug.Log(count);

        //入力されていない時全てを初期化する
        if (!dataManager.isOwnerInputKey_C_R_RIGHT && !dataManager.isOwnerInputKey_C_R_LEFT &&
            !dataManager.isOwnerInputKey_C_R_UP && !dataManager.isOwnerInputKey_C_R_DOWN)
        {
            count = 0;
            isStop = true;
            first = true;

            isRight = false;
            isLeft = false;
            isUp = false;
            isDown = false;
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
        if (hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitRight)
        {
            RightRotate();
        }
        else if (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitLeft)
        {
            LeftRotate();
        }

        //想定の反対方向に回転した時リセット用
        if (count - memCount == 2)
            count = 0;

        //正しく回転した時
        if (count >= 4)
        {
            transform.position += new Vector3(-0.1f, 0, 0);
            count = 0;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(false);

            hitOwner = false;
        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(false);

            hitClient = false;
        }
    }

    //右回転処理
    private void RightRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT && isRight)
        {
            if (isRight)
                count++;

            isRight = false;
            isDown = true;
        }

        if (dataManager.isOwnerInputKey_C_R_DOWN && isDown)
        {
            if (isDown)
                count++;

            isDown = false;
            isLeft = true;
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT && isLeft)
        {
            if (isLeft)
                count++;

            isLeft = false;
            isUp = true;
        }

        if (dataManager.isOwnerInputKey_C_R_UP && isUp)
        {
            if (isUp)
                count++;

            isUp = false;
            isRight = true;
        }
    }

    //左回転処理
    private void LeftRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT && isRight)
        {
            if (isRight)
                count++;

            isRight = false;
            isUp = true;
        }

        if (dataManager.isOwnerInputKey_C_R_UP && isUp)
        {
            if (isUp)
                count++;

            isUp = false;
            isLeft = true;
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT && isLeft)
        {
            if (isLeft)
                count++;

            isLeft = false;
            isDown = true;
        }


        if (dataManager.isOwnerInputKey_C_R_DOWN && isDown)
        {
            if (isDown)
                count++;

            isDown = false;
            isRight = true;
        }

        
        
    }


}
