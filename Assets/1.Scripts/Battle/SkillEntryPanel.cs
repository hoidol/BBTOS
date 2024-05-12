using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillEntryPanel : MonoBehaviour
{
    public string skillName;
    public string skillNumber;
    [HideInInspector] public Image skillImage;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonBtn);   
        skillImage = transform.Find("SkillImage").GetComponent<Image>();
        skillImage.sprite = SkillMgr.Instance.skillDic[skillName].sprite;
    }
    
    public void OnButtonBtn()
    {
        GetComponentInParent<PlayerBattleOption>().ChangeSkill(SkillMgr.Instance.skillDic[skillName]);
    }
}