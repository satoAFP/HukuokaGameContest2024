using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GimmickUnlockButtonManagement : CGimmick
{
    [SerializeField, Header("�ǂ̃M�~�b�N�ɂ��邩")]
    [Header("0:�I�u�W�F�N�g���� / 1:�I�u�W�F�N�g�o��")]
    private int gimmickNum;

    [SerializeField, Header("�M�~�b�N�p�{�^��")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("��")]
    private GameObject door;

    [SerializeField, Header("�񓚉摜���i�[����I�u�W�F�N�g")]
    public GameObject answerArea;

    [SerializeField, Header("�񓚉摜���i�[����I�u�W�F�N�g")]
    public GameObject timeLimitSlider;

    [SerializeField, Header("�񓚉摜�𕡐�����I�u�W�F�N�g")]
    private GameObject initAnswer;

    [SerializeField, Header("���͂��鐔")]
    private int inputKey;

    [SerializeField, Header("�c�莞��")]
    private Text timetext;

    [SerializeField, Header("���͎��̐�������")]
    private int timeLimit;

    [SerializeField, Header("�������B�����ǂ���")]
    private bool isHideAnswer;

    [SerializeField, Header("�B���ꍇ�ǂ����B����(�`�F�b�N������Player1�̓������B�����)")]
    private List<bool> isWhareHideAnswer;

    //�ǂ���̃v���C���[���G��Ă��邩
    [System.NonSerialized] public bool isHitPlayer1 = false;
    [System.NonSerialized] public bool isHitPlayer2 = false;

    [System.NonSerialized] public bool isHitUnlockButton1 = false;
    [System.NonSerialized] public bool isHitUnlockButton2 = false;

    //���͊J�n���
    [System.NonSerialized] public bool isStartCount = false;

    //���ꂼ��̓��͏�
    [System.NonSerialized] public bool isOwnerClear = false;
    [System.NonSerialized] public bool isClientClear = false;

    //���͊J�n���
    [System.NonSerialized] public bool isAllClear = false;

    //�񓚃f�[�^
    private List<int> answer = new List<int>();

    private int frameCount = 0;


    //�񓚃f�[�^������1�x�������Ȃ��p
    private bool isAnswerFirst = true;
    //�A�����b�N�{�^���N����Ԃ�A���œ����ȂȂ��p
    private bool isUnlockButtonStartFirst = true;
    //�N���A�󋵋��L��A���œ����Ȃ��p
    private bool isOwnerClearFirst = true;
    private bool isClientClearFirst = true;
    private bool isStartCountFisrt = true;


    private enum Key
    {
        A, B, X, Y, Right, Left, Up, Down, R1, R2, L1, L2
    }

    private void Start()
    {
        timeLimitSlider.GetComponent<Slider>().value = 1;

        //Gimmick�ɂ���Ĕ��̊J�����߂�
        if (gimmickNum == 0)
            door.SetActive(true);
        if (gimmickNum == 1)
            door.SetActive(false);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //�}�X�^�[�̎�������ݒ肵�ăf�[�^��n��
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //2P�����Ȃ����͍��Ȃ�
            if (ManagerAccessor.Instance.dataManager.player2 != null)
            {
                //�ŏ��̈�񂾂�
                if (isAnswerFirst)
                {
                    //�����̐����ƃf�[�^�̎󂯓n��
                    for (int i = 0; i < inputKey; i++)
                    {
                        answer.Add(Random.Range(0, 12));
                        //�A���œ��������ɂȂ�Ȃ����߂̏���
                        while (true)
                        {
                            if (i != 0)
                            {
                                if (answer[i] == answer[i - 1])
                                    answer[i] = Random.Range(0, 12);
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, answer[i]);
                    }
                    ManagerAccessor.Instance.dataManager.chat.text = answer[0].ToString() + ":" + answer[1].ToString() + ":" + answer[2].ToString() + ":" + answer[3].ToString() + ":" + answer[4].ToString();

                    //�����ݒ�
                    AnswerSet();

                    isAnswerFirst = false;
                }
            }
        }
        //�}�X�^�[�łȂ����A�����f�[�^���󂯎��܂őҋ@
        else
        {
            if (answer.Count != 0)
            {
                //�ŏ��̈�񂾂�
                if (isAnswerFirst)
                {
                    ManagerAccessor.Instance.dataManager.chat.text = answer[0].ToString() + ":" + answer[1].ToString() + ":" + answer[2].ToString() + ":" + answer[3].ToString() + ":" + answer[4].ToString();
                    //�����ݒ�
                    AnswerSet();

                    isAnswerFirst = false;
                }
            }
        }

        //�N���A���Ă��Ȃ��Ƃ�
        if (!isAllClear)
        {
            //���͊J�n���̎��Ԍv�Z
            if (isStartCount)
            {
                if (isStartCountFisrt)
                {
                    photonView.RPC(nameof(RpcShareIsStartCount), RpcTarget.Others, isStartCount);
                    isStartCountFisrt = false;
                }

                frameCount++;
                if (frameCount == timeLimit * 60)
                {
                    //���͏󋵏�����
                    isStartCount = false;
                    frameCount = 0;

                    //�~�X���J�E���g
                    if (!isOwnerClear)
                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                    if (!isClientClear)
                        CallRpcShareInputMiss();

                    //�N���A�󋵏�����
                    isOwnerClear = false;
                    isClientClear = false;

                    //���������
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        //�N���A�󋵏�����
                        for (int j = 0; j < answer.Count; j++)
                        {
                            gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation[j] = false;
                        }
                    }
                }

                //�c�莞�ԕ\��
                timeLimitSlider.GetComponent<Slider>().value = 1 - (float)frameCount / (float)(timeLimit * 60);
                //timetext.text = frameCount.ToString() + "/" + timeLimit * 60;
            }
            else
            {
                if (!isStartCountFisrt)
                {
                    photonView.RPC(nameof(RpcShareIsStartCount), RpcTarget.Others, isStartCount);
                    isStartCountFisrt = true;
                }
            }



            //Player1�̃N���A��񑗐M
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (isOwnerClear)
                {
                    if (isOwnerClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, true);
                        isOwnerClearFirst = false;
                    }
                }
                else
                {
                    if (!isOwnerClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, false);
                        isOwnerClearFirst = true;
                    }
                }
            }
            //Player2�̃N���A��񑗐M
            else
            {
                if (isClientClear)
                {
                    if (isClientClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, true);
                        isClientClearFirst = false;
                    }
                }
                else
                {
                    if (!isClientClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, false);
                        isClientClearFirst = true;
                    }
                }
            }

            //�����N���A�����炱�̏����𔲂���
            if (isOwnerClear && isClientClear)
            {
                isAllClear = true;
            }


        }
        else
        {
            //��������
            if (gimmickNum == 0)
                door.SetActive(false);
            if (gimmickNum == 1)
                door.SetActive(true);

            answerArea.SetActive(false);
            timeLimitSlider.SetActive(false);
        }

    }


    //�����ݒ�p�֐�
    private void AnswerSet()
    {
        GameObject clone = null;
        SpriteManager spriteManager = ManagerAccessor.Instance.spriteManager;

        //�������͗p�u���b�N�ɓ����f�[�^��n��
        for (int i = 0; i < gimmickButton.Count; i++)
        {
            gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;

            //�N���A�󋵏�����
            for (int j = 0; j < answer.Count; j++)
            {
                gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation.Add(false);
            }
        }

        //�񓚕`��p
        for (int i = 0; i < answer.Count; i++)
        {
            switch (answer[i])
            {
                case (int)Key.A:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowDown;
                    break;
                case (int)Key.B:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowRight;
                    break;
                case (int)Key.X:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowLeft;
                    break;
                case (int)Key.Y:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowUp;
                    break;
                case (int)Key.Right:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossRight;
                    break;
                case (int)Key.Left:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossLeft;
                    break;
                case (int)Key.Up:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossUp;
                    break;
                case (int)Key.Down:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossDown;
                    break;
                case (int)Key.R1:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.R1;
                    break;
                case (int)Key.R2:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.R2;
                    break;
                case (int)Key.L1:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.L1;
                    break;
                case (int)Key.L2:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.L2;
                    break;
            }
        }
    }


    //�}�X�^�[�T�C�h�Ō��߂����������L
    [PunRPC]
    private void RpcShareAnswer(int ans)
    {
        answer.Add(ans);
    }

    //isUnlockButtonStart�����L
    [PunRPC]
    private void RpcShareIsUnlockButtonStart(bool data)
    {
        ManagerAccessor.Instance.dataManager.isUnlockButtonStart = data;
    }

    //isStartCount�����L
    [PunRPC]
    private void RpcShareIsStartCount(bool data)
    {
        isStartCount = data;
    }


    //�N���A�󋵂����L
    [PunRPC]
    private void RpcShareIsClear(bool data)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            isClientClear = data;
        }
        else
        {
            isOwnerClear = data;
        }
    }

    //�{�^���Ƀv���C���[���G��Ă��邩�ǂ���
    public void CallRpcShareHitUnlockButton(bool button1, bool data)
    {
        photonView.RPC(nameof(RpcShareHitUnlockButton), RpcTarget.All, button1, data);
    }
    [PunRPC]
    private void RpcShareHitUnlockButton(bool button1, bool data)
    {
        if (button1)
            isHitUnlockButton1 = data;
        else
            isHitUnlockButton2 = data;
    }

    //�v���C���[���G��Ă����Ԃ��ǂ������L
    public void CallRpcShareHitPlayer(bool isowner, bool data)
    {
        photonView.RPC(nameof(RpcShareHitPlayer), RpcTarget.All, isowner, data);
    }
    [PunRPC]
    private void RpcShareHitPlayer(bool isowner, bool data)
    {
        if (isowner)
            isHitPlayer1 = data;
        else
            isHitPlayer2 = data;
    }

    //�v���C���[���G��Ă����Ԃ��ǂ������L
    public void CallRpcShareHitPlayerName(int name,int objNum)
    {
        photonView.RPC(nameof(RpcShareHitPlayerName), RpcTarget.All, name, objNum);
    }
    [PunRPC]
    private void RpcShareHitPlayerName(int name, int objNum)
    {
        if (name == 0)
            gimmickButton[objNum].GetComponent<GimmickUnlockButton>().managementPlayerName = "Player1";
        if (name == 1)
            gimmickButton[objNum].GetComponent<GimmickUnlockButton>().managementPlayerName = "Player2";
        if (name == 2)
            gimmickButton[objNum].GetComponent<GimmickUnlockButton>().managementPlayerName = "CopyKey";
    }


    //�N���C�A���g���Ń~�X������I�[�i�[���ŉ��Z
    public void CallRpcShareInputMiss()
    {
        photonView.RPC(nameof(RpcShareInputMiss), RpcTarget.Others);
    }
    [PunRPC]
    private void RpcShareInputMiss()
    {
        ManagerAccessor.Instance.dataManager.clientMissCount++;
    }
}
