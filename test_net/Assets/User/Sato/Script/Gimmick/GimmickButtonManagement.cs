using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButtonManagement : CGimmick
{
    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;

        for (int i = 0; i < gimmickButton.Count; i++)
        {
            if (gimmickButton[0].GetComponent<GimmickButton>().isButton == true)
            {
                count++;
            }
        }

        if (gimmickButton.Count == count) 
        {
            door.SetActive(false);
        }

    }
}
