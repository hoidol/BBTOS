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
    public TMP_Text mindTitleText;
    public void SetData(Dialogue dialogue, CharacterData cData)
    {
        this.dialogue = dialogue;
        nameText.text = cData.name;
        nameText.color = cData.color;
        barImage.color = cData.color;
        dialogueText.text = dialogue.script;
        mindTitleText.color = cData.color;
        mindTitleText.gameObject.SetActive(false);

    }
    public Color mindColor;
    public void OpenMind()
    {
        mindTitleText.gameObject.SetActive(true);
        dialogueText.text = dialogue.mindScript;
        dialogueText.color = mindColor;
    }
}
