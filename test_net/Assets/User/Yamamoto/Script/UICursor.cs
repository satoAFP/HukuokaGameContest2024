using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Photon.Pun;

public class UICursor : MonoBehaviourPunCallbacks
{
    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("板のアイコン")]
    private GameObject BoardIcon;

    [SerializeField, Header("コピーキーのアイコン")]
    private GameObject CopyKeyIcon;

    [SerializeField, Header("色を変えたいオブジェクト")]
    private GameObject[] ColorChangeObjects;

    private int LRmove = 0;//1:右　2:左

    private bool movestart = false;//移動中かを判断する

    private int ColorChangeframe = 0;//色を変更させる時間を計る

    private int Change_Color = 1;//数字によって色を変更させる

    [SerializeField, Header("点滅の間隔")] private int blinkingtime;

   
    //カーソルの色を設定できる
    [SerializeField, Header("カーソルカラー1")] private Color Type1;
    [SerializeField, Header("カーソルカラー2")] private Color Type2;

    // Start is called before the first frame update
    void Start()
    {
        test_net = new Test_net();//スクリプトを変数に格納

        GetComponent<Image>().color = Type1;//初期カーソルカラー
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        if (ManagerAccessor.Instance.dataManager.player1 != null)
        {
            if(datamanager.player1.GetComponent<PlayerController>().movelock)
            {
                ColorChangeframe++;

                //約一秒程度でカーソルの色を変える
                if (ColorChangeframe >= blinkingtime)
                {
                    CursorColorChange();
                }
            }
            else
            {
                ColorChangeframe = 0;//カーソルの色変化フレーム計算リセット
                GetComponent<Image>().color = Type1;//カーソルの色を黒に変える
            }

            //箱を開けている時カーソル移動をする
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
            {
                //各プレイヤーのアイコンを元の色に戻す
                for (int i = 0; i < ColorChangeObjects.Length; i++)
                {
                    ColorChangeObjects[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }

                //押されたボタンの左右でカーソルの移動位置を決める
                if (datamanager.isOwnerInputKey_C_D_RIGHT && !movestart)
                {
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "None";
                    movestart = true;
                    LRmove = 1;//右にカーソル移動
                }

                if (datamanager.isOwnerInputKey_C_D_LEFT && !movestart)
                {
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "None";
                    movestart = true;
                    LRmove = 2;//左にカーソル移動
                }
            }
            else
            {
                //各プレイヤーのアイコンを黒いカラーに変更
                for (int i = 0; i < ColorChangeObjects.Length; i++)
                {
                    ColorChangeObjects[i].GetComponent<Image>().color = new Color32(0, 0, 0, 192);
                }
                   
            }

        }

        if(movestart)
        {
            if(LRmove==1)
            {
                transform.position = Vector2.MoveTowards(transform.position, CopyKeyIcon.transform.position, moveSpeed * Time.deltaTime);
                if (transform.position == CopyKeyIcon.transform.position)
                {
                    movestart = false;
                    LRmove = 0;//移動終了
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "CopyKey";//現在カーソルが選択しているアイテム名
                }
            }
            else if(LRmove==2)
            {
                transform.position = Vector2.MoveTowards(transform.position, BoardIcon.transform.position, moveSpeed * Time.deltaTime);
                if (transform.position == BoardIcon.transform.position)
                {
                    movestart = false;
                    LRmove = 0;//移動終了
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "Board";//現在カーソルが選択しているアイテム名
                }
            }
            
        }
    }


    //ここでカーソルの色を変える
    private void CursorColorChange()
    {
        if (Change_Color == 1)
        {
            //Debug.Log("赤");
            GetComponent<Image>().color = Type2;//一定時間でカーソルを赤にする
            ColorChangeframe = 0;//フレーム計算リセット
            Change_Color = 2;
        }
        else if (Change_Color == 2)
        {
            //Debug.Log("黒");
            GetComponent<Image>().color = Type1;//一定時間でカーソルを黒にする
            ColorChangeframe = 0;//フレーム計算リセット
            Change_Color = 1;
        }
    }


}
