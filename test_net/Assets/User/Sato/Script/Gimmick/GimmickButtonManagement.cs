using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickButtonManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�M�~�b�N�p�{�^��")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("��")]
    private GameObject door;

    [SerializeField, Header("�ǂ̃M�~�b�N�ɂ��邩")]
    [Header("0:�I�u�W�F�N�g���� / 1:�I�u�W�F�N�g�o��")]
    private int gimmickNum;

    //��������
    private bool isSuccess = false;
    //���s����
    private bool isFailure = false;

    //�t���[���J�E���g�p
    private int count = 0;

    private void Start()
    {
        //Gimmick�ɂ���Ĕ��̊J�����߂�
        if (gimmickNum == 0)
            door.SetActive(true);
        if (gimmickNum == 1)
            door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSuccess)
        {
            //�ǂ��炩�Е������͊J�n�ŃJ�E���g�J�n
            if (gimmickButton[0].GetComponent<GimmickButton>().isButton ||
                gimmickButton[1].GetComponent<GimmickButton>().isButton)
            {
                //�����G��Ă���ꍇ
                if ((gimmickButton[0].GetComponent<GimmickButton>().isOwnerHit && gimmickButton[1].GetComponent<GimmickButton>().isClientHit) ||
                    (gimmickButton[1].GetComponent<GimmickButton>().isOwnerHit && gimmickButton[0].GetComponent<GimmickButton>().isClientHit))
                {
                    //���s�������͂���Ȃ�
                    if (!isFailure)
                    {
                        count++;
                        //���s�܂ł̃t���[���܂�
                        if (count <= ManagerAccessor.Instance.dataManager.MissFrame)
                        {
                            //�������͐�����Gimmick�N��
                            if (gimmickButton[0].GetComponent<GimmickButton>().isButton &&
                                gimmickButton[1].GetComponent<GimmickButton>().isButton)
                            {
                                isSuccess = true;
                            }
                        }
                        else
                        {
                            //���s��񑗐M
                            if (PhotonNetwork.IsMasterClient)
                            {
                                if (gimmickButton[0].GetComponent<GimmickButton>().isButton)
                                {
                                    if (gimmickButton[0].GetComponent<GimmickButton>().isOwnerOnButton)
                                        ManagerAccessor.Instance.dataManager.clientMissCount++;
                                    if (gimmickButton[0].GetComponent<GimmickButton>().isClientOnButton)
                                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                                }
                                if (gimmickButton[1].GetComponent<GimmickButton>().isButton)
                                {
                                    if (gimmickButton[1].GetComponent<GimmickButton>().isOwnerOnButton)
                                        ManagerAccessor.Instance.dataManager.clientMissCount++;
                                    if (gimmickButton[1].GetComponent<GimmickButton>().isClientOnButton)
                                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                                }
                            }

                            //�������ԓ��ɂł��Ȃ���Ύ��s
                            isFailure = true;
                            count = 0;
                        }
                    }
                }
            }
            else
            {
                //�����̓��͉����ōēx���͎�t
                isFailure = false;
            }
        }


        //������������������ƁA�����J��
        if (isSuccess) 
        {
            if (gimmickNum == 0)
                door.SetActive(false);
            if (gimmickNum == 1)
                door.SetActive(true);
        }

    }
}
