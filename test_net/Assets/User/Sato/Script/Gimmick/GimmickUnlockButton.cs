using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GimmickUnlockButton : CGimmick
{
    private enum Key
    {
        A, B, X, Y, Right, Left, Up, Down, R1, R2, L1, L2
    }

    //�l�Ŏ����Ă���G��Ă��锻��
    private bool islocalUnlockButtonStart = false;

    //�ǂ���̃v���C���[���G��Ă��邩
    private bool isHitPlayer1 = false;
    private bool isHitPlayer2 = false;

    private Rigidbody2D rb2d;


    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public bool isButton = false;

    //����
    [System.NonSerialized] public List<int> answer = new List<int>();

    //�񓚏�
    public List<bool> ClearSituation;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        //�A�����b�N�{�^���J�n
        if (islocalUnlockButtonStart)
        {
            //�����̐�
            for (int i = 0; i < answer.Count; i++)
            {
                //�N���A���Ă�����͔͂�΂����
                if (ClearSituation[i])
                {
                    continue;
                }
                else
                {
                    //���͏��ƈ�v���邩�`�F�b�N
                    InputAnswer(i);
                    break;
                }
            }

            //�ŏ��̓��͂������̎��A�J�E���g�J�n
            if (ClearSituation[0])
            {
                transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount = true;
            }

            //�Ō�̓��͂��I������Ƃ��N���A���𑗂�
            if (ClearSituation[ClearSituation.Count - 1])
            {
                //�}�X�^�[���ǂ���
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().isOwnerClear = true;
                }
                else
                {
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().isClientClear = true;

                }
            }



        }



        rb2d.WakeUp();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //�v���C���[2���G��Ă��Ȃ��Ƃ�
            if (!isHitPlayer2)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(true);
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(true);
                    //�I�u�W�F�N�g�G��Ă�����
                    ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                    islocalUnlockButtonStart = true;
                }

                
                isHitPlayer1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            //�v���C���[1���G��Ă��Ȃ��Ƃ�
            if (!isHitPlayer1)
            {
                if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(true);
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(true);
                    //�I�u�W�F�N�g�G��Ă�����
                    ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                    islocalUnlockButtonStart = true;
                }

                
                isHitPlayer2 = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��I��
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            isHitPlayer1 = false;
        }
        if (collision.gameObject.name == "Player2")
        {
            //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��I��
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            isHitPlayer2 = false;
        }

    }

    /// <summary>
    /// �񓚓��͎擾�p�֐�
    /// </summary>
    /// <param name="i">���X�g���̉��Ԗڂ��񓚒���</param>
    private void InputAnswer(int i)
    {
        //�}�X�^�[���ǂ���
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //���ꂼ��̓��͂Ƃ����Ă��邩�ǂ���
            switch (answer[i])
            {
                case (int)Key.A:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CA)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.B:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.X:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CX)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Y:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CY)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Right:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_RIGHT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Left:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_LEFT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Up:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Down:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_DOWN)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R1:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_R1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R2:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_R2)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L1:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L2:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L2)
                        ClearSituation[i] = true;
                    break;
            }
        }
        else
        {
            switch (answer[i])
            {
                case (int)Key.A:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_CA)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.B:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_CB)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.X:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_CX)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Y:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_CY)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Right:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_RIGHT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Left:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_LEFT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Up:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Down:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_DOWN)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R1:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_R1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R2:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_R2)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L1:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_L1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L2:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_L2)
                        ClearSituation[i] = true;
                    break;
            }
        }
    }

}
