using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueBubble : MonoBehaviour
{
    public TMP_Text dialogueText;
    public void SetDialouge(string txt)
    {
        dialogueText.text = txt;
    }
}
