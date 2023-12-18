using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{


    private bool isMenuOpen = false;    //���j���[���J���Ă��邩�ǂ���

    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ManagerAccessor.Instance.dataManager.isOwnerInputKeyPause)
        {
            if (first)
            {
                isMenuOpen = !isMenuOpen;
                first = false;
            }
        }
        else
            first = true;
    }
}
