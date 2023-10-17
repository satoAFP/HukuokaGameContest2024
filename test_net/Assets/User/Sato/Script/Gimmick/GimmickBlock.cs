using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //オブジェクトが持ち上がっているとき
    [System.NonSerialized] public bool liftMode = false;

    //プレイヤー取得用
    private GameObject Player;

    //1P、2Pがそれぞれ当たっている判定
    private bool hitOwner = false;
    private bool hitClient = false;

    //ブロックとプレイヤーの距離
    private Vector3 dis = Vector3.zero;

    //連続で反応しないための処理
    private bool first = true;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            Player = GetPlyerObj("Player1");
        else
            Player = GetPlyerObj("Player2");

        //1P、2Pが触れているかつ、アクションしているとき持ち上がる
        if (hitOwner && (ManagerAccessor.Instance.dataManager.isOwnerInputKey_LM || ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB) &&
            hitClient && (ManagerAccessor.Instance.dataManager.isClientInputKey_LM || ManagerAccessor.Instance.dataManager.isClientInputKey_CB))
        {
            if(first)
            {
                //持ち上がった位置に移動
                Vector3 input = gameObject.transform.position;
                input.y += 0.5f;
                gameObject.transform.localPosition = input;

                dis = transform.position - Player.transform.position;
                
                first = false;

                
            }

            //プレイヤーに追従させる
            gameObject.transform.position = dis + Player.transform.position;

            //プレイヤーが動いているとき、ブロックサイドも同期させる
            if (Player.GetComponent<AvatarTransformView>().isPlayerMove)
                GetComponent<AvatarOnlyTransformView>().isPlayerMove = true;
            else
                GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

            liftMode = true;
            Player.GetComponent<PlayerController>().islift = true;
        }
        else
        {
            if (!first)
            {

                //元の高さに戻す
                Vector3 input = gameObject.transform.position;
                input.y -= 0.5f;
                gameObject.transform.localPosition = input;

                dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                first = true;

                
            }

            //同期解除
            GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

            liftMode = false;
            Player.GetComponent<PlayerController>().islift = false;
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //持ち上げていないとき
        if (!liftMode)
        {
            if (collision.gameObject.name == "Player1")
            {
                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                hitClient = false;
            }
        }
    }


}
