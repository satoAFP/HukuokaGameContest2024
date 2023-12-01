using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerate : MonoBehaviour
{
    [SerializeField] private GameObject GenelateErea_L;//�����ꏊ��
    [SerializeField] private GameObject GenelateErea_R;//�����ꏊ�E


    [SerializeField] private GameObject[] Stones = new GameObject[3];//�傫����

    private int stonetype = 0;//���������̎�ނ������_���Ɍ��߂�

    private float height;

    [SerializeField, Header("���b�Ԃ��ɐ������邩�ݒ�ł���")] private float spawnInterval; // �����Ԋu

    [SerializeField, Header("spawnInterval�Őݒ肵���b���Ƃɉ��������邩�ݒ�ł���")] private int spawnstone;      // ���ɐ��������̐�

    private float timer = 0f;//���Ԃ��J�E���g

    private Vector2 spawnPosition;//�␶���ꏊ

    // Start is called before the first frame update
    void Start()
    {
        height = gameObject.GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        //�����Ŋ�𐶐����鎞�Ԃ��v��
        if (timer >= spawnInterval)
        {
            for(int i=0;i < spawnstone; i++)
            {
                SpawnObject();
            }
           
            timer = 0f;
        }
    }

    public void SpawnObject()
    {

        stonetype = Random.Range(1, 3);//��̎�ނ������_���I�o

        float randomX = Random.Range(GenelateErea_L.transform.localPosition.x, GenelateErea_R.transform.localPosition.x);//�����_����X���W

        spawnPosition = new Vector2(randomX, height);//�����ʒu��ݒ�

        var worldPoint = transform.TransformPoint(spawnPosition);//���[���h���W�ɕϊ�

        worldPoint.y = height;//���������[���h���W�ɂ���Ƃ����̂Ń��[�J���ɖ߂�

       // Debug.Log(spawnPosition);

        // Debug.Log("���[���h�Ԋ�"+worldPoint);

        Instantiate(Stones[stonetype], worldPoint, Quaternion.identity);
    }
}
