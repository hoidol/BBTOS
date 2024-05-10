using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractResultDialoguePanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text diceResultText;
    public TMP_Text[] resultTexts;
    public GameObject[] lockPanels;
    public Image barImage;
    public void SetData(Dialogue dialogue, CharacterData cData)
    {
        nameText.text = cData.name;
        nameText.color = cData.color;
        barImage.color = cData.color;
        diceResultText.text = dialogue.script;
        for(int i = 0; i < 3; i++)
        {
            resultTexts[i].text = dialogue.interactResultDatas[i].script;
            lockPanels[i].SetActive(dialogue.interactResultDatas[i].locked);
        }
    }
}
