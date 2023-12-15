using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class EndingSystem : MonoBehaviourPunCallbacks
{
    private bool first = true;

       
    //�R���g���[���[B����
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

        if (first)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Title");

            // ���r�[����ޏo����
            PhotonNetwork.Disconnect();
            first = false;
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

    }
}
