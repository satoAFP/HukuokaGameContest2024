using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StageNameSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W�����o
        int stageNum = int.Parse(ManagerAccessor.Instance.sceneMoveManager.GetSceneName().Substring(5, 1));
        //�X�e�[�W���\��
        GetComponent<Text>().text = "��" + (9 - stageNum) + "�w";
    }
}
