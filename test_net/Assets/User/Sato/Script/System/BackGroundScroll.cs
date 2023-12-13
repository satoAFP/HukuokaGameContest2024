using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackGroundScroll : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("背景オブジェクト")] private GameObject BackGround;

    [SerializeField, Header("画像サイズ")] private Vector2 Size;

    private GameObject[] imgObj = new GameObject[9];    //背景画像格納用
    private Vector3 player = Vector3.zero;              //その画面での操作キャラ取得用

    [SerializeField] private Vector2 mapPos = Vector2.zero;//背景画像単位での主人公の座標

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //プレイヤーが存在しているとき
        if (ManagerAccessor.Instance.dataManager.player1 != null &&
            ManagerAccessor.Instance.dataManager.player2 != null)
        {
            //Locketに乗っていない時
            if (!ManagerAccessor.Instance.dataManager.isFlyStart)
            {
                //その画面での操作キャラ取得
                if (PhotonNetwork.IsMasterClient)
                {
                    player = ManagerAccessor.Instance.dataManager.player1.transform.position;
                }
                else
                {
                    player = ManagerAccessor.Instance.dataManager.player2.transform.position;
                }
            }
            else
            {
                player = ManagerAccessor.Instance.dataManager.flyPos;
            }

            if (first)
            {
                //背景生成
                for (int i = 0; i < 9; i++)
                    imgObj[i] = Instantiate(BackGround);

                //背景の初期座標設定
                for (int i = 0; i < 9; i += 3)
                {
                    imgObj[i].transform.position = new Vector3(-9, Size.y / 2 - (Size.y * i / 3), 0);
                    imgObj[i + 1].transform.position = new Vector3(9, Size.y / 2 - (Size.y * i / 3), 0);
                    imgObj[i + 2].transform.position = new Vector3(27, Size.y / 2 - (Size.y * i / 3), 0);
                }

                first = false;
            }

            //背景画像単位での主人公の座標設定
            if (player.x >= 0)
            {
                mapPos.x = Mathf.Floor(player.x / Size.x);
            }
            else
            {
                if (mapPos.x == 0)
                    mapPos.x = -1;

                mapPos.x = Mathf.Ceil(player.x / Size.x) - 1;
            }

            if (player.y >= 0)
            {
                mapPos.y = Mathf.Floor(player.y / Size.y);
            }
            else
            {
                if (mapPos.y == 0)
                    mapPos.y = -1;

                mapPos.y = Mathf.Ceil(player.y / Size.y) - 1;
            }

            //操作キャラの座標によって背景画像の座標更新
            for (int i = 0; i < 9; i += 3)
            {
                imgObj[i].transform.position = new Vector3(-(Size.x / 2) + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
                imgObj[i + 1].transform.position = new Vector3((Size.x / 2) + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
                imgObj[i + 2].transform.position = new Vector3((Size.x / 2) + Size.x + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
            }
        }
    }
}
