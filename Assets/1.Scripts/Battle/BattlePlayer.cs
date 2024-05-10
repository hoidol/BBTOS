using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
public class BattlePlayer : MonoSingleton<BattlePlayer>, IBattleUnit
{
    public GameObject playerPanel;
    public DialogueBubble bubble;
    public GameObject optionPanel;
    public PlayerBattleOption[] battleOptions;

    public Transform DiceTargetPointTr => PlayerBattleOption.selectedOption.transform;
    public Transform EndTargetPointTr => null;

    public GameObject endTurnButton;

    public GameObject diceGroupObject;
    //public Transform diceRollingPoint;
    public Transform addDiceRollingPoint;

    public string startBattleDialogue;
    public RoundBehaviourInfo[] roundBehaviourInfos;
    public SkillApplyInfo[] skillApplyInfos;
    //public string endBattleDialogue;

    IBattleUnit targetUnit;
    public override void Awake()
    {
        //battleOptions = FindObjectsOfType<PlayerBattleOption>();
    }
    public  void InitUnit()
    {
        endTurnButton.SetActive(false);
        playerPanel.SetActive(false);
        bubble.gameObject.SetActive(false);
        optionPanel.SetActive(false);

        diceGroupObject.SetActive(false);
    }

    public void StartBattle()
    {
        playerPanel.SetActive(true);
        bubble.gameObject.SetActive(true);
        bubble.dialogueText.text = startBattleDialogue;
    }

    public void EndBattle()
    {
        //bubble.gameObject.SetActive(true);
        //bubble.dialogueText.text = endBattleDialogue;
    }

    public  void StartRound(int round)
    {
        optionPanel.SetActive(false);
        for (int i =0;i< roundBehaviourInfos[round].roundSelectInfos.Length; i++)
        {
            battleOptions[i].SetSelectOptionInfo(roundBehaviourInfos[round].roundSelectInfos[i]);
        }
        for (int i = 0; i < battleOptions.Length; i++)
        {
            battleOptions[i].StartRound();
        }

        
    }

    public void StartSelect(int round)
    {
        bubble.gameObject.SetActive(false);
        optionPanel.SetActive(true);
        for(int i=0;i< battleOptions.Length; i++)
        {
            battleOptions[i].gameObject.SetActive(true);
            battleOptions[i].StartSelect(round);
        }
    }
    public void UpdatePointer(int round)
    {
       
    }
    public  void Roll(int round)
    {
        targetUnit = roundBehaviourInfos[round].roundSelectInfos[PlayerBattleOption.selectedOption.idx].target.GetComponent<IBattleUnit>();

        diceGroupObject.SetActive(true);
        if (BattleCat.Instance.isBonus)
        {
            addDiceRollingPoint.gameObject.SetActive(true);
        }
        else
        {
            addDiceRollingPoint.gameObject.SetActive(false);
        }
        //targetUnit = 
        StartCoroutine(CoRoll(round));
    }
    List<BattleDice> curDices = new List<BattleDice>();
    SelectOptionInfo curOptionInfo;
    IEnumerator CoRoll(int round)
    {
        yield return null;
        curDices.Clear();

        curOptionInfo = roundBehaviourInfos[round].roundSelectInfos[PlayerBattleOption.selectedOption.idx];

        
        if (BattleCat.Instance.isBonus)
        {
            BattleDice dice = BattleDice.Instantiate(PlayerBattleOption.selectedOption.curSkill.diceType);
            curDices.Add(dice);
        }

        for (int i = 0; i < PlayerBattleOption.selectedOption.curSkill.diceCount; i++)
        {
            BattleDice dice = BattleDice.Instantiate(PlayerBattleOption.selectedOption.curSkill.diceType);
            curDices.Add(dice);
        }

        for (int i = 0; i < curDices.Count; i++)
        {
            curDices[i].transform.position = (Vector2)PlayerBattleOption.selectedOption.dicePoint.position + Vector2.one * i * 0.5f;
        }
    }
    public virtual void ApplySkill(int round, Action endEffect)
    {
        StartCoroutine(CoApplySkill(round, endEffect));
    }

    IEnumerator CoApplySkill(int round, Action endEffect)
    {
        int[] nums = new int[curDices.Count];
        for(int i = 0; i < nums.Length; i++)
        {
            nums[i] = curDices[i].curNumber;
        }

        for (int i = 0; i < curDices.Count; i++)
        {
            curDices[i].MoveTo(targetUnit.DiceTargetPointTr.position, true, (dice) =>
            {
                
            });
        }
        yield return new WaitForSeconds(1);
        PlayerBattleOption.selectedOption.curSkill.ExcuteSkill(targetUnit, nums);


        BattleEnemy enemy = curOptionInfo.target.GetComponent<BattleEnemy>();


        enemy.ChangeHp(skillApplyInfos[round].damage);
        enemy.ChangeHp(skillApplyInfos[round].heal);

        endEffect.Invoke();
    }

    public void EndSelect(int round)
    {
        for (int i = 0; i < battleOptions.Length; i++)
        {
            battleOptions[i].pointer.gameObject.SetActive(false);
            battleOptions[i].focusOutlineAnimator.gameObject.SetActive(false);
            if (PlayerBattleOption.selectedOption != battleOptions[i])
            {
                battleOptions[i].gameObject.SetActive(false);
            }
        }

    }

    public void EndRound(int round)
    {
        PlayerBattleOption.selectedOption.pointer.gameObject.SetActive(false);
        for (int i =0;i< curDices.Count; i++)
        {
            Destroy(curDices[i].gameObject);
        }
        for(int  i=0;i< battleOptions.Length; i++)
        {
            battleOptions[i].EndRound();
        }
        curDices.Clear();

        optionPanel.SetActive(false);
        bubble.gameObject.SetActive(true);
        bubble.dialogueText.text = BattlePlayer.Instance.roundBehaviourInfos[round].roundEndScript;

        PlayerBattleOption.selectedOption = null;
    }


    public void OnClickedEndTurn()
    {
        //플레이어가 뭔가를 선택해야지 완료 누를 수 있음
        //PlayerBattleOption.selectedOption 선택한 옵션
        BattleMgr.Instance.EndTurn();
    }

}

[System.Serializable]
public class RoundBehaviourInfo
{
    public int round;

    public SelectOptionInfo[] roundSelectInfos;
    //public string roundStartScript;
    public string roundEndScript;
}

[System.Serializable]
public class SelectOptionInfo
{
    public string script; //대사
    public BubbleTagType  bubbleTagType;
    public string tagText;
    public GameObject target;

    public Transform startPoint;
    public Transform endPoint;

    public DiceType[] diceTypes;
    public int[] diceNumbers;

    public bool disableChoice;//true 선택 못함

}

[System.Serializable]
public class SkillApplyInfo
{
    public int damage;
    public int heal;
    public int debuff;
    public int releaseDebuff;


}
public enum BubbleTagType
{
    Attack,Heal,Defence
}

public enum RoundType
{
    Intro,
    Battle,
    End
}