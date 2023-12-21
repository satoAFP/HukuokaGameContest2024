using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class GimmickUnlockButton : CGimmick
{
    const int NONE = 999;

    private enum Key
    {
        A, B, X, Y, Right, Left, Up, Down, R1, R2, L1, L2
    }

    [SerializeField, Header("�{�^���ԍ�")] private int ObjNum;

    [SerializeField, Header("�U�����Ă��鎞��")] private int vibrationTime;

    [SerializeField, Header("�U�����̈ړ���")] private Vector3 vibrationPower;

    [SerializeField, Header("����SE")] AudioClip successSE;
    [SerializeField, Header("���sSE")] AudioClip failureSE;

    private AudioSource audioSource;

    private DataManager dataManager = null;

    //�l�Ŏ����Ă���G��Ă��锻��
    private bool islocalUnlockButtonStart = false;

    //���͂����s���������Z�b�g����悤
    private bool isAnswerReset = false;

    //���s���U��
    private bool isVibration = false;
    private int vibrationNum = NONE;
    private int vibrationCount = 0;

    private List<string> HitNames = new List<string>();

    private Rigidbody2D rb2d;


    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public bool isButton = false;

    //���g�̒S���v���C���[��
    [System.NonSerialized] public string managementPlayerName = null;

    //����
    [System.NonSerialized] public List<int> answer = new List<int>();

    //�񓚏�
    public List<bool> ClearSituation;

    private bool first1 = true;
    private bool firstInput = true;
    private bool firstSet = true;

    private void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        dataManager = ManagerAccessor.Instance.dataManager;

        //�N���A����Ɠ������Ȃ�
        if (!transform.parent.GetComponent<GimmickUnlockButtonManagement>().isAllClear)
        {
            //�A�����b�N�{�^���J�n
            if (islocalUnlockButtonStart)
            {
                if (InputCheck())
                {
                    if (firstInput)
                    {
                        firstInput = false;

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
                    }
                }
                else
                {
                    firstInput = true;
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

            if(transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount)
            {
                if (firstSet)
                {
                    if (HitNames.Count != 0)
                    {
                        //���͊J�n�O�ɂP��A�����b�N�{�^���̒S����ݒ�
                        if (HitNames[0] == "Player1")
                            transform.parent.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayerName(0, ObjNum);
                        else if (HitNames[0] == "Player2")
                            transform.parent.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayerName(1, ObjNum);
                        else if (HitNames[0] == "CopyKey")
                            transform.parent.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayerName(2, ObjNum);

                        firstSet = false;
                    }
                }
            }
            //���͎��ԏI�����A�S���̃v���C���[���O�����
            else
            {
                managementPlayerName = "";

                if (!transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 ||
                    !transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) 
                {
                    //�^�C�����~�b�g�Ɖ񓚃f�[�^�`��I��
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
                    ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
                    islocalUnlockButtonStart = false;
                }

                firstSet = true;
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
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

            isAnswerReset = false;
        }

        if(isVibration)
        {
            if (vibrationCount < vibrationTime)
            {
                vibrationCount++;
                if (vibrationCount % 2 == 0)
                {
                    Debug.Log("0");
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[vibrationNum].transform.position += vibrationPower;
                }
                else
                {
                    Debug.Log("1");
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[vibrationNum].transform.position -= vibrationPower;
                }
            }
            else
            {
                isVibration = false;
                vibrationCount = 0;
            }
        }


        //�{�^���ɒN���G��Ă��Ȃ��Ƃ��}�l�[�W���[�ɓ������Ă��Ȃ�����𑗂�
        if (HitNames.Count == 0) 
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
            HitNames.Add(collision.gameObject.name);

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
            //���͊J�n���Ⴄ�L���������͂��Ȃ����߂̏���
            if (collision.gameObject.name == managementPlayerName || managementPlayerName == "") 
            {
                if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
                {
                    //���{�^���Ƀv���C���[������Ƃ�
                    if ((transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 &&
                        transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) ||
                        transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount)
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
                    if ((transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 &&
                        transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) ||
                        transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount)
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
        for (int i = 0; i < HitNames.Count; i++)
        {
            if (HitNames[i] == collision.gameObject.name)
            {
                HitNames.RemoveAt(i);
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
                    CheckInputButton(dataManager.isOwnerInputKey_CA, i);
                    break;
                case (int)Key.B:
                    CheckInputButton(dataManager.isOwnerInputKey_CB, i);
                    break;
                case (int)Key.X:
                    CheckInputButton(dataManager.isOwnerInputKey_CX, i);
                    break;
                case (int)Key.Y:
                    CheckInputButton(dataManager.isOwnerInputKey_CY, i);
                    break;
                case (int)Key.Right:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_RIGHT, i);
                    break;
                case (int)Key.Left:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_LEFT, i);
                    break;
                case (int)Key.Up:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_UP, i);
                    break;
                case (int)Key.Down:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_DOWN, i);
                    break;
                case (int)Key.R1:
                    CheckInputButton(dataManager.isOwnerInputKey_C_R1, i);
                    break;
                case (int)Key.R2:
                    CheckInputButton(dataManager.isOwnerInputKey_C_R2, i);
                    break;
                case (int)Key.L1:
                    CheckInputButton(dataManager.isOwnerInputKey_C_L1, i);
                    break;
                case (int)Key.L2:
                    CheckInputButton(dataManager.isOwnerInputKey_C_L2, i);
                    break;
            }
        }
        else
        {
            switch (answer[i])
            {
                case (int)Key.A:
                    CheckInputButton(dataManager.isClientInputKey_CA, i);
                    break;
                case (int)Key.B:
                    CheckInputButton(dataManager.isClientInputKey_CB, i);
                    break;
                case (int)Key.X:
                    CheckInputButton(dataManager.isClientInputKey_CX, i);
                    break;
                case (int)Key.Y:
                    CheckInputButton(dataManager.isClientInputKey_CY, i);
                    break;
                case (int)Key.Right:
                    CheckInputButton(dataManager.isClientInputKey_C_D_RIGHT, i);
                    break;
                case (int)Key.Left:
                    CheckInputButton(dataManager.isClientInputKey_C_D_LEFT, i);
                    break;
                case (int)Key.Up:
                    CheckInputButton(dataManager.isClientInputKey_C_D_UP, i);
                    break;
                case (int)Key.Down:
                    CheckInputButton(dataManager.isClientInputKey_C_D_DOWN, i);
                    break;
                case (int)Key.R1:
                    CheckInputButton(dataManager.isClientInputKey_C_R1, i);
                    break;
                case (int)Key.R2:
                    CheckInputButton(dataManager.isClientInputKey_C_R2, i);
                    break;
                case (int)Key.L1:
                    CheckInputButton(dataManager.isClientInputKey_C_L1, i);
                    break;
                case (int)Key.L2:
                    CheckInputButton(dataManager.isClientInputKey_C_L2, i);
                    break;
            }
        }
    }

    //�񓚏󋵍X�V
    private void CheckInputButton(bool inputkey,int ansnum)
    {
        //����
        if (inputkey)
        {
            ClearSituation[ansnum] = true;
            transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[ansnum].GetComponent<Image>().color = new Color32(128, 128, 128, 255);

            //SE�Đ�
            audioSource.PlayOneShot(successSE);
        }
        //���s
        else
        {
            //�����̃��Z�b�g
            isAnswerReset = true;

            //�o�C�u���[�V�����J�n
            isVibration = true;
            vibrationNum = ansnum;

            //�~�X���J�E���g
            if (PhotonNetwork.IsMasterClient)
                ManagerAccessor.Instance.dataManager.ownerMissCount++;
            else
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareInputMiss();

            //SE�Đ�
            audioSource.PlayOneShot(failureSE);
        }
    }

    //�R���g���[���[�����͂���Ă��邩�ǂ���
    private bool InputCheck()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (dataManager.isOwnerInputKey_CA || dataManager.isOwnerInputKey_CB || dataManager.isOwnerInputKey_CX || dataManager.isOwnerInputKey_CY ||
                dataManager.isOwnerInputKey_C_D_RIGHT || dataManager.isOwnerInputKey_C_D_LEFT || dataManager.isOwnerInputKey_C_D_UP || dataManager.isOwnerInputKey_C_D_DOWN ||
                dataManager.isOwnerInputKey_C_R1 || dataManager.isOwnerInputKey_C_R2 || dataManager.isOwnerInputKey_C_L1 || dataManager.isOwnerInputKey_C_L2) 
            {
                return true;
            }
        }
        else
        {
            if (dataManager.isClientInputKey_CA || dataManager.isClientInputKey_CB || dataManager.isClientInputKey_CX || dataManager.isClientInputKey_CY ||
                dataManager.isClientInputKey_C_D_RIGHT || dataManager.isClientInputKey_C_D_LEFT || dataManager.isClientInputKey_C_D_UP || dataManager.isClientInputKey_C_D_DOWN ||
                dataManager.isClientInputKey_C_R1 || dataManager.isClientInputKey_C_R2 || dataManager.isClientInputKey_C_L1 || dataManager.isClientInputKey_C_L2)
            {
                return true;
            }
        }

        return false;
    }

}
