using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSystem : MonoBehaviour
{
    const int NONE = 9999;

    [SerializeField, Header("階段")] private GameObject[] stairs;

    [SerializeField, Header("フェード速度")] private int feedSpeed;

    private int memFeedStairs = NONE;//クリアしたてのステージ数

    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.saveDataManager.ClearDataLoad();
        ManagerAccessor.Instance.saveDataManager.FirstClearDataLoad();

        for (int i = 0; i < ManagerAccessor.Instance.saveDataManager.clearData.Length; i++) 
        {
            if (ManagerAccessor.Instance.saveDataManager.clearData[i] == 1) 
            {
                if (ManagerAccessor.Instance.saveDataManager.firstClearData[i] == 0)
                {
                    stairs[i].SetActive(true);
                    stairs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    ManagerAccessor.Instance.saveDataManager.FirstClearDataSave("first" + i);
                    memFeedStairs = i;
                }
                else
                {
                    stairs[i].SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (memFeedStairs != NONE) 
        {
            if (stairs[memFeedStairs].GetComponent<SpriteRenderer>().color.a < 255) 
            {
                stairs[memFeedStairs].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, (byte)feedSpeed);
            }
            else
            {
                memFeedStairs = NONE;
            }
        }
    }
}
