using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickButton : CGimmick
{
    //ÇªÇÍÇºÇÍÇÃÉ{É^Éìì¸óÕèÛãµ
    [System.NonSerialized] public bool isButton = false;

    private bool firstPushP1 = true;
    private bool firstPushP2 = true;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    if (firstPushP1)
                    {
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true);
                        firstPushP1 = false;
                    }
                }
                else
                {
                    if (!firstPushP1)
                    {
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                        firstPushP1 = true;
                    }
                }
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    if (firstPushP2)
                    {
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true);
                        firstPushP2 = false;
                    }
                }
                else
                {
                    if (!firstPushP2)
                    {
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                        firstPushP2 = true;
                    }
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                firstPushP1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                firstPushP2 = true;
            }
        }
    }


    [PunRPC]
    protected void RpcButtonCheck(bool onButton)
    {
        isButton = onButton;
    }
}
