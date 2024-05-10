using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MindDialgouePanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image barImage;

    Dialogue dialogue;
    public void SetData(Dialogue dialogue, CharacterData cData)
    {
        this.dialogue = dialogue;
        nameText.text = cData.name;
        nameText.color = cData.color;
        barImage.color = cData.color;
        dialogueText.text = dialogue.script;

    }

    public void OpenMind()
    {
        dialogueText.text = dialogue.mindScript;
    }
}
