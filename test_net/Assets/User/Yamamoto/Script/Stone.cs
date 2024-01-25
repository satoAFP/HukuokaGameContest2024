using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2D��ێ�����ϐ�

    private float speed = 0;//���΂̑��x

    private float acc = 0;//�����x

    // Start is called before the first frame update
    void Start()
    {
        //�����ŗ��΂̑��x�Ɖ����x�����̒l���烉���_���őI��
        acc = Random.Range(0.2f, 1.0f);
        speed = Random.Range(2.0f, 5.0f);

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
            rb.constraints = RigidbodyConstraints2D.None;//FreezePosition����������

            transform.Translate(Vector3.down * (speed + acc * Time.time) * Time.deltaTime);//���΂̈ړ�����
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
