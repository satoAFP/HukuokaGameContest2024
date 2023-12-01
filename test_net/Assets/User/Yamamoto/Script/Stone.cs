using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2D��ێ�����ϐ�

    private float gravity = 0;//�d�͂������_���ɐݒ�

    // Start is called before the first frame update
    void Start()
    {
        gravity = Random.Range(0.1f, 1.0f);

        // �Q�[���I�u�W�F�N�g�ɃA�^�b�`���ꂽRigidbody2D�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>();

        //�d�̓X�P�[����ύX����
        rb.gravityScale = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        
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
