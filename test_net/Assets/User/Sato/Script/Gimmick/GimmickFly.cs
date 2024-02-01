using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class GimmickFly : MonoBehaviourPunCallbacks
{
    const int START_WAIT_TIME = 20;//���͂��󂯕t���Ȃ�����

    [SerializeField, Header("�����G�t�F�N�g")] private GameObject BombEffect;

    [SerializeField, Header("�ړ���")]
    private float MovePower;

    [SerializeField, Header("��]��")]
    private int MoveAngle;

    [SerializeField, Header("�S�[����̉�]��")]
    private float correctionAngle;

    [SerializeField, Header("�����܂ł̃N�[���^�C��")]
    private int CoolTime;

    [SerializeField, Header("�S�[����N���A�\���܂ł̃t���[��")]
    private int GoalTime;

    [SerializeField, Header("�{�^����������܂ł̉�")]
    private int DestroyCount;

    [SerializeField, Header("���΂ɓ��������㉽�t���[���㎀�S���邩")]
    private int HitStoneDethTime;

    [SerializeField, Header("��荞��SE")] AudioClip rideSE;
    [SerializeField, Header("���SE")] AudioClip flySE;

    private AudioSource audioSource;

    private DataManager dataManager;        //�f�[�^�}�l�[�W���[

    private Rigidbody2D rigidbody;          //���W�b�g�{�f�B

    private GameObject player1;              //�v���C���[�I�u�W�F�N�g�擾�p
    private GameObject player2;

    private bool isStart = false;           //���P�b�g�J�n
    private bool isGoal = false;            //�S�[������

    private int ownerTapNum = 0;            //���ꂼ��̃^�b�v��
    private int clientTapNum = 0;

    private int ownerCoolTimeCount = 0;     //���ꂼ��̃^�b�v����߂���̃N�[���^�C��
    private int clientCoolTimeCount = 0;

    private bool isOwnerCoolTime = false;   //���ꂼ��N�[���^�C������
    private bool isClientCoolTime = false;

    private float dis = 0;                  //�^�b�v�񐔂̍�

    private bool isHit = false;             //�����ꂼ�̃v���C���[�����P�b�g�ɐG��Ă��邩�ǂ���

    private bool isOwnerStart = false;      //�����ꂼ�̃v���C���[�����P�b�g����J�n�����ǂ���
    private bool isClientStart = false;

    private int startWaitTimeCount = 0;     //���P�b�g�J�n�����͂���������󂯕t���Ȃ�
    private int goalCount = 0;              //�S�[����N���A��\������܂ł̃J�E���g
    private int tapCount = 0;

    private bool isHitStone = false;        //���΂ɓ�����������
    private int hitStoneDethTimeCount = 0;  //���΂ɓ��������㉽�t���[���㎀�S���邩�J�E���g

    //�A���Ŕ������Ȃ�
    private bool startFirst = true;
    private bool startFirst2 = true;
    private bool startOtherFirst = true;
    private bool ownerFirst = true;
    private bool clientFirst = true;
    private bool OwnerCoolTimeFirst = true;
    private bool ClientCoolTimeFirst = true;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        ownerCoolTimeCount = CoolTime;
        clientCoolTimeCount = CoolTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�f�[�^�}�l�[�W���[�擾
        dataManager = ManagerAccessor.Instance.dataManager;

        if (!isHitStone)
        {
            //�|�[�Y���͎~�܂�
            if (!ManagerAccessor.Instance.dataManager.isClear &&
                !ManagerAccessor.Instance.dataManager.isDeth &&
                !ManagerAccessor.Instance.dataManager.isPause)
            {
                rigidbody.simulated = true;

                if (!isStart)
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        //���P�b�g�ɐG��Ă����Ԃ�B���͂Ŕ��ˑҋ@���
                        if (dataManager.isOwnerInputKey_CB)
                        {
                            if (isHit)
                            {
                                if (startFirst)
                                {
                                    //SE�Đ�
                                    if (!isOwnerStart)
                                        audioSource.PlayOneShot(rideSE);

                                    photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, true);

                                    //�v���C���[�̃p�����[�^�ύX
                                    ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(false);
                                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = true;
                                    ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = false;
                                    ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = false;
                                    GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform;


                                    startFirst = false;
                                }
                            }
                        }
                        else
                            startFirst = true;
                    }
                    else
                    {
                        //���P�b�g�ɐG��Ă����Ԃ�B���͂Ŕ��ˑҋ@���
                        if (dataManager.isClientInputKey_CB)
                        {
                            if (isHit)
                            {
                                if (startFirst)
                                {
                                    //SE�Đ�
                                    if (!isClientStart)
                                        audioSource.PlayOneShot(rideSE);

                                    photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, true);

                                    //�v���C���[�̃p�����[�^�ύX
                                    ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(false);
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = true;
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = false;
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = false;
                                    GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform;


                                    startFirst = false;
                                }
                            }
                        }
                        else
                            startFirst = true;
                    }

                    //���ꂼ�ꃍ�P�b�g���ˏ�Ԃ̎��A�ʂ̉�ʂ̎��g�̃I�u�W�F�N�g����\���ɂ���
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (isClientStart)
                        {
                            if (startOtherFirst)
                            {
                                //�v���C���[�̃p�����[�^�ύX
                                ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(false);
                                ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = true;
                                ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = false;
                                ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = false;

                                startOtherFirst = false;
                            }
                        }
                        else
                        {
                            if (!startOtherFirst)
                            {
                                //�v���C���[�̃p�����[�^�ύX
                                ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(true);
                                ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = false;
                                ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = true;
                                ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = true;

                                startOtherFirst = true;
                            }
                        }
                    }
                    else
                    {
                        if (isOwnerStart)
                        {
                            if (startOtherFirst)
                            {
                                //�v���C���[�̃p�����[�^�ύX
                                ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(false);
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = true;
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = false;
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = false;

                                startOtherFirst = false;
                            }
                        }
                        else
                        {
                            if (!startOtherFirst)
                            {
                                //�v���C���[�̃p�����[�^�ύX
                                ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(true);
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = false;
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = true;
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = true;

                                startOtherFirst = true;
                            }
                        }
                    }
                }

                //��l�Ƃ����ˏ�ԂɂȂ�ƃ��P�b�g�X�^�[�g
                if (isOwnerStart && isClientStart)
                {
                    //�����ׂ��{�^���\��
                    if (!isStart)
                    {
                        transform.GetChild(4).gameObject.SetActive(true);
                        transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

                        //�G�t�F�N�g����
                        GameObject clone = Instantiate(ManagerAccessor.Instance.dataManager.StarEffect);
                        clone.transform.position = transform.position;
                    }

                    isStart = true;
                    ManagerAccessor.Instance.dataManager.isFlyStart = true;

                    //�d�͐ݒ�
                    rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    //�����蔻��ݒ�
                    GetComponent<BoxCollider2D>().isTrigger = false;
                }
                else
                {
                    //�Е��������ˏ�Ԃ̎��͍~��邱�Ƃ��o����
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (dataManager.isOwnerInputKey_CA)
                        {
                            if (isHit)
                            {
                                if (startFirst2)
                                {
                                    photonView.RPC(nameof(RpcShareIsOwnerStart), RpcTarget.All, false);

                                    //�v���C���[�̃p�����[�^�ύX
                                    ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").gameObject.SetActive(true);
                                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().isFly = false;
                                    ManagerAccessor.Instance.dataManager.player1.GetComponent<BoxCollider2D>().enabled = true;
                                    ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = true;
                                    GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = ManagerAccessor.Instance.dataManager.player1.transform;

                                    startFirst2 = false;
                                }
                            }

                        }
                        else
                            startFirst2 = true;
                    }
                    else
                    {
                        if (dataManager.isClientInputKey_CA)
                        {
                            if (isHit)
                            {
                                if (startFirst2)
                                {
                                    photonView.RPC(nameof(RpcShareIsClientStart), RpcTarget.All, false);

                                    //�v���C���[�̃p�����[�^�ύX
                                    ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").gameObject.SetActive(true);
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().isFly = false;
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<BoxCollider2D>().enabled = true;
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<SpriteRenderer>().enabled = true;
                                    ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = true;
                                    GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = ManagerAccessor.Instance.dataManager.player2.transform;

                                    startFirst2 = false;
                                }
                            }
                        }
                        else
                            startFirst2 = true;
                    }

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

                //�I�u�W�F�N�g�擾�X�V
                ManagerAccessor.Instance.dataManager.flyPos = transform.position;

                if (!isGoal)
                {
                    if (isStart)
                    {
                        startWaitTimeCount++;
                        if (startWaitTimeCount >= START_WAIT_TIME)
                        {
                            //���ꂼ��̘A�ŏ���
                            if (dataManager.isOwnerInputKey_CB)
                            {
                                if (ownerFirst)
                                {
                                    ownerTapNum += MoveAngle;
                                    ownerFirst = false;
                                    ownerCoolTimeCount = 0;

                                    if (tapCount < DestroyCount)
                                    {
                                        tapCount++;
                                        transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)(256 / DestroyCount));
                                        transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)(256 / DestroyCount));
                                    }
                                    else
                                    {
                                        transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                                        transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                                        transform.GetChild(4).gameObject.SetActive(false);
                                    }

                                    //SE�Đ�
                                    audioSource.PlayOneShot(flySE);
                                }
                            }
                            else
                            {
                                ownerFirst = true;
                            }

                            if (dataManager.isClientInputKey_CB)
                            {
                                if (clientFirst)
                                {
                                    clientTapNum += MoveAngle;
                                    clientFirst = false;
                                    clientCoolTimeCount = 0;

                                    if (tapCount < DestroyCount)
                                    {
                                        tapCount++;
                                        transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)(256 / DestroyCount));
                                        transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)(256 / DestroyCount));
                                    }
                                    else
                                    {
                                        transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                                        transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                                        transform.GetChild(4).gameObject.SetActive(false);
                                    }

                                    //SE�Đ�
                                    audioSource.PlayOneShot(flySE);
                                }
                            }
                            else
                            {
                                clientFirst = true;
                            }
                        }

                        //�N�[���^�C���J�E���g
                        ownerCoolTimeCount++;
                        clientCoolTimeCount++;

                        //�^�b�v�񐔂̍�
                        dis = ownerTapNum - clientTapNum;
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
                        float rad = -dis * Mathf.Deg2Rad; //�p�x�����W�A���p�ɕϊ�

                        //�ړ������ݒ�
                        Vector2 power = new Vector2(Mathf.Sin(rad) * mag, Mathf.Cos(rad) * mag);

                        //�ړ��ʉ��Z
                        if (!(isOwnerCoolTime && isClientCoolTime))
                        {
                            rigidbody.velocity = new Vector2(power.x * MovePower, power.y * MovePower);
                        }

                        if (isOwnerCoolTime)
                            transform.GetChild(3).gameObject.SetActive(false);
                        else
                            transform.GetChild(3).gameObject.SetActive(true);

                        if (isClientCoolTime)
                            transform.GetChild(2).gameObject.SetActive(false);
                        else
                            transform.GetChild(2).gameObject.SetActive(true);

                        //�e�̍��W�����L����
                        if (PhotonNetwork.LocalPlayer.IsMasterClient)
                        {
                            //�p�x�̑��
                            transform.eulerAngles = new Vector3(0, 0, dis);
                        }
                    }
                }
                else
                {
                    //�����蔻��ݒ�
                    GetComponent<BoxCollider2D>().isTrigger = true;

                    if (dis <= -0.3f || dis >= 0.3f)
                    {
                        GetComponent<Rigidbody2D>().simulated = false;

                        if (dis < 0)
                            dis += 0.1f * correctionAngle;
                        if (dis > 0)
                            dis -= 0.1f * correctionAngle;


                        //�e�̍��W�����L����
                        if (PhotonNetwork.LocalPlayer.IsMasterClient)
                        {
                            //�p�x�̑��
                            transform.eulerAngles = new Vector3(0, 0, dis);
                        }
                    }
                    else
                    {

                        GetComponent<Rigidbody2D>().simulated = true;
                        //�{�̂�^�������ɂ���
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        //��ї����Ă���
                        rigidbody.velocity = new Vector2(0, 2);

                        //�S�[�����Ă����莞�ԂŃ��U���g���o��
                        goalCount++;
                        if (goalCount == GoalTime)
                        {
                            ManagerAccessor.Instance.dataManager.isClear = true;
                        }
                    }
                }
            }
            else
            {
                rigidbody.velocity = new Vector2(0, 0);
                rigidbody.simulated = false;
            }
        }
        else
        {
            //���΂ł̎u�]����
            hitStoneDethTimeCount++;
            if (hitStoneDethTimeCount == HitStoneDethTime)
            {
                ManagerAccessor.Instance.dataManager.isDeth = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
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

            if (collision.gameObject.tag == "FallStone")
            {
                photonView.RPC(nameof(RpcShareHitStone), RpcTarget.All, collision.transform.position.x, collision.transform.position.y);
                Destroy(collision.gameObject);
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

            if (collision.gameObject.tag == "FallStone")
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Goal")
        {
            isGoal = true;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
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
    private void RpcShareHitStone(float x,float y)
    {
        GameObject clone = Instantiate(BombEffect);
        clone.transform.position = new Vector3(x, y, 0);
        clone.transform.localScale = new Vector3(2, 2, 0);
        isHitStone = true;
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
