using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class TestInput : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputAction m_inputmover;//インプットアクション
    //入力された方向を入れる変数
    private Vector2 inputDirection;
    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    //playerinputが必要な時に呼び出す
    private void OnEnable()
    {
        m_inputmover.Enable();
    }
    private void OnDisable()
    {
        m_inputmover.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        //名前とIDを設定
        gameObject.name = "Player" + photonView.OwnerActorNr;

        // デバイス一覧を取得
        foreach (var device in InputSystem.devices)
        {
            // デバイス名をログ出力
            Debug.Log(device.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
             inputDirection = m_inputmover.ReadValue<Vector2>();
             transform.Translate
        (
            inputDirection.x * moveSpeed,
            inputDirection.y * moveSpeed,
            0.0f);
        }
       

    }
}
