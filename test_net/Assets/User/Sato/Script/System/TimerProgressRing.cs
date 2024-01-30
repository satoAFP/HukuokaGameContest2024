using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerProgressRing : MonoBehaviour
{
    private Image circle;



    private void Start()
    {
        //�f�[�^�擾
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

            //�\���L�[���������Ă���Ƃ��̂ݕ\��
            if (dataManager.isOwnerInputKey_C_D_DOWN)
            {
                //�R�s�[�����o�����Ă���Ƃ��̓R�s�[�����Q��
                if ((dataManager.copyKey != null && dataManager.board != null) || dataManager.copyKey != null)
                {
                    circle.fillAmount = 1.0f / 30.0f * dataManager.copyKey.GetComponent<CopyKey>().holdtime;
                }
                //�{�[�h�����̎��̓{�[�h���Q��
                else if (dataManager.board != null)
                {
                    circle.fillAmount = 1.0f / 30.0f * dataManager.board.GetComponent<Board>().holdtime;
                }
            }
        }
    }
}