using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EnemyBattleOption : BattleOption
{
    public GameObject bottomInfoObject;
    public TMP_Text dialogueText;

    SelectOptionInfo selectOptionInfo;
    public void SetSelectOptionInfo(SelectOptionInfo info)
    {
        selectOptionInfo = info;
        dialogueText.text = info.script;
       
    }
    public void SetDialogue(string txt)
    {
        dialogueText.text = txt;
        bottomInfoObject.gameObject.SetActive(false);
    }
}
