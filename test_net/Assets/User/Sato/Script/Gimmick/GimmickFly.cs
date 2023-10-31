using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickFly : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�����܂ł̃N�[���^�C��")]
    private int CoolTime;

    [SerializeField, Header("�ړ���")]
    private float MovePower;

    [SerializeField, Header("��]��")]
    private int MoveAngle;

    private DataManager dataManager;        //�f�[�^�}�l�[�W���[

    private bool isStart = false;           //���P�b�g�J�n

    private int ownerTapNum = 0;            //���ꂼ��̃^�b�v��
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;     //���ꂼ��̃^�b�v����߂���̃N�[���^�C��
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;   //���ꂼ��N�[���^�C������
    private bool isClientCoolTime = false;

    //�A���Ŕ������Ȃ�
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
        //�f�[�^�}�l�[�W���[�擾
        dataManager = ManagerAccessor.Instance.dataManager;

        if (dataManager.isOwnerInputKey_CB)
        {
            isStart = true;
        }

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
                    Debug.Log("bbb");
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
                    Debug.Log("ccc");
                }
            }

            //�{���ݒ�
            if (!isOwnerCoolTime && !isClientCoolTime)
                mag = 2;
            else if (!isOwnerCoolTime)
                mag = 1;
            else if (!isClientCoolTime)
                mag = 1;
            else
                mag = 0;

            //�p�x�ݒ�
            float rad = dis * Mathf.Deg2Rad; //�p�x�����W�A���p�ɕϊ�

            Debug.Log(isOwnerCoolTime+":"+ isClientCoolTime);

            //�ړ������ݒ�
            Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);
            Vector2 input;
            input.x = transform.position.x + power.x * MovePower;
            input.y = transform.position.y + power.y * MovePower;

            if (isOwnerCoolTime && isClientCoolTime)
            {
                input.y = transform.position.y - 0.005f;
                Debug.Log("aaa");
            }

            //�ړ��ʁA�p�x�̑��
            transform.position = input;
            transform.eulerAngles = new Vector3(0, 0, -dis);
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

}
