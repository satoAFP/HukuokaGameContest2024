using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StageNameSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ステージ数抽出
        int stageNum = int.Parse(ManagerAccessor.Instance.sceneMoveManager.GetSceneName().Substring(5, 1));
        //ステージ名表示
        GetComponent<Text>().text = "第" + (9 - stageNum) + "層";
    }
}
