using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    //クリア情報
    [System.NonSerialized] public int[] clearData;
    [System.NonSerialized] public int[] firstClearData;

    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        ManagerAccessor.Instance.saveDataManager = this;

        clearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
        firstClearData = new int[ManagerAccessor.Instance.dataManager.StageNum];

        //ステージ数に初期化
        clearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
        firstClearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
    }

    //クリアしたステージを記憶
    public void ClearDataSave(string stage)
    {
        PlayerPrefs.SetInt(stage, 1);
    }

    //初クリアしたステージを記憶
    public void FirstClearDataSave(string stage)
    {
        PlayerPrefs.SetInt(stage, 1);
    }

    //クリアデータ更新用関数
    public void ClearDataLoad()
    {
        for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++) 
        {
            clearData[i] = PlayerPrefs.GetInt("Stage" + (i + 1), 0);
            Debug.Log(PlayerPrefs.GetInt("Stage" + (i + 1), 0).ToString());
        }
    }

    //初クリアデータ更新用関数
    public void FirstClearDataLoad()
    {
        for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++)
        {
            firstClearData[i] = PlayerPrefs.GetInt("first" + (i + 1), 0);
        }
    }

    //クリアしたステージを消去
    public void ClearDataReset()
    {
        PlayerPrefs.DeleteAll();
    }
}
