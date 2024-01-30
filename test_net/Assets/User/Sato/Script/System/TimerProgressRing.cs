using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerProgressRing : MonoBehaviour
{
    private Image circle;



    private void Start()
    {
        //データ取得
        circle = gameObject.GetComponent<Image>();
        circle.fillAmount = 0;
    }

    private void FixedUpdate()
    {
        if (!ManagerAccessor.Instance.dataManager.isClear &&
            !ManagerAccessor.Instance.dataManager.isDeth &&
            !ManagerAccessor.Instance.dataManager.isPause)
        {
            DataManager dataManager = ManagerAccessor.Instance.dataManager;
            circle.fillAmount = 0;

            //十字キー下を押しているときのみ表示
            if (dataManager.isOwnerInputKey_C_D_DOWN)
            {
                //コピー鍵が出現しているときはコピー鍵を参照
                if ((dataManager.copyKey != null && dataManager.board != null) || dataManager.copyKey != null)
                {
                    circle.fillAmount = 1.0f / 30.0f * dataManager.copyKey.GetComponent<CopyKey>().holdtime;
                }
                //ボードだけの時はボードを参照
                else if (dataManager.board != null)
                {
                    circle.fillAmount = 1.0f / 30.0f * dataManager.board.GetComponent<Board>().holdtime;
                }
            }
        }
    }
}