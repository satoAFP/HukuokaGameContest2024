using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    //�N���A���
    [System.NonSerialized] public int[] clearData;

    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        ManagerAccessor.Instance.saveDataManager = this;

        //�X�e�[�W���ɏ�����
        clearData = new int[ManagerAccessor.Instance.dataManager.StageNum];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�N���A�����X�e�[�W���L��
    public void ClearDataSave(string stage)
    {
        PlayerPrefs.SetInt(stage, 1);
    }

    //�N���A�f�[�^�X�V�p�֐�
    public void ClearDataLoad()
    {
        for (int i = 0; i < clearData.Length; i++) 
        {
            clearData[i] = PlayerPrefs.GetInt("Stage" + i, 0);
        }
    }

    //�N���A�����X�e�[�W������
    public void ClearDataReset()
    {
        PlayerPrefs.DeleteAll();
    }
}
