using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class GoalSystem : CGimmick
{
    [SerializeField, Header("�t�F�[�h���x")] private int FeedSpeed;

    [SerializeField, Header("�t�F�[�h�I����̃V�[���؂�ւ����x")] private int FeedEndSpeed;

    [SerializeField, Header("�S�[��SE")] AudioClip goalSE;

    [SerializeField, Header("SE��炷��")] private int SEPlayNum;

    [SerializeField, Header("SE��炷�Ԋu")] private int SEInterbal;

    private AudioSource audioSource;

    //�S�[���ɐG��Ă��邩�ǂ���
    private bool isOwnerEnter = false;
    private bool isClientEnter = false;

    //�t�F�[�h���I��������ǂ���
    private bool isOwnerFadeEnd = false;
    private bool isClientFadeEnd = false;

    private int frameCount = 0;
    private int SECount = 0;

    private bool first = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�G��Ă���Ƃ��\���L�[��
        if (isOwnerEnter)
        {
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP)
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER1);
                    first = false;
                }
            }
        }
        if (isClientEnter)
        {
            if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP)
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER2);
                    first = false;
                }
            }
        }

        //�S�[����̃t�F�[�h����
        if (ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear)
        {
            //�����ׂ��{�^���̉摜��\��
            ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(false);

            if (ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
            else
                isOwnerFadeEnd = true;

            if(PhotonNetwork.IsMasterClient)
            {
                SECount++;
                if (SECount <= SECount * SEPlayNum)
                {
                    if (SECount % SEInterbal == 0)
                    {
                        audioSource.PlayOneShot(goalSE);
                    }
                }
            }
        }

        if (ManagerAccessor.Instance.dataManager.GetSetIsClientClear)
        {
            //�����ׂ��{�^���̉摜��\��
            ManagerAccessor.Instance.dataManager.player2.transform.GetChild(0).gameObject.SetActive(false);

            if (ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
            else
                isClientFadeEnd = true;

            //SE�Đ�
            if (!PhotonNetwork.IsMasterClient)
            {
                SECount++;
                if (SECount <= SECount * SEPlayNum)
                {
                    if (SECount % SEInterbal == 0)
                    {
                        audioSource.PlayOneShot(goalSE);
                    }
                }
            }
        }

        //�N���A����ƃ��U���g��ʕ\��
        if (isOwnerFadeEnd && isClientFadeEnd)
        {
            frameCount++;
            if (frameCount == FeedEndSpeed)
                ManagerAccessor.Instance.dataManager.isClear = true;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, true);
                isOwnerEnter = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, true);
                isClientEnter = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //�����ׂ��{�^���̉摜��\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, false);
                isOwnerEnter = false;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //�����ׂ��{�^���̉摜��\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, false);
                isClientEnter = false;
            }
        }
    }

    [PunRPC]
    private void RpcClearCheck(int master)
    {
        //�I�[�i�[�̎�
        if (master == PLAYER1)
        {
            ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear = true;
        }
        else if (master == PLAYER2)
        {
            ManagerAccessor.Instance.dataManager.GetSetIsClientClear = true;
        }
    }

    [PunRPC]
    protected void RpcShareIsNotOpenBox(bool isOwner, bool data)
    {
        if (isOwner)
            ManagerAccessor.Instance.dataManager.isOwnerNotOpenBox = data;
        else
            ManagerAccessor.Instance.dataManager.isClientNotOpenBox = data;
    }

}
