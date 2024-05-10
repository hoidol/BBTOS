using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CatBattleOption : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject topInfo;
    public Image autoCheckImage;
    public TMP_Text autoCheckText;
    
    public void SetDialouge(string txt)
    {
        dialogueText.text = txt;
    }
}
