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

    private bool firstLR = true;//���E�ړ���x�����������s��



    // Start is called before the first frame update
    void Start()
    {
        copykey = transform.parent.GetComponent<CopyKey>();//CopyKey�X�N���v�g���擾

        GetComponent<SpriteRenderer>().sprite = CKeyImage;//�摜�̏�����
    }

    // Update is called once per frame
    void Update()
    {
        if(copykey.copykey_death)
        {
            GetComponent<SpriteRenderer>().sprite = CKeyDeathImage;//�R�s�[�L�[�̎��S���摜
        }
    }
}
