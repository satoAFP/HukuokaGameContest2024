using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    //�N���A���
    [System.NonSerialized] public int[] clearData;
    [System.NonSerialized] public int[] firstClearData;

    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        ManagerAccessor.Instance.saveDataManager = this;

        clearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
        firstClearData = new int[ManagerAccessor.Instance.dataManager.StageNum];

        //�X�e�[�W���ɏ�����
        clearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
        firstClearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
    }

    //�N���A�����X�e�[�W���L��
    public void ClearDataSave(string stage)
    {
        PlayerPrefs.SetInt(stage, 1);
    }

    //���N���A�����X�e�[�W���L��
    public void FirstClearDataSave(string stage)
    {
        PlayerPrefs.SetInt(stage, 1);
    }

    //�N���A�f�[�^�X�V�p�֐�
    public void ClearDataLoad()
    {
        for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++) 
        {
            clearData[i] = PlayerPrefs.GetInt("Stage" + (i + 1), 0);
            Debug.Log(PlayerPrefs.GetInt("Stage" + (i + 1), 0).ToString());
        }
    }

    //���N���A�f�[�^�X�V�p�֐�
    public void FirstClearDataLoad()
    {
        for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++)
        {
            firstClearData[i] = PlayerPrefs.GetInt("first" + (i + 1), 0);
        }
    }

    //�N���A�����X�e�[�W������
    public void ClearDataReset()
    {
        PlayerPrefs.DeleteAll();
    }
}
