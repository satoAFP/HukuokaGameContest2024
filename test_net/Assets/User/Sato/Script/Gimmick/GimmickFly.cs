using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickFly : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�ړ���")]
    private float MovePower;

    [SerializeField, Header("��]��")]
    private int MoveAngle;

    [SerializeField, Header("�����܂ł̃N�[���^�C��")]
    private int CoolTime;

    [SerializeField, Header("�d�͉����x")]
    private float Gravity;

    [SerializeField, Header("�d�͉����x�ő�l")]
    private float GravityMax;

    private DataManager dataManager;        //�f�[�^�}�l�[�W���[

    private GameObject player1;              //�v���C���[�I�u�W�F�N�g�擾�p
    private GameObject player2;

    private bool isStart = false;           //���P�b�g�J�n

    private int ownerTapNum = 0;            //���ꂼ��̃^�b�v��
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;     //���ꂼ��̃^�b�v����߂���̃N�[���^�C��
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;   //���ꂼ��N�[���^�C������
    private bool isClientCoolTime = false;

    private float gravity = 0;              //�d��

    private bool isHit = false;             //�����ꂼ�̃v���C���[�����P�b�g�ɐG��Ă��邩�ǂ���

    private bool isOwnerStart = false;      //�����ꂼ�̃v���C���[�����P�b�g����J�n�����ǂ���
    private bool isClientStart = false;

    //�A���Ŕ������Ȃ�
    private bool startFirst = true;
    private bool ownerFirst = true;
    private bool clientFirst = true;
    private bool OwnerCoolTimeFirst = true;
    private bool ClientCoolTimeFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.GetChild(2).gameObject.transform.eulerAngles = Vector3.zero;

        //�f�[�^�}�l�[�W���[�擾
        dataManager = ManagerAccessor.Instance.dataManager;

        //���P�b�g�ɐG��Ă����Ԃ�B���͂Ŕ��ˑҋ@���
        if (dataManager.isOwnerInputKey_CB)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (isHit)
                {
                    if (startFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, true);
                        transform.GetChild(2).gameObject.SetActive(true);

                        player.SetActive(false);

                        startFirst = false;
                    }
                }
            }
            else
            {
                if (isHit)
                {
                    if (startFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, true);
                        player.SetActive(false);
                        transform.GetChild(2).gameObject.SetActive(true);

                        startFirst = false;
                    }
                }
            }
        }
        else
            startFirst = true;


        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if(isClientStart)
            {

            }
        }
        else
        {
            if (isOwnerStart)
            {

            }
        }

        //��l�Ƃ����ˏ�ԂɂȂ�ƃ��P�b�g�X�^�[�g
        if (isOwnerStart && isClientStart)
        {
            isStart = true;
        }
        else
        {
            //�Е��������ˏ�Ԃ̎��͍~��邱�Ƃ��o����
            if (dataManager.isOwnerInputKey_CA)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    if (isHit)
                    {
                        if (startFirst)
                        {
                            photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, false);
                            player.SetActive(true);
                            transform.GetChild(2).gameObject.SetActive(false);

                            startFirst = false;
                        }
                    }
                }
                else
                {
                    if (isHit)
                    {
                        if (startFirst)
                        {
                            photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, false);
                            player.SetActive(true);
                            transform.GetChild(2).gameObject.SetActive(false);

                            startFirst = false;
                        }
                    }
                }
            }
            else
                startFirst = true;
        }

        //����Ă���Ƃ��̉摜�̕\��
        if (isOwnerStart)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);

        if (isClientStart)
            transform.GetChild(1).gameObject.SetActive(true);
        else
            transform.GetChild(1).gameObject.SetActive(false);


        if (isStart)
        {
            //���ꂼ��̘A�ŏ���
            if (dataManager.isOwnerInputKey_CB)
            {
                if (ownerFirst)
                {
                    ownerTapNum += MoveAngle;
                    ownerFirst = false;
                    ownerCoolTimeCount = 0;
                }
            }
            else
            {
                ownerFirst = true;
                ownerCoolTimeCount++;
            }

            if (dataManager.isClientInputKey_CB)
            {
                if (clientFirst)
                {
                    clientTapNum += MoveAngle;
                    clientFirst = false;
                    clientCoolTimeCount = 0;
                }
            }
            else
            {
                clientFirst = true;
                clientCoolTimeCount++;
            }

            //�^�b�v�񐔂̍�
            int dis = ownerTapNum - clientTapNum;
            //�ړ��ʂ̔{��
            float mag = 0;

            //�N�[���^�C�����ɘA�ł��Ȃ��Ɨ�����
            if (ownerCoolTimeCount >= CoolTime)
            {
                if (OwnerCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, true);
                    OwnerCoolTimeFirst = false;
                }
            }
            else
            {
                if (!OwnerCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsOwnerCoolTime), RpcTarget.All, false);
                    OwnerCoolTimeFirst = true;
                }
            }

            if (clientCoolTimeCount >= CoolTime)
            {
                if (ClientCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, true);
                    ClientCoolTimeFirst = false;
                }
            }
            else
            {
                if (!ClientCoolTimeFirst)
                {
                    photonView.RPC(nameof(RpcShareIsClientCoolTime), RpcTarget.All, false);
                    ClientCoolTimeFirst = true;
                }
            }

            //�{���ݒ�
            if (!isOwnerCoolTime && !isClientCoolTime)
                mag = 2;
            else if (!isOwnerCoolTime || !isClientCoolTime) 
                mag = 1;
            else
                mag = 0;

            //�p�x�ݒ�
            float rad = dis * Mathf.Deg2Rad; //�p�x�����W�A���p�ɕϊ�

            //�ړ������ݒ�
            Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);
            Vector2 input;
            input.x = transform.position.x + power.x * MovePower;
            input.y = transform.position.y + power.y * MovePower;

            //���݂����͂������Ƃ���������
            if (isOwnerCoolTime && isClientCoolTime)
            {
                //�d�͉����̍ő�l�ݒ�
                if (gravity < GravityMax)
                    gravity += Gravity;

                //�d�͉��Z
                input.y = transform.position.y - gravity;
            }
            else
            {
                gravity = 0;
            }

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�ړ��ʁA�p�x�̑��
                transform.position = input;
                transform.eulerAngles = new Vector3(0, 0, -dis);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

                isHit = true;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

                isHit = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                if (!isOwnerStart)
                    isHit = false;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                if (!isClientStart)
                    isHit = false;
            }
        }
    }



    [PunRPC]
    private void RpcShareIsOwnerCoolTime(bool data)
    {
        isOwnerCoolTime = data;
    }

    [PunRPC]
    private void RpcShareIsClientCoolTime(bool data)
    {
        isClientCoolTime = data;
    }

    [PunRPC]
    private void RpcShareIsOwnerStart(bool data)
    {
        isOwnerStart = data;
    }

    [PunRPC]
    private void RpcShareIsClientStart(bool data)
    {
        isClientStart = data;
    }
}
