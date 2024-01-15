using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StoneGenerate : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject GenelateErea_L;//�����ꏊ��
    [SerializeField] private GameObject GenelateErea_R;//�����ꏊ�E


    [SerializeField] private GameObject[] Stones = new GameObject[3];//�傫����

    [SerializeField] private GameObject StoneParentObject;//����������̐e�I�u�W�F�N�g

    private int stonetype = 0;//���������̎�ނ������_���Ɍ��߂�

    private float height;

    [SerializeField, Header("���b�Ԃ��ɐ������邩�ݒ�ł���")] private float spawnInterval; // �����Ԋu

    [SerializeField, Header("spawnInterval�Őݒ肵���b���Ƃɉ��������邩�ݒ�ł���")] private int spawnstone;      // ���ɐ��������̐�

    [SerializeField, Header("�ړ����x")] private float moveSpeed;

    private float firstspeed = 0;//�ŏ��ɐݒ肵��moveSpeed��ۑ�����

    private bool movestop = false;//DeathErea�̈ړ����~�߂�

    private float timer = 0f;//���Ԃ��J�E���g

    private Vector2 spawnPosition;//�␶���ꏊ

    private GameObject ParentObj;//��̐e�I�u�W�F�N�g�ɂȂ����

    private GameObject ChildObj;//����������I�u�W�F�N�g���q�I�u�W�F�N�g�ɂ���

    private Rigidbody2D rb; // Rigidbody2D��ێ�����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        height = gameObject.GetComponent<Renderer>().bounds.size.y;

        ParentObj = Instantiate(StoneParentObject, Vector2.zero, Quaternion.identity);

        firstspeed = moveSpeed;

        // �Q�[���I�u�W�F�N�g�ɃA�^�b�`���ꂽRigidbody2D�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ////���O�ɂ��ړ��X�s�[�h���ς��̂�h�����߃v���C���[���̈ړ��������擾����
        //if (PhotonNetwork.IsMasterClient)
        //{

           
        //}

       

        // �I�u�W�F�N�g���E�Ɉړ�������
        if (!movestop && !ManagerAccessor.Instance.dataManager.isPause)
        {
            Debug.Log("�ړ���");
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        

        if (ManagerAccessor.Instance.dataManager.isPause ||
            ManagerAccessor.Instance.dataManager.isClear ||
            ManagerAccessor.Instance.dataManager.isDeth)
        {
            moveSpeed = 0;//�f�X�G���A�Ɋ|�����Ă��鑬�x��0�ɂ���
            rb.constraints = RigidbodyConstraints2D.FreezeAll;//�������̈ړ����~�߂�
        }
        else//�|�[�Y���łȂ���Ί�𐶐���������
        {
            //�|�[�Y�������Ɏ~�߂��ړ���������������
            moveSpeed = firstspeed;
            //rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            rb.constraints =
�@�@�@�@�@�@RigidbodyConstraints2D.FreezeRotation |
�@�@�@�@�@�@RigidbodyConstraints2D.FreezePositionY;

            timer += Time.deltaTime;

            //�����Ŋ�𐶐����鎞�Ԃ��v��
            if (timer >= spawnInterval)
            {
                for (int i = 0; i < spawnstone; i++)
                {
                    SpawnObject();
                }

                timer = 0f;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�X�g�b�v�G���A�ɓ���ƈړ����~�߂�
        if (collision.gameObject.tag == "StopErea")
        {
            Debug.Log("�G���A�X�g�b�v");
            movestop = true;

        }

    }


    public void SpawnObject()
    {

        stonetype = Random.Range(1, 3);//��̎�ނ������_���I�o

        float randomX = Random.Range(GenelateErea_L.transform.localPosition.x, GenelateErea_R.transform.localPosition.x);//�����_����X���W

        spawnPosition = new Vector2(randomX, height);//�����ʒu��ݒ�

        var worldPoint = transform.TransformPoint(spawnPosition);//���[���h���W�ɕϊ�

        worldPoint.y = height;//���������[���h���W�ɂ���Ƃ����̂Ń��[�J���ɖ߂�



        ChildObj = Instantiate(Stones[stonetype], worldPoint, Quaternion.identity);

        ChildObj.transform.parent = ParentObj.transform; // �����������ParentObj�̎q�I�u�W�F�N�g�ɂ���
    }




}
