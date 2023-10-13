using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGimmickActionManagement : CGimmick
{
    private enum KEY_NUMBER
    {
        A, D, W, S, B, LM, CB
    }

    //�{�^�����A���Ŕ������Ȃ��悤
    private bool firstA = true, firstD = true, firstW = true, firstS = true, firstB = true, firstLM = true, firstCB = true;

    private void Update()
    {
        //���g�̃A�o�^�[���ǂ���
        if (photonView.IsMine)
        {
            //���L�������L�[�̐��������₷
            ShareKey(Input.GetKey(KeyCode.A), (int)KEY_NUMBER.A, ref firstA);
            ShareKey(Input.GetKey(KeyCode.D), (int)KEY_NUMBER.D, ref firstD);
            ShareKey(Input.GetKey(KeyCode.W), (int)KEY_NUMBER.W, ref firstW);
            ShareKey(Input.GetKey(KeyCode.S), (int)KEY_NUMBER.S, ref firstS);
            ShareKey(Input.GetKey(KeyCode.B), (int)KEY_NUMBER.B, ref firstB);
            ShareKey(Input.GetMouseButton(0), (int)KEY_NUMBER.LM, ref firstLM);

            //Debug.Log("A:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_A + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_A);
            //Debug.Log("D:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_D + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_D);
            //Debug.Log("W:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_W + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_W);
            //Debug.Log("S:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_S + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_S);
        }
    }


    //���L����L�[�̃f�[�^���M
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
            if (key == (int)KEY_NUMBER.CB)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB = onkey;

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
            if (key == (int)KEY_NUMBER.CB)
                ManagerAccessor.Instance.dataManager.isClientInputKey_CB = onkey;
        }
    }



    /// <summary>
    /// �L�[����͂��Ă��邩�ǂ����̃f�[�^�𑗂�֐�
    /// </summary>
    /// <param name="inputKey">���͂��ꂽ�L�[</param>
    /// <param name="Key">���̃L�[����������</param>
    /// <param name="first">���������������Ȃ�����</param>
    private void ShareKey(bool inputKey,int Key,ref bool first)
    {
        //���͂��ꂽ�Ƃ�
        if (inputKey)
        {
            if (first)
            {
                //�����ꂽ���𑗂�
                photonView.RPC(nameof(RpcShareKey), RpcTarget.All, gameObject.name, Key, true);
                first = false;
            }
        }
        else
        {
            if (!first)
            {
                //���������𑗂�
                photonView.RPC(nameof(RpcShareKey), RpcTarget.All, gameObject.name, Key, false);
                first = true;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //�R���g���[���[���͋��L

    //�R���g���[���[B����
    public void OnActionPress(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // �����ꂽ�u�Ԃ�Performed�ƂȂ�
            if (!context.performed) return;

            ShareKey(true, (int)KEY_NUMBER.CB, ref firstCB);
        }
    }
    public void OnActionRelease(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // �����ꂽ�u�Ԃ�Performed�ƂȂ�
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.CB, ref firstCB);
        }
    }


    //�R���g���[���[�X�e�B�b�N����
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();

        if (inputDirection.x > 0)
            Debug.Log("�E");
        if (inputDirection.x < 0)
            Debug.Log("��");
        if (inputDirection.y > 0)
            Debug.Log("��");
        if (inputDirection.y < 0)
            Debug.Log("��");
        if (inputDirection.x == 0 || inputDirection.y == 0) 
            Debug.Log("�X�g�b�v");

    }

}
