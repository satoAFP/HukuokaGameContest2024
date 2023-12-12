using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSystem : MonoBehaviour
{
    const int NONE = 9999;      //�f�[�^�������Ă��Ȃ�
    const int startTime = 5;    //����������^�C�~���O

    [SerializeField, Header("��")] private GameObject[] door;

    [SerializeField, Header("�K�i")] private GameObject[] stairs;

    [SerializeField, Header("�t�F�[�h���x")] private int feedSpeed;

    private List<GameObject> stairChildren;//�K�i�̎q�I�u�W�F�N�g�擾�p

    private int memFeedStairs = NONE;   //�N���A�����ẴX�e�[�W��

    private int count = 0;              //�t���[���J�E���g


    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        //���[�h�̌��ˍ����タ����Ɠǂݍ��݂����炷
        if (count == startTime) 
        {
            //�N���A�f�[�^���[�h
            ManagerAccessor.Instance.saveDataManager.ClearDataLoad();
            ManagerAccessor.Instance.saveDataManager.FirstClearDataLoad();

            for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++)
            {
                if (ManagerAccessor.Instance.saveDataManager.clearData[i] == 1)
                {
                    //�N���A���Ă���X�e�[�W�͋��F�ɂ���
                    door[i].GetComponent<SpriteRenderer>().color = new Color32(255, 238, 186, 255);

                    //�N���A���Ă���X�e�[�W�̊K�i���o��
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
