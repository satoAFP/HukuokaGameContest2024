using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleteTime : MonoBehaviour
{
    [SerializeField, Header("�G�t�F�N�g��������܂ł̎���")] private int DeleteTime;

    private int count = 0;//�t���[���J�E���g�p

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ݒ莞�Ԃō폜
        count++;
        if (count == DeleteTime) 
        {
            Destroy(gameObject);
        }
    }
}