using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class OpeningSystem : MonoBehaviourPunCallbacks
{
    private bool first = true;

    [PunRPC]
    private void RpcSceneMove()
    {
        //オーナーの時
        if (PhotonNetwork.IsMasterClient)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        if (first)
        {
            photonView.RPC(nameof(RpcSceneMove), RpcTarget.All);
            first = false;
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

    }
}
