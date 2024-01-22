using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGetHitObjTag : MonoBehaviourPunCallbacks
{
    [System.NonSerialized] public List<string> HitTags = new List<string>();

    [SerializeField, Header("���[�J�����W")] private Vector3 localPos;

    private void Update()
    {
        //�f�[�^�}�l�[�W���[�擾
        DataManager dataManager = ManagerAccessor.Instance.dataManager;

        //���W��e�I�u�W�F�N�g�ɍ��킹��
        transform.position = transform.parent.transform.position + localPos;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor" || collision.tag == "Gimmick")
        {
            //�G�ꂽ�I�u�W�F�N�g�̃^�O�L��
            HitTags.Add(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�o���I�u�W�F�N�g�̃^�O������
        for (int i = 0; i < HitTags.Count; i++)
        {
            if (HitTags[i] == collision.tag)
            {
                HitTags.RemoveAt(i);
            }
        }
    }
}
