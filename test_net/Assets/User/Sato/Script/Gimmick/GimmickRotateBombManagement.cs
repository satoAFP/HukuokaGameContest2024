using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickRotateBombManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("RotateBomb")] private GameObject rotateBomb;

    private GameObject clone = null;//�����������e�i�[�p

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (clone == null) 
        {
            //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                clone = PhotonNetwork.Instantiate("RotateBomb", transform.position, Quaternion.identity);
            }
        }
    }
}
