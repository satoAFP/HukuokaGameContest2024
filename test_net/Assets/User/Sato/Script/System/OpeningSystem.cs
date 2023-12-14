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
        //�I�[�i�[�̎�
        if (PhotonNetwork.IsMasterClient)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    //�R���g���[���[B����
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

        if (first)
        {
            photonView.RPC(nameof(RpcSceneMove), RpcTarget.All);
            first = false;
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

    }
}
