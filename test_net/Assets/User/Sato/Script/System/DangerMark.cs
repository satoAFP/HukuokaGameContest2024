using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DangerMark : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("����")] private Transform stoneGenelate;

    [SerializeField, Header("�\�����鋗��")] private float displayPos;


    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

}
