using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    //public Image bgImage;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image barImage;

    //public GameObject checkPanel;
    public Image checkImage;
    public TMP_Text checkText;

    public void SetData(Dialogue dialogue, CharacterData cData)
    {
        nameText.text = cData.name;
        nameText.color = cData.color;
        barImage.color = cData.color;
        dialogueText.text = dialogue.script;
        checkImage.gameObject.SetActive(false);
        checkText.gameObject.SetActive(false);
        if (dialogue.type == DialogueType.Check)
        {
            checkImage.gameObject.SetActive(true);
            checkText.gameObject.SetActive(true);
            if (dialogue.check)
            {
                checkImage.sprite = Resources.Load<Sprite>("Sprites/AutoCheck_Suc");
            }
            else
            {
                checkImage.sprite = Resources.Load<Sprite>("Sprites/AutoCheck_Fail");

            }

            checkText.text = dialogue.mindScript;
        }

    }
}
