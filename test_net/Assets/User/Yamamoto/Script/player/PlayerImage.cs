using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerImage : MonoBehaviourPunCallbacks
{
    private string parentObjectName;//親オブジェクトの名前を取得する
    //プレイヤー画像
    //プレイヤー1
    [SerializeField, Header("宝箱")]
    private Sprite p1Image;
    [SerializeField, Header("空いた宝箱")]
    private Sprite p1OpenImage;
    [SerializeField, Header("持ち上げモーション中の宝箱")]
    private Sprite p1LiftImage;
    //プレイヤー2
    [SerializeField, Header("鍵")]
    private Sprite p2Image;
    [SerializeField, Header("持ち上げモーション中の鍵")]
    private Sprite p2LiftImage;

    private Animator anim;//アニメーター

    // Start is called before the first frame update
    void Start()
    {
     
        //親オブジェクトの名前を取得
        parentObjectName = transform.parent.name;

        //プレイヤーによってイラストを変える
        if (parentObjectName == "Player1")
        {
            GetComponent<SpriteRenderer>().sprite = p1Image;
        }
        if (parentObjectName == "Player2")
        {
            GetComponent<SpriteRenderer>().sprite = p2Image;
        }
        if (parentObjectName == "CopyKey")
        {
            GetComponent<SpriteRenderer>().sprite = p2Image;
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        

        //箱イラスト
        if (parentObjectName == "Player1")
        {
            //プレイヤーの移動した方向に応じてプレイヤーの向きを変える
            if(ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().imageleft)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f);
            }

            //宝箱オープン画像
            if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_boxopenimage)
            {
               // Debug.Log("空く");

                GetComponent<SpriteRenderer>().sprite = p1OpenImage;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = p1Image;
            }

            //ブロック持ち上げ画像
            if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_liftimage)
            {
                GetComponent<SpriteRenderer>().sprite = p1LiftImage;
            }
            
            //ブロックを降ろした時（元の画像に戻す）
            if(ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_unloadimage)
            {
               GetComponent<SpriteRenderer>().sprite = p1Image;
            }

       
            //アニメーションを再生
            if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().animplay
            && !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_boxopenimage
            && !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_liftimage)
            {
               // Debug.Log("player1あにむ開始");
                anim.SetBool("isMove", true);
            }
            else
            {
               // Debug.Log("player1あにむowari");
                anim.SetBool("isMove", false);
            }

            //ジャンプ中はアニメーション中断
            if(ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().bjump)
            {
                //Debug.Log("jump1");
                anim.SetBool("isMove", false);
            }

        }
        //鍵イラスト
        else if (parentObjectName == "Player2")
        {
            //プレイヤーの移動した方向に応じてプレイヤーの向きを変える
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().imageleft)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            //ブロック持ち上げ画像
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_liftimage)
            {
                GetComponent<SpriteRenderer>().sprite = p2LiftImage;
            }

            //ブロックを降ろした時（元の画像に戻す）
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_unloadimage)
            {
                GetComponent<SpriteRenderer>().sprite = p2Image;
            }

            //アニメーションを再生
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().animplay
            && !ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_liftimage)
            {
                anim.SetBool("isMove", true);
            }
            else
            {
                anim.SetBool("isMove", false);
            }

            //ジャンプ中はアニメーション中断
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().bjump)
            {
               // Debug.Log("jump2");
                anim.SetBool("isMove", false);
            }
        }
    }
}
