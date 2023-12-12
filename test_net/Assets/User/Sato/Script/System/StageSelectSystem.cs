using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSystem : MonoBehaviour
{
    const int NONE = 9999;      //データが入っていない
    const int startTime = 5;    //初期化するタイミング

    [SerializeField, Header("扉")] private GameObject[] door;

    [SerializeField, Header("階段")] private GameObject[] stairs;

    [SerializeField, Header("フェード速度")] private int feedSpeed;

    private List<GameObject> stairChildren;//階段の子オブジェクト取得用

    private int memFeedStairs = NONE;   //クリアしたてのステージ数

    private int count = 0;              //フレームカウント


    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        //ロードの兼ね合い上ちょっと読み込みをずらす
        if (count == startTime) 
        {
            //クリアデータロード
            ManagerAccessor.Instance.saveDataManager.ClearDataLoad();
            ManagerAccessor.Instance.saveDataManager.FirstClearDataLoad();

            for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++)
            {
                if (ManagerAccessor.Instance.saveDataManager.clearData[i] == 1)
                {
                    //クリアしているステージは金色にする
                    door[i].GetComponent<SpriteRenderer>().color = new Color32(255, 238, 186, 255);

                    //クリアしているステージの階段を出す
                    if (ManagerAccessor.Instance.saveDataManager.firstClearData[i] == 0)
                    {
                        stairs[i].SetActive(true);
                        stairs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                        for (int j = 0; j < stairs[i].transform.childCount; j++)
                        {
                            stairs[i].transform.GetChild(j).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                        }

                        ManagerAccessor.Instance.saveDataManager.FirstClearDataSave("first" + (i + 1));
                        memFeedStairs = i;
                    }
                    else
                    {
                        stairs[i].SetActive(true);
                    }
                }
            }
        }



        if (memFeedStairs != NONE)
        {
            if (stairs[memFeedStairs].GetComponent<SpriteRenderer>().color.a < 255)
            {
                stairs[memFeedStairs].GetComponent<SpriteRenderer>().color += new Color32(0, 0, 0, (byte)feedSpeed);

                for (int i = 0; i < stairs[i].transform.childCount; i++) 
                {
                    stairs[memFeedStairs].transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color += new Color32(0, 0, 0, (byte)feedSpeed);
                }
            }
            else
            {
                memFeedStairs = NONE;
            }
        }
    }
}
