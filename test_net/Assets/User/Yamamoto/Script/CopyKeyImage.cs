using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyKeyImage : MonoBehaviour
{
    [SerializeField, Header("�ʏ펞�̃R�s�[�L�[")]
    private Sprite CKeyImage;

    [SerializeField, Header("�^�����̃R�s�[�L�[")]
    private Sprite CKeyLiftImage;

   [SerializeField, Header("���S���̃R�s�[�L�[")]
    private Sprite CKeyDeathImage;

    CopyKey copykey;

    



    // Start is called before the first frame update
    void Start()
    {
        copykey = transform.parent.GetComponent<CopyKey>();//CopyKey�X�N���v�g���擾

        GetComponent<SpriteRenderer>().sprite = CKeyImage;//�摜�̏�����
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(copykey.copykey_death)
        {
            GetComponent<SpriteRenderer>().sprite = CKeyDeathImage;//�R�s�[�L�[�̎��S���摜
        }
        else
        {
            //�R�s�[�L�[�̈ړ����������ɉ����ăv���C���[�̌�����ς���
            if (ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().imageleft)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            if (ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().changeliftimage)
            {
                GetComponent<SpriteRenderer>().sprite = CKeyLiftImage;//�����グ���̉摜�ɕς���
            }
            else if(ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().standardCopyKeyImage)
            {
                GetComponent<SpriteRenderer>().sprite = CKeyImage;//�摜�����ɖ߂�
            }



        }
    }
}
