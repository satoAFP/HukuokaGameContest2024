using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickRotateBomb : MonoBehaviourPunCallbacks
{
    //�f�[�^�}�l�[�W���[�擾�p
    DataManager dataManager = null;

    [SerializeField, Header("�ړ���")] private float MovePower;

    [SerializeField, Header("���W�X�V�^�C�~���O")] private int PosUpData;

    [SerializeField, Header("�{�^���\���I���܂ł̃t���[��")] private int DisplayTime;

    [SerializeField, Header("��̓��͂ɂ����̃t���[��")] private int rotateSpeed;

    //�R�s�[�L�[�̂ݎ擾�p
    private GameObject copyKeyObj = null;
    private string hitObjName = null;

    //1P�A2P�����ꂼ�ꓖ�����Ă��锻��
    private bool hitOwner = false;
    private bool hitClient = false;

    //���݂̓��͂��Ă������
    private bool isRight = false;
    private bool isLeft = false;
    private bool isUp = false;
    private bool isDown = false;

    //���͂���Ă��Ȃ���
    private bool isStop = false;

    //���Ԃɓ��͂����ƃJ�E���g�����
    private int count = 0;

    //�ړ�����
    private Vector3 movePower = Vector3.zero;

    //�ړ��J�n
    private bool isOwnerMoveStart = false;
    private bool isClientMoveStart = false;

    //frame�J�E���g
    private int frameCount = 0;

    //�_�ł��Ȃ����߂̃{�^���\����������܂ł̃��O�J�E���g
    private int displayTimeCount = 0;

    //��]���x��}�邽�߂̃J�E���g
    private int rotateSpeedCount = 0;

    //�A���œ���Ȃ�����
    private bool first = true;
    private bool first1 = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //�f�[�^�}�l�[�W���[�擾
        dataManager = ManagerAccessor.Instance.dataManager;


        //���͂���Ă��Ȃ����S�Ă�����������
        if (!dataManager.isOwnerInputKey_C_R_RIGHT && !dataManager.isOwnerInputKey_C_R_LEFT &&
            !dataManager.isOwnerInputKey_C_R_UP && !dataManager.isOwnerInputKey_C_R_DOWN)
        {
            if (first1)
            {
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
                else
                    photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);
                first1 = false;
            }

            count = 0;
            rotateSpeedCount = 0;
            isStop = true;
            first = true;

            isRight = false;
            isLeft = false;
            isUp = false;
            isDown = false;
        }
        else
        {
            first1 = true;
        }

        //���͂���Ă��Ȃ��Ƃ��A�ŏ��ɓ��͂��ꂽ���������]���n�܂�
        if (isStop)
        {
            if (dataManager.isOwnerInputKey_C_R_RIGHT)
            {
                if (first)
                {
                    isRight = true;
                    isStop = false;
                    first = false;
                }
            }

            if (dataManager.isOwnerInputKey_C_R_LEFT)
            {
                if (first)
                {
                    isLeft = true;
                    isStop = false;
                    first = false;
                }
            }

            if (dataManager.isOwnerInputKey_C_R_UP)
            {
                if (first)
                {
                    isUp = true;
                    isStop = false;
                    first = false;
                }
            }

            if (dataManager.isOwnerInputKey_C_R_DOWN)
            {
                if (first)
                {
                    isDown = true;
                    isStop = false;
                    first = false;
                }
            }
        }

        //�O�t���[����count�L���p
        int memCount = count;

        //��]���͏��擾����
        if (hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitRight && dataManager.isOwnerInputKey_C_L_RIGHT && dataManager.isClientInputKey_C_L_RIGHT)
        {
            RightRotate();
            movePower.x = MovePower;
        }
        else if (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitLeft && dataManager.isOwnerInputKey_C_L_LEFT && dataManager.isClientInputKey_C_L_LEFT)
        {
            LeftRotate();
            movePower.x = -MovePower;
        }

        //�z��̔��Ε����ɉ�]���������Z�b�g�p
        if (count - memCount == 2)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
            else
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);

            count = 0;
        }

        Debug.Log("isOwnerMoveStart" + isOwnerMoveStart + "isClientMoveStart" + isClientMoveStart);

        //��������]������
        if (count >= 4)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, true);
            else
                photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, true);

            count = 0;
        }

        //�ړ��J�n
        if (isOwnerMoveStart && isClientMoveStart) 
        {
            transform.position += movePower;
            frameCount++;
        }

        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //���W����
            if (frameCount == PosUpData)
            {
                photonView.RPC(nameof(RpcSharePos), RpcTarget.Others, transform.position.x, transform.position.y);
                frameCount = 0;
            }

            //�摜�̔�\���̃��O�𔭐�������
            if(!hitOwner)
            {
                displayTimeCount++;
                if (displayTimeCount == DisplayTime)
                {
                    if (PhotonNetwork.IsMasterClient)
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
                    else
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);

                    if (hitObjName == "Player1")
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(1).gameObject.SetActive(false);
                    if (hitObjName == "CopyKey")
                        copyKeyObj.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            //�摜�̔�\���̃��O�𔭐�������
            if (!hitClient)
            {
                displayTimeCount++;
                if (displayTimeCount == DisplayTime)
                {
                    if (PhotonNetwork.IsMasterClient)
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, true, false);
                    else
                        photonView.RPC(nameof(RpcShareIsMoveStart), RpcTarget.All, false, false);

                    ManagerAccessor.Instance.dataManager.player2.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //�����ׂ��{�^���̉摜�\��
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (dataManager.isOwnerHitRight)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickRight;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateR;
                }

                if (dataManager.isOwnerHitLeft)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickLeft;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateL;
                }

            }

            hitOwner = true;
            displayTimeCount = 0;
        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜�\��
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {

                if (dataManager.isClientHitRight)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickRight;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateR;
                }

                if (dataManager.isClientHitLeft)
                {
                    collision.transform.GetChild(1).gameObject.SetActive(true);
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.LStickLeft;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateL;
                }
            }

            hitClient = true;
            displayTimeCount = 0;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            hitOwner = false;
            hitObjName = collision.gameObject.name;
        }

        if (collision.gameObject.name == "CopyKey") 
        {
            copyKeyObj = collision.gameObject;
        }

        if (collision.gameObject.name == "Player2")
        {
            hitClient = false;
        }
    }

    //�E��]����
    private void RightRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT)
        {
            if (isRight)
            {
                count++;
                rotateSpeedCount = 0;

                isRight = false;
                isDown = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_DOWN)
        {
            if (isDown)
            {
                count++;
                rotateSpeedCount = 0;

                isDown = false;
                isLeft = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT)
        {
            if (isLeft)
            {
                count++;
                rotateSpeedCount = 0;

                isLeft = false;
                isUp = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_UP)
        {
            if (isUp)
            {
                count++;
                rotateSpeedCount = 0;

                isUp = false;
                isRight = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }
    }

    //����]����
    private void LeftRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT)
        {
            if (isRight)
            {
                count++;
                rotateSpeedCount = 0;

                isRight = false;
                isUp = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_UP)
        {
            if (isUp)
            {
                count++;
                rotateSpeedCount = 0;

                isUp = false;
                isLeft = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT)
        {
            if (isLeft)
            {
                count++;
                rotateSpeedCount = 0;

                isLeft = false;
                isDown = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }


        if (dataManager.isOwnerInputKey_C_R_DOWN)
        {
            if (isDown)
            {
                count++;
                rotateSpeedCount = 0;

                isDown = false;
                isRight = true;
            }

            //��莞�ԓ������Ȃ��Ɖ�]���Ă��Ȃ��Ƃ݂Ȃ����
            rotateSpeedCount++;
            if (rotateSpeedCount == rotateSpeed)
            {
                count = 0;
            }
        }
    }

    [PunRPC]
    private void RpcSharePos(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    [PunRPC]
    private void RpcShareIsMoveStart(bool isManage, bool data)
    {
        if (isManage)
            isOwnerMoveStart = data;
        else
            isClientMoveStart = data;
    }
}
