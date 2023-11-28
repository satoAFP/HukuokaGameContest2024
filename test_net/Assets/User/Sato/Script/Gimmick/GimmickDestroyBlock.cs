using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickDestroyBlock : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�̑��x")] private int FeedSpeed;

    [System.NonSerialized] public bool DestroyStart = false;//���Œ�

    // Update is called once per frame
    void FixedUpdate()
    {
        //�t�F�[�h�������蔻�����
        if(DestroyStart)
        {
            GetComponent<BoxCollider2D>().enabled = false;

            GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
        }
        //�����ɂȂ����i�K�ō폜
        if (GetComponent<SpriteRenderer>().color.a <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
