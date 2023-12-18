using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class EndingSystem : MonoBehaviourPunCallbacks
{
    private bool first = true;

       
    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        if (first)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Title");

            // ロビーから退出する
            PhotonNetwork.Disconnect();
            first = false;
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

    }
}
