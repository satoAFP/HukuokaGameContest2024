using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
