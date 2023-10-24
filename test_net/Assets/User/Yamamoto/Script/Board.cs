using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Board : MonoBehaviourPunCallbacks
{

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    // 2軸入力を受け取るAction
    [SerializeField] private InputActionProperty _moveAction;

    //移動方向入れる変数
    private Collider2D collider;//板のコライダー

    //inputsystemをスクリプトで呼び出す
    private BoardInput boardinput;

    //移動を止める
    private bool movelock = false;

    private void OnDestroy()
    {
        _moveAction.action.Dispose();
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
 
        collider = this.GetComponent<BoxCollider2D>();
        boardinput = new BoardInput();//スクリプトを変数に格納

        collider.isTrigger = true;//コライダーのトリガー化

    }

    // Update is called once per frame
    void Update()
    {
        //データマネージャー取得
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        // 2軸入力読み込み
        var inputValue = _moveAction.action.ReadValue<Vector2>();

        if(!movelock)
        {
            // xy軸方向で移動
            transform.Translate(inputValue * (moveSpeed * Time.deltaTime));
        }
       

        if (datamanager.isOwnerInputKey_CB)
        {
            movelock = true;
            collider.isTrigger = false;//トリガー化解除
        }

    }
}
