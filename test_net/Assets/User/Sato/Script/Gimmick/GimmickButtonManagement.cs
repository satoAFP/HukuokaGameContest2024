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

    [SerializeField, Header("����SE")] AudioClip successSE;
    [SerializeField, Header("���sSE")] AudioClip failureSE;

    private AudioSource audioSource;

    //��������
    private bool isSuccess = false;
    //���s����
    private bool isFailure = false;

    //�t���[���J�E���g�p
    private int count = 0;

    //��񂵂�����Ȃ�
    private bool first = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Gimmick�ɂ���Ĕ��̊J�����߂�
        if (gimmickNum == 0)
            door.SetActive(true);
        if (gimmickNum == 1)
            door.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
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

                                //SE�Đ�
                                audioSource.PlayOneShot(successSE);
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

                            //SE�Đ�
                            audioSource.PlayOneShot(failureSE);
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
            if (first)
            {
                if (gimmickNum == 0)
                    door.SetActive(false);
                if (gimmickNum == 1)
                    door.SetActive(true);

                gameObject.transform.Find("Button1").transform.localScale = new Vector2(-1, 1);
                gameObject.transform.Find("Button2").transform.localScale = new Vector2(-1, 1);

                //�G�t�F�N�g����
                Instantiate(ManagerAccessor.Instance.dataManager.StarEffect, gameObject.transform.Find("Button1"));
                Instantiate(ManagerAccessor.Instance.dataManager.StarEffect, gameObject.transform.Find("Button2"));

                first = false;
            }
        }

    }


    //�{�^�����͏��𑊎�ɑ��M
    [PunRPC]
    protected void RpcShareIsSuccess()
    {
        isSuccess = true;
    }



}
