using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGimmickActionManagement : CGimmick
{
    //�C���v�b�g�A�N�V������move�擾�p
    private InputAction actionMove;


    //�L�[�i���o�[
    private enum KEY_NUMBER
    {
        A, D, W, S, B, LM, C_B,
        C_L_RIGHT, C_L_LEFT, C_L_UP, C_L_DOWN
    }

    //�{�^�����A���Ŕ������Ȃ��悤
    private bool firstA = true, firstD = true, firstW = true, firstS = true, firstB = true, firstLM = true, firstC_B = true;
    private bool firstC_L_Right = true, firstC_L_Left = true, firstC_L_Up = true, firstC_L_Down = true;

    private void Start()
    {
        //�R���g���[���[��Move�擾
        actionMove = GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Move");
    }

    private void Update()
    {
        //���g�̃A�o�^�[���ǂ���
        if (photonView.IsMine)
        {
            //�ړ��ʎ擾
            Vector2 stickValue = actionMove.ReadValue<Vector2>();

            //���L�������L�[�̐��������₷
            ShareKey(Input.GetKey(KeyCode.A), (int)KEY_NUMBER.A, ref firstA);
            ShareKey(Input.GetKey(KeyCode.D), (int)KEY_NUMBER.D, ref firstD);
            ShareKey(Input.GetKey(KeyCode.W), (int)KEY_NUMBER.W, ref firstW);
            ShareKey(Input.GetKey(KeyCode.S), (int)KEY_NUMBER.S, ref firstS);
            ShareKey(Input.GetKey(KeyCode.B), (int)KEY_NUMBER.B, ref firstB);
            ShareKey(Input.GetMouseButton(0), (int)KEY_NUMBER.LM, ref firstLM);

            //Debug.Log("A:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_RIGHT + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_RIGHT);
            //Debug.Log("D:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_LEFT + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_LEFT);
            //Debug.Log("W:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_UP + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_UP);
            //Debug.Log("S:Owner/" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_DOWN + ":Client/" + ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_DOWN);

            
            //�R���g���[���[���L�ݒ�
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
            if (key == (int)KEY_NUMBER.C_B)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB = onkey;
            if (key == (int)KEY_NUMBER.C_L_RIGHT)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_RIGHT = onkey;
            if (key == (int)KEY_NUMBER.C_L_LEFT)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_LEFT = onkey;
            if (key == (int)KEY_NUMBER.C_L_UP)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_UP = onkey;
            if (key == (int)KEY_NUMBER.C_L_DOWN)
                ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_DOWN = onkey;

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
            if (key == (int)KEY_NUMBER.C_B)
                ManagerAccessor.Instance.dataManager.isClientInputKey_CB = onkey;
            if (key == (int)KEY_NUMBER.C_L_RIGHT)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_RIGHT = onkey;
            if (key == (int)KEY_NUMBER.C_L_LEFT)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_LEFT = onkey;
            if (key == (int)KEY_NUMBER.C_L_UP)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_UP = onkey;
            if (key == (int)KEY_NUMBER.C_L_DOWN)
                ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_DOWN = onkey;
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

            ShareKey(true, (int)KEY_NUMBER.C_B, ref firstC_B);
        }
    }
    public void OnActionRelease(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            // �����ꂽ�u�Ԃ�Performed�ƂȂ�
            if (!context.performed) return;

            ShareKey(false, (int)KEY_NUMBER.C_B, ref firstC_B);
        }
    }
    
}
