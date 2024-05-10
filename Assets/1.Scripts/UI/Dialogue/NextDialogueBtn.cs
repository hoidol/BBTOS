using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDialogueBtn : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedBtn);
    }
 

    void OnClickedBtn()
    {
        DialogueMgr.Instance.OnClickedNextBtn();
    }
}
