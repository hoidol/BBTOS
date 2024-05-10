using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueOption : MonoBehaviour
{
    public TMP_Text optionText;
    public Color color;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedBtn);
        optionText = GetComponentInChildren<TMP_Text>();
    }
    public void SetDialogue(DialogueOptionData data)
    {
        optionText.text = data.script;
        if (data.disable)
        {
            optionText.color = Color.gray;
        }
        else
        {
            optionText.color = color;
        }
        
    }

    void OnClickedBtn()
    {
        DialogueMgr.Instance.OnClickedNextBtn();
    }
}
