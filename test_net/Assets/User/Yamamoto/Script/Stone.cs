using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2D��ێ�����ϐ�

    private float speed = 0;//���΂̑��x

    private float acc = 0;//�����x

    [SerializeField, Header("�����_���Ɍ��߂鑬�x�̍Œ�l")]
    private float randspeed_low = 0;
    [SerializeField, Header("�����_���Ɍ��߂鑬�x�̍ő�l")]
    private float randspeed_max = 0;
    [SerializeField, Header("�����_���Ɍ��߂�����x�̍Œ�l")]
    private float randacc_low = 0;
    [SerializeField, Header("�����_���Ɍ��߂�����x�̍ő�l")]
    private float randacc_max = 0;

    // Start is called before the first frame update
    void Start()
    {
        //�����ŗ��΂̑��x�Ɖ����x�����̒l���烉���_���őI��
        acc �@= Random.Range(randacc_low, randacc_max);
        speed = Random.Range(randspeed_low, randspeed_max);

        // �Q�[���I�u�W�F�N�g�ɃA�^�b�`���ꂽRigidbody2D�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //�|�[�Y���̎���̗������~�߂�
        if (ManagerAccessor.Instance.dataManager.isPause)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;//FreezePositionY���I���ɂ���
        }
        else
        {
            acc += 0.05f;//�����x+
            rb.constraints = RigidbodyConstraints2D.None;//FreezePosition����������

            transform.Translate(Vector3.down * (speed + acc) * Time.deltaTime);//���΂̈ړ�����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        //�₪��ʊO�ɍs���ƍ폜
        if (collision.gameObject.tag == "DeathField")
        {
            //Debug.Log("����[��");
            Destroy(this.gameObject);
        }
    }
}
