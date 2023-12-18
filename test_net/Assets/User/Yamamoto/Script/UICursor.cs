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

    private int LRmove = 0;//1:右　2:左

    private bool movestart = false;//移動中かを判断する

    //カーソルの色を決める
   // [SerializeField] private Color color_black;
    //[SerializeField] private Color color_red;

    private int ColorChangeframe = 0;//色を変更させる時間を計る

    private int Change_Color = 1;//数字によって色を変更させる

    // Start is called before the first frame update
    void Start()
    {
        test_net = new Test_net();//スクリプトを変数に格納

        GetComponent<Image>().color = Color.black;//初期カーソルカラー
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;


        if (ManagerAccessor.Instance.dataManager.player1 != null)
        {
            //箱を開けている時カーソル移動をする
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
            {
                ColorChangeframe++;

                Debug.Log("ColorChangeframe" + ColorChangeframe);

                //約一秒程度でカーソルの色を変える
                if(ColorChangeframe >= 60 )
                {
                    CursorColorChange();
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



    private void CursorColorChange()
    {
        if (Change_Color == 1)
        {
            Debug.Log("赤");
            GetComponent<Image>().color = Color.red;//一定時間でカーソルを赤にする
            ColorChangeframe = 0;
            Change_Color = 2;
        }
        else if (Change_Color == 2)
        {
            Debug.Log("黒");
            GetComponent<Image>().color = Color.black;//一定時間でカーソルを黒にする
            ColorChangeframe = 0;
            Change_Color = 1;
        }
    }
}
