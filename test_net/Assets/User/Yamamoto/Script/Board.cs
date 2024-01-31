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

    private Collider2D collider;//板のコライダー

    private Rigidbody2D rigid;//リジッドボディ

    public int pushnum = 0;//ボタンを押した回数

    //inputsystemをスクリプトで呼び出す
    private BoardInput boardinput;

    //移動を止める
    private bool movelock = false;
    //ボタンの複数回入力を防ぐ
    private bool pushbutton = false;

    [SerializeField, Header("アイテム回収時間（大体60で１秒）")]
    private int collecttime;

    [SerializeField]
    public int holdtime;//設定したアイテム回収時間を代入する

    private bool firstcorsor = false;//カーソルを移動させるたびにボタンの表示を変える

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

        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        boardinput = new BoardInput();//スクリプトを変数に格納

        collider.isTrigger = true;//コライダーのトリガー化

        holdtime = collecttime;//長押しカウント時間を初期化

        //全体からコピー鍵取得
        ManagerAccessor.Instance.dataManager.board = gameObject;

        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = true;//カーソル移動ロック

        //画像を半透明にする
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 128);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //データマネージャー取得
        DataManager datamanager = ManagerAccessor.Instance.dataManager;
        
        //プレイヤーがゲームオーバーになっていなければ板の基本操作許可
        if(!ManagerAccessor.Instance.dataManager.isDeth)
        {
            //現在カーソルが板を選んでいる時
            if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "Board")
            {
                //プレイヤー1側（箱）でしか操作できない
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    if (firstcorsor)
                    {
                        //板を設置した時に下ボタンの吹き出し表示
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowDown;

                        firstcorsor = false;
                    }

                    // 2軸入力読み込み
                    var inputValue = _moveAction.action.ReadValue<Vector2>();

                    if (!movelock)
                    {
                        // xy軸方向で移動
                        transform.Translate(inputValue * (moveSpeed * Time.deltaTime));
                    }


                    //ゲームパッドの右ボタンを押したとき
                    if (datamanager.isOwnerInputKey_CB)
                    {
                       

                        if (!pushbutton)
                        {
                            pushbutton = true;
                            pushnum++;
                        }

                    }
                    else
                    {
                        pushbutton = false;
                    }

                    //pushnumが2なのは板生成時に右ボタンが押された状態のため初期値が1になっている
                    if (pushnum == 2)
                    {
                        Debug.Log("Set");
                        photonView.RPC(nameof(Rpc_SetBoard), RpcTarget.All);

                        //板を設置した時に下ボタンの吹き出し表示
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowDown;


                    }


                }


                //ゲームパッド下ボタンで置きなおし
                if (datamanager.isOwnerInputKey_CA)
                {
                    //holdtime--;//長押しカウントダウン

                    //箱側でしか吹き出しを表示させない
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        //板を置きなおしたとき右ボタンの吹き出し表示
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
                    }

                    //画像を半透明にする
                    GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 128);

                    movelock = false;
                    collider.isTrigger = true;//トリガー化
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = true;//鍵の生成を止める
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = true;//カーソル移動を止める
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = true;//鍵の移動させない


                }


            }
            else
            {
                firstcorsor = true;
            }

            //十字キー下でアイテム回収
            if (datamanager.isOwnerInputKey_C_D_DOWN)
            {
                holdtime--;//長押しカウントダウン

                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = true;//鍵の生成を止める
                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = true;//カーソル移動を止める
                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = true;//鍵の移動させない

                //ゲームパッド下ボタン長押しで回収
                if (holdtime <= 0)//回収カウントが0になると回収
                {
                    DeleteBoard();
                }

            }
            else
            {
                holdtime = collecttime;//長押しカウントリセット
            }
        }
        else
        {
            collider.isTrigger = true;//念のためにトリガー化
        }
        

    }


    //この関数は通信用
    [PunRPC]
    private void Rpc_SetBoard()
    {
        //画像の透明度を元に戻す
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

        movelock = true;
        collider.isTrigger = false;//トリガー化解除
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = false;//鍵生成許可
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = false;//カーソル移動許可
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = false;//鍵の移動可能
        pushnum = 1;
    }

    //板を削除
    private void DeleteBoard()
    {
      //  ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().boxopen = true;//箱を開ける
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = false;//鍵生成許可
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = false;//カーソル移動許可
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = false;//鍵の移動可能
        Destroy(gameObject);
    }
}
