using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGimmickActionManagement : CGimmick
{
    //インプットアクションのmove取得用
    private InputAction actionMove;


    //ボタンが連続で反応しないよう
    private bool firstA = true, firstD = true, firstW = true, firstS = true, firstB = true;
    private bool firstLM = true;
    private bool firstC_A = true, firstC_B = true, firstC_X = true, firstC_Y = true;
    private bool firstC_L_Right = true, firstC_L_Left = true, firstC_L_Up = true, firstC_L_Down = true;
    private bool firstC_D_Right = true, firstC_D_Left = true, firstC_D_Up = true, firstC_D_Down = true;

    private void Start()
    {
        //コントローラーのMove取得
        actionMove = GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Move");
    }

    private void Update()
    {
        //自身のアバターかどうか
        if (photonView.IsMine)
        {
            //移動量取得
            Vector2 stickValue = actionMove.ReadValue<Vector2>();

            //共有したいキーの数だけ増やす
            ShareKey(Input.GetKey(KeyCode.A), (int)KEY_NUMBER.A, ref firstA);
            ShareKey(Input.GetKey(KeyCode.D), (int)KEY_NUMBER.D, ref firstD);
            ShareKey(Input.GetKey(KeyCode.W), (int)KEY_NUMBER.W, ref firstW);
            ShareKey(Input.GetKey(KeyCode.S), (int)KEY_NUMBER.S, ref firstS);
            ShareKey(Input.GetKey(KeyCode.B), (int)KEY_NUMBER.B, ref firstB);
            ShareKey(Input.GetMouseButton(0), (int)KEY_NUMBER.LM, ref firstLM);

            //Debug.Log("A:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_LEFT + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_LEFT);
            //Debug.Log("D:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_RIGHT + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_RIGHT);
            //Debug.Log("W:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP);
           // Debug.Log("S:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_DOWN + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_DOWN);


            //コントローラー共有設定
            if (stickValue.x > 0.1f)
                ShareKey(true, (int)KEY_NUMBER.C_L_RIGHT, ref firstC_L_Right);
            else
                ShareKey(false, (int)KEY_NUMBER.C_L_RIGHT, ref firstC_L_Right);
            if (stickValue.x < -0.1f)
                ShareKey(true, (int)KEY_NUMBER.C_L_LEFT, ref firstC_L_Left);
            else
                ShareKey(false, (int)KEY_NUMBER.C_L_LEFT, ref firstC_L_Left);
            if (stickValue.y > 0.1f)
                ShareKey(true, (int)KEY_NUMBER.C_L_UP, ref firstC_L_Up);
            else
                ShareKey(false, (int)KEY_NUMBER.C_L_UP, ref firstC_L_Up);
            if (stickValue.y < -0.1f)
                ShareKey(true, (int)KEY_NUMBER.C_L_DOWN, ref firstC_L_Down);
            else
                ShareKey(false, (int)KEY_NUMBER.C_L_DOWN, ref firstC_L_Down);
        }
    }


    //共有するキーのデータ送信
    [PunRPC]
    private void RpcShareKey(string name, int key, bool onkey)
    {
        if (name == "Player1")
        {
            if (key == (int)KEY_NUMBER.A)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_A = onkey;
            if (key == (int)KEY_NUMBER.D)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_D = onkey;
            if (key == (int)KEY_NUMBER.W)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_W = onkey;
            if (key == (int)KEY_NUMBER.S)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_S = onkey;
            if (key == (int)KEY_NUMBER.B)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_B = onkey;
            if (key == (int)KEY_NUMBER.LM)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_LM = onkey;
            if (key == (int)KEY_NUMBER.C_A)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_CA = onkey;
            if (key == (int)KEY_NUMBER.C_B)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB = onkey;
            if (key == (int)KEY_NUMBER.C_X)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_CX = onkey;
            if (key == (int)KEY_NUMBER.C_Y)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_CY = onkey;
            if (key == (int)KEY_NUMBER.C_L_RIGHT)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_RIGHT = onkey;
            if (key == (int)KEY_NUMBER.C_L_LEFT)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_LEFT = onkey;
            if (key == (int)KEY_NUMBER.C_L_UP)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_UP = onkey;
            if (key == (int)KEY_NUMBER.C_L_DOWN)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_DOWN = onkey;
            if (key == (int)KEY_NUMBER.C_D_RIGHT)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_RIGHT = onkey;
            if (key == (int)KEY_NUMBER.C_D_LEFT)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_LEFT = onkey;
            if (key == (int)KEY_NUMBER.C_D_UP)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP = onkey;
            if (key == (int)KEY_NUMBER.C_D_DOWN)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_DOWN = onkey;

        }
        else if (name == "Player2")
        {
            if (key == (int)KEY_NUMBER.A)
                ManagerAccessor.Instance.dataManager.isClientInputKey_A = onkey;
            if (key == (int)KEY_NUMBER.D)
                ManagerAccessor.Instance.dataManager.isClientInputKey_D = onkey;
            if (key == (int)KEY_NUMBER.W)
                ManagerAccessor.Instance.dataManager.isClientInputKey_W = onkey;
            if (key == (int)KEY_NUMBER.S)
                ManagerAccessor.Instance.dataManager.isClientInputKey_S = onkey;
            if (key == (int)KEY_NUMBER.B)
                ManagerAccessor.Instance.dataManager.isClientInputKey_B = onkey;
            if (key == (int)KEY_NUMBER.LM)
                ManagerAccessor.Instance.dataManager.isClientInputKey_LM = onkey;
            if(key == (int)KEY_NUMBER.C_A)
                ManagerAccessor.Instance.dataManager.isClientInputKey_CA = onkey;
            if (key == (int)KEY_NUMBER.C_B)
                ManagerAccessor.Instance.dataManager.isClientInputKey_CB = onkey;
            if (key == (int)KEY_NUMBER.C_X)
                ManagerAccessor.Instance.dataManager.isClientInputKey_CX = onkey;
            if (key == (int)KEY_NUMBER.C_Y)
                ManagerAccessor.Instance.dataManager.isClientInputKey_CY = onkey;
            if (key == (int)KEY_NUMBER.C_L_RIGHT)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_RIGHT = onkey;
            if (key == (int)KEY_NUMBER.C_L_LEFT)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_LEFT = onkey;
            if (key == (int)KEY_NUMBER.C_L_UP)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_UP = onkey;
            if (key == (int)KEY_NUMBER.C_L_DOWN)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_DOWN = onkey;
            if (key == (int)KEY_NUMBER.C_D_RIGHT)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_RIGHT = onkey;
            if (key == (int)KEY_NUMBER.C_D_LEFT)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_LEFT = onkey;
            if (key == (int)KEY_NUMBER.C_D_UP)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP = onkey;
            if (key == (int)KEY_NUMBER.C_D_DOWN)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_DOWN = onkey;
        }
    }



    /// <summary>
    /// キーを入力しているかどうかのデータを送る関数
    /// </summary>
    /// <param name="inputKey">入力されたキー</param>
    /// <param name="Key">何のキーを押したか</param>
    /// <param name="first">長押しが反応しないため</param>
    private void ShareKey(bool inputKey,int Key,ref bool first)
    {
        //入力されたとき
        if (inputKey)
        {
            if (first)
            {
                //押された情報を送る
                photonView.RPC(nameof(RpcShareKey), RpcTarget.All, gameObject.name, Key, true);
                first = false;
            }
        }
        else
        {
            if (!first)
            {
                //離した情報を送る
                photonView.RPC(nameof(RpcShareKey), RpcTarget.All, gameObject.name, Key, false);
                first = true;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //コントローラー入力共有

    //コントローラーA入力
    public void OnActionPressA(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_A, ref firstC_A);
        }
    }
    public void OnActionReleaseA(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_A, ref firstC_A);
        }
    }

    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_B, ref firstC_B);
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_B, ref firstC_B);
        }
    }

    //コントローラーX入力
    public void OnActionPressX(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_X, ref firstC_X);
        }
    }
    public void OnActionReleaseX(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_X, ref firstC_X);
        }
    }

    //コントローラーY入力
    public void OnActionPressY(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_Y, ref firstC_Y);
        }
    }
    public void OnActionReleaseY(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_Y, ref firstC_Y);
        }
    }

    //コントローラー十字キー上（箱を開ける）
    public void OnOpenActionPress(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_D_UP, ref firstC_D_Up);
        }
    }
    public void OnOpenActionRelease(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_D_UP, ref firstC_D_Up);
        }
    }

    //コントローラー十字キー右
    public void OnR_D_PadPress(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_D_RIGHT, ref firstC_D_Right);
        }
    }
    public void OnR_D_PadRelease(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_D_RIGHT, ref firstC_D_Right);
        }
    }

    //コントローラー十字キー左
    public void OnL_D_PadPress(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 押された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.C_D_LEFT, ref firstC_D_Left);
        }
    }
    public void OnL_D_PadRelease(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // 離された瞬間でPerformedとなる
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_D_LEFT, ref firstC_D_Left);
        }
    }

}
