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

    [SerializeField, Header("�{�^���ԍ�")] private int ObjNum;

    //�l�Ŏ����Ă���G��Ă��锻��
    private bool islocalUnlockButtonStart = false;

    //���͂����s���������Z�b�g����悤
    private bool isAnswerReset = false;

    private List<string> HitTags = new List<string>();

    private Rigidbody2D rb2d;


    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public bool isButton = false;

    //����
    [System.NonSerialized] public List<int> answer = new List<int>();

    //�񓚏�
    public List<bool> ClearSituation;

    private bool first1 = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        //�N���A����Ɠ������Ȃ�
        if (!transform.parent.GetComponent<GimmickUnlockButtonManagement>().isAllClear)
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
            //OnCollisionStay2D����ɓ���������
            rb2d.WakeUp();
        }

        //���͎��s���A���͏�񃊃Z�b�g
        if(isAnswerReset)
        {
            for (int i = 0; i < ClearSituation.Count; i++) 
            {
                ClearSituation[i] = false;
            }
            isAnswerReset = false;
        }


        //�{�^���ɒN���G��Ă��Ȃ��Ƃ��}�l�[�W���[�ɓ������Ă��Ȃ�����𑗂�
        if (HitTags.Count == 0) 
        {
            if (first1)
            {
                if (ObjNum == 0)
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(true, false);
                else if (ObjNum == 1)
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(false, false);
                first1 = false;
            }
        }
        else
        {
            first1 = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�G�ꂽ�I�u�W�F�N�g�̖��O�L��
            HitTags.Add(collision.gameObject.name);

            if (ObjNum == 0)
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(true, true);
            else if (ObjNum == 1)
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(false, true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //�N���A����Ɠ������Ȃ�
        if (!transform.parent.GetComponent<GimmickUnlockButtonManagement>().isAllClear)
        {
            if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
            {
                //�v���C���[2���G��Ă��Ȃ��Ƃ�
                if (transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 &&
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) 
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
                }
            }
            if (collision.gameObject.name == "Player2")
            {
                //�v���C���[1���G��Ă��Ȃ��Ƃ�
                if (transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 &&
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2)
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
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��I��
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayer(true, false);
        }
        if (collision.gameObject.name == "Player2")
        {
            //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��I��
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayer(false, false);
        }


        //�o���I�u�W�F�N�g�̃^�O������
        for (int i = 0; i < HitTags.Count; i++)
        {
            if (HitTags[i] == collision.gameObject.name)
            {
                HitTags.RemoveAt(i);
            }
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
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_CA, i);
                    break;
                case (int)Key.B:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB, i);
                    break;
                case (int)Key.X:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_CX, i);
                    break;
                case (int)Key.Y:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_CY, i);
                    break;
                case (int)Key.Right:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_RIGHT, i);
                    break;
                case (int)Key.Left:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_LEFT, i);
                    break;
                case (int)Key.Up:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP, i);
                    break;
                case (int)Key.Down:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_DOWN, i);
                    break;
                case (int)Key.R1:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_R1, i);
                    break;
                case (int)Key.R2:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_R2, i);
                    break;
                case (int)Key.L1:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L1, i);
                    break;
                case (int)Key.L2:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L2, i);
                    break;
            }
        }
        else
        {
            switch (answer[i])
            {
                case (int)Key.A:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_CA, i);
                    break;
                case (int)Key.B:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_CB, i);
                    break;
                case (int)Key.X:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_CX, i);
                    break;
                case (int)Key.Y:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_CY, i);
                    break;
                case (int)Key.Right:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_RIGHT, i);
                    break;
                case (int)Key.Left:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_LEFT, i);
                    break;
                case (int)Key.Up:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP, i);
                    break;
                case (int)Key.Down:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_DOWN, i);
                    break;
                case (int)Key.R1:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_R1, i);
                    break;
                case (int)Key.R2:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_R2, i);
                    break;
                case (int)Key.L1:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_L1, i);
                    break;
                case (int)Key.L2:
                    CheckInputButton(ManagerAccessor.Instance.dataManager.isClientInputKey_C_L2, i);
                    break;
            }
        }
    }

    //�񓚏󋵍X�V
    private void CheckInputButton(bool inputkey,int ansnum)
    {
        if (inputkey)
            ClearSituation[ansnum] = true;
        else
            isAnswerReset = true;
    }

}
