using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SkillInfoPanel : MonoBehaviour
{
    public int skillNumber;
    public TMP_Text nameText;
    public TMP_Text infoText;
    public Image[] diceImages;

    public void Start()
    {
        Skill skill = SkillMgr.Instance.GetSkill(skillNumber);
        nameText.text = skill.skillName;
        infoText.text = skill.skillInfo;

        if (skill.diceCount > 0)
        {
            for (int i = 0; i < diceImages.Length; i++)
            {
                if (i < skill.diceCount)
                {
                    diceImages[i].sprite = Resources.Load<Sprite>($"Sprites/DiceThum_{skill.diceType}");
                    diceImages[i].gameObject.SetActive(true);
                }
                else
                {
                    diceImages[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < diceImages.Length; i++)
            {
                diceImages[i].gameObject.SetActive(false);
            }
        }
        
        
    }
}
