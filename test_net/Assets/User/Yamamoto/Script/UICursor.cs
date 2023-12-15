using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class UICursor : MonoBehaviourPunCallbacks
{
    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("板のアイコン")]
    private GameObject BoardIcon;

    [SerializeField, Header("コピーキーのアイコン")]
    private GameObject CopyKeyIcon;

    private int LRmove = 0;//1:右　2:左

    private bool movestart = false;//移動中かを判断する

    // 点滅させる対象
    [SerializeField] private Renderer _target;
    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;

    private double _time;

    // Start is called before the first frame update
    void Start()
    {
        test_net = new Test_net();//スクリプトを変数に格納
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        // 内部時刻を経過させる
        _time += Time.deltaTime;

        if (ManagerAccessor.Instance.dataManager.player1 != null)
        {
            //箱を開けている時カーソル移動をする
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
            {
                // 周期cycleで繰り返す値の取得
                // 0～cycleの範囲の値が得られる
                var repeatValue = Mathf.Repeat((float)_time, _cycle);

                // 内部時刻timeにおける明滅状態を反映
                _target.enabled = repeatValue >= _cycle * 0.5f;

                //押されたボタンの左右でカーソルの移動位置を決める
                if (datamanager.isOwnerInputKey_C_D_RIGHT && !movestart)
                {
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "None";
                    movestart = true;
                    LRmove = 1;//右にカーソル移動
                }

                if (datamanager.isOwnerInputKey_C_D_LEFT && !movestart)
                {
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "None";
                    movestart = true;
                    LRmove = 2;//左にカーソル移動
                }
            }
        }

        if(movestart)
        {
            if(LRmove==1)
            {
                transform.position = Vector2.MoveTowards(transform.position, CopyKeyIcon.transform.position, moveSpeed * Time.deltaTime);
                if (transform.position == CopyKeyIcon.transform.position)
                {
                    movestart = false;
                    LRmove = 0;//移動終了
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "CopyKey";//現在カーソルが選択しているアイテム名
                }
            }
            else if(LRmove==2)
            {
                transform.position = Vector2.MoveTowards(transform.position, BoardIcon.transform.position, moveSpeed * Time.deltaTime);
                if (transform.position == BoardIcon.transform.position)
                {
                    movestart = false;
                    LRmove = 0;//移動終了
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "Board";//現在カーソルが選択しているアイテム名
                }
            }
            
        }
    }
}
