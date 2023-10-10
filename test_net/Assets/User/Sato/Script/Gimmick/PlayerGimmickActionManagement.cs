using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayerGimmickActionManagement : CGimmick
{
    private enum KEY_NUMBER
    {
        A, D, W, S
    }

    //ボタンが連続で反応しないよう
    private bool first1 = true, first2 = true, first3 = true, first4 = true;

    private void Update()
    {
        //自身のアバターかどうか
        if (photonView.IsMine)
        {
            ShareKey(Input.GetKey(KeyCode.A), (int)KEY_NUMBER.A, ref first1);
            ShareKey(Input.GetKey(KeyCode.D), (int)KEY_NUMBER.D, ref first2);
            ShareKey(Input.GetKey(KeyCode.W), (int)KEY_NUMBER.W, ref first3);
            ShareKey(Input.GetKey(KeyCode.S), (int)KEY_NUMBER.S, ref first4);

            Debug.Log("A:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_A + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_A);
            Debug.Log("D:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_D + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_D);
            Debug.Log("W:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_W + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_W);
            Debug.Log("S:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_S + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_S);
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
                Debug.Log("aaa");
                first = false;
            }
        }
        else
        {
            if (!first)
            {
                //離した情報を送る
                photonView.RPC(nameof(RpcShareKey), RpcTarget.All, gameObject.name, Key, false);
                Debug.Log("bbb");
                first = true;
            }
        }
    }


    [PunRPC]
    private void RpcShareKey(string name,int key,bool onkey)
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
        }
    }
}
