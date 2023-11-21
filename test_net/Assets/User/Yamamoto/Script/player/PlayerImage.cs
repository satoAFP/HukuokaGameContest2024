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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //箱イラスト
        if (parentObjectName == "Player1")
        {
            //宝箱オープン画像
            if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_boxopenimage)
            {
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
            else
            {
               // GetComponent<SpriteRenderer>().sprite = p1Image;
            }
        }
        //鍵イラスト
        else if (parentObjectName == "Player2")
        {
            //ブロック持ち上げ画像
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_liftimage)
            {
                GetComponent<SpriteRenderer>().sprite = p2LiftImage;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = p2Image;
            }
        }


    }
}
