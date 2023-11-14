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
    private bool isMoveStart = false;

    //frame�J�E���g
    private int frameCount = 0;


    //�A���œ���Ȃ�����
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //�f�[�^�}�l�[�W���[�擾
        dataManager = ManagerAccessor.Instance.dataManager;


        //���͂���Ă��Ȃ����S�Ă�����������
        if (!dataManager.isOwnerInputKey_C_R_RIGHT && !dataManager.isOwnerInputKey_C_R_LEFT &&
            !dataManager.isOwnerInputKey_C_R_UP && !dataManager.isOwnerInputKey_C_R_DOWN)
        {
            count = 0;
            isMoveStart = false;
            isStop = true;
            first = true;

            isRight = false;
            isLeft = false;
            isUp = false;
            isDown = false;
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
        if (hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitRight)
        {
            RightRotate();
            movePower.x = MovePower;
        }
        else if (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitLeft)
        {
            LeftRotate();
            movePower.x = -MovePower;
        }

        //�z��̔��Ε����ɉ�]���������Z�b�g�p
        if (count - memCount == 2)
        {
            isMoveStart = false;
            count = 0;
        }


        //��������]������
        if (count >= 4)
        {
            isMoveStart = true;
            count = 0;
        }

        //�ړ��J�n
        if (isMoveStart)
        {
            transform.position += movePower;
            frameCount++;
        }

        //���W����
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (frameCount == PosUpData)
            {
                photonView.RPC(nameof(RpcSharePos), RpcTarget.Others, transform.position.x, transform.position.y);
                frameCount = 0;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //�����ׂ��{�^���̉摜�\��
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                collision.transform.GetChild(1).gameObject.SetActive(true);

                if (dataManager.isOwnerHitRight)
                {
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.RStickRight;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateR;
                }

                if (dataManager.isOwnerHitLeft)
                {
                    collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.RStickLeft;
                    collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateL;
                }

            }

            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(true);

            if (dataManager.isOwnerHitRight)
            {
                collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.RStickRight;
                collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateR;
            }

            if (dataManager.isOwnerHitLeft)
            {
                collision.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.RStickLeft;
                collision.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().runtimeAnimatorController = ManagerAccessor.Instance.spriteManager.RStickRotateL;
            }

            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(false);

            hitOwner = false;
        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(false);

            hitClient = false;
        }
    }

    //�E��]����
    private void RightRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT && isRight)
        {
            if (isRight)
                count++;

            isRight = false;
            isDown = true;
        }

        if (dataManager.isOwnerInputKey_C_R_DOWN && isDown)
        {
            if (isDown)
                count++;

            isDown = false;
            isLeft = true;
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT && isLeft)
        {
            if (isLeft)
                count++;

            isLeft = false;
            isUp = true;
        }

        if (dataManager.isOwnerInputKey_C_R_UP && isUp)
        {
            if (isUp)
                count++;

            isUp = false;
            isRight = true;
        }
    }

    //����]����
    private void LeftRotate()
    {
        if (dataManager.isOwnerInputKey_C_R_RIGHT && isRight)
        {
            if (isRight)
                count++;

            isRight = false;
            isUp = true;
        }

        if (dataManager.isOwnerInputKey_C_R_UP && isUp)
        {
            if (isUp)
                count++;

            isUp = false;
            isLeft = true;
        }

        if (dataManager.isOwnerInputKey_C_R_LEFT && isLeft)
        {
            if (isLeft)
                count++;

            isLeft = false;
            isDown = true;
        }


        if (dataManager.isOwnerInputKey_C_R_DOWN && isDown)
        {
            if (isDown)
                count++;

            isDown = false;
            isRight = true;
        }
    }

    [PunRPC]
    private void RpcSharePos(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }
}
