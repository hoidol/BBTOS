using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class PlayerBattleOption : BattleOption, IPointerEnterHandler, IPointerExitHandler
{
    public int idx;
    public static PlayerBattleOption focusOption;
    public static PlayerBattleOption selectedOption;
    
    public GameObject skillListPanel;
    public SkillEntryPanel[] skillEntryPanels;

    public int skillNumber; //디폴트 스킬
    [HideInInspector] public Skill curSkill;

    public Transform dicePoint;

    public Animator focusOutlineAnimator;


    public TMP_Text scriptText;
    public Image bubbleTagImage;
    public TMP_Text bubbleTagText;

    public Image curSkillImage;

    public ArrowPointer pointer;
    public Color pointColor;
    public DiceType diceType;
    public int diceThrowCount;
    public Image[] skillDiceImgs;
    public SkillInfoPopUp skillInfoPopUp;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedBtn);
        focusOutlineAnimator.gameObject.SetActive(false);
        skillListPanel.gameObject.SetActive(false);
    }
    void Start()
    {
           
    }

    public void StartSelect(int round)
    {
        Debug.Log($"StartSelect {round} {gameObject.name}");
        if(pointer == null)
            pointer = ArrowPointer.Instantiate();

        curSkill = SkillMgr.Instance.GetSkill(skillNumber);
        UpdateSkill();

        pointer.gameObject.SetActive(false);
        skillListPanel.gameObject.SetActive(false);
    }

    void UpdateSkill()
    {
        //
        skillInfoPopUp.skillNumber = curSkill.skillNumber;
        skillInfoPopUp.Focus(false);
        for (int i =0;i< skillDiceImgs.Length; i++)
        {
            skillDiceImgs[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < curSkill.diceCount; i++)
        {
            if(i < skillDiceImgs.Length)
            {
                skillDiceImgs[i].sprite = Resources.Load<Sprite>($"Sprites/DiceThum_{curSkill.diceType}");
                skillDiceImgs[i].gameObject.SetActive(true);
            }
        }
        curSkillImage.sprite = curSkill.sprite;


    }
    SelectOptionInfo selectOptionInfo;
    public void SetSelectOptionInfo(SelectOptionInfo info)
    {
        selectOptionInfo = info;
        scriptText.text = info.script;
        bubbleTagText.text = info.tagText;
        bubbleTagImage.sprite = Resources.Load<Sprite>("Sprites/BubbleTag" +info.bubbleTagType);
        if (!info.disableChoice)
        {
            scriptText.color = Color.white;
        }
        else
        {
            scriptText.color = Color.gray;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (PlayerBattleOption.focusOption != null )
        {
            PlayerBattleOption.focusOption.Focus(false);
        }

        PlayerBattleOption.focusOption = this;
        PlayerBattleOption.focusOption.Focus(true);

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    IBattleUnit targetUnit;
    SelectOptionInfo curSelectOptionInfo;
    public void Focus(bool b)
    {
        

        if (b)
        {
            Debug.Log("포커스 잡기" +idx);
            focusOutlineAnimator.gameObject.SetActive(true);

            focusOutlineAnimator.Play("Focused");
            skillListPanel.SetActive(true);
            curSelectOptionInfo = BattlePlayer.Instance.roundBehaviourInfos[BattleMgr.Instance.curRound].roundSelectInfos[idx];
            targetUnit = curSelectOptionInfo.target.GetComponent<IBattleUnit>();

            pointer.gameObject.SetActive(true);
        }
        else
        {
            if (PlayerBattleOption.selectedOption != null)
            {
                if(PlayerBattleOption.selectedOption == this)
                    return;
            }
                
            pointer.gameObject.SetActive(false);
            focusOutlineAnimator.gameObject.SetActive(false);
            skillListPanel.SetActive(false);
        }
    }
    
    void Update()
    {
        if (pointer == null)
            return;

        if (pointer.gameObject.activeSelf && targetUnit != null)
        {
            pointer.Point(transform, transform.position, curSelectOptionInfo.endPoint.position, pointColor);
        }
    }

    void OnClickedBtn()
    {
        if (selectOptionInfo.disableChoice)
            return;

        if (PlayerBattleOption.selectedOption != null)
            return;
            
        focusOutlineAnimator.Play("Selected");
        selectedOption = this;

        skillListPanel.SetActive(false);
        PlayerBattleOption.selectedOption = this;
        BattlePlayer.Instance.endTurnButton.SetActive(true);
        Debug.Log("BattlePlayerOption OnClickedBtn");
    }

    public void ChangeSkill(Skill s)
    {
        if (selectOptionInfo.disableChoice)
            return;

        curSkill = s;
        curSkillImage.sprite = s.sprite;
        skillListPanel.SetActive(false);
        UpdateSkill();
    }

    public void StartRound()
    {
        selectedOption = null;
        focusOption = null;
        focusOutlineAnimator.gameObject.SetActive(false);
    }

    public void EndRound()
    {
        focusOutlineAnimator.gameObject.SetActive(false);
    }
    //ArrowPointer pointer;
    public void SetSkillTarget(Skill s)
    {
        curSkill = s;
        
        //for (int i = 0; i < BattleMgr.Instance.enemies.Length; i++)
        //{
        //    if (BattleMgr.Instance.enemies[i].key == s.targetEnemyKey)
        //    {
        //        if (pointer == null)
        //            ArrowPointer.Point(transform, transform.position, BattleMgr.Instance.enemies[i].battleOption.transform.position);
        //    }
        //}
    }
}
