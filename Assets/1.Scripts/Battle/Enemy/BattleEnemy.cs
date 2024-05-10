using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleEnemy : MonoBehaviour, IBattleUnit
{
    public string key;
    public float maxHp;
    public float curHp;
    public UnitType unitType;
    public EnemyBattleOption battleOption;
    public Transform DiceTargetPointTr => battleOption.transform;
    public Transform EndTargetPointTr => battleOption.endPointTr.transform;

    public string[] skillNames;
    [HideInInspector] public Skill[] skills;
    public Image hpImg;
    public Transform dicePoint;


    public string startBattleDialogue;
    public RoundBehaviourInfo[] roundBehaviourInfos;

    public SkillApplyInfo[] skillApplyInfos;
    //public string endBattleDialogue;

    ArrowPointer pointer;
    IBattleUnit target;
    public BattleEnemy targetEnemy;
    public Color pointerColor;
    public virtual void InitUnit()
    {
        if(pointer== null)
            pointer = ArrowPointer.Instantiate();

        pointer.gameObject.SetActive(false);
        if(skills == null)
        {
            skills = new Skill[skillNames.Length];
            for (int i = 0; i < skillNames.Length; i++)
            {
                skills[i] = SkillMgr.Instance.skillDic[skillNames[i]];
            }
        }
       
        battleOption.gameObject.SetActive(false);
        hpImg.fillAmount = 1;
    }

    public virtual void StartBattle()
    {
        battleOption.gameObject.SetActive(true);

        battleOption.bottomInfoObject.gameObject.SetActive(false);
        battleOption.dialogueText.text = startBattleDialogue;
    }
    public virtual void EndBattle()
    {
        battleOption.bottomInfoObject.gameObject.SetActive(false);
        //battleOption.gameObject.SetActive(true);
        //battleOption.dialogueText.text = endBattleDialogue;
    }

    public virtual void StartRound(int round)
    {
        //battleOption.gameObject.SetActive(true);
        //battleOption.dialogueText.text = roundBehaviourInfos[round].roundStartScript;
        if (skillApplyInfos[round].releaseDebuff != 0)
        {
            debuffObj.SetActive(false);
        }
    }

    public virtual void StartSelect(int round)
    {
        battleOption.gameObject.SetActive(true);
        battleOption.bottomInfoObject.gameObject.SetActive(true);
        battleOption.dialogueText.text = roundBehaviourInfos[round].roundSelectInfos[0].script;

        target = roundBehaviourInfos[round].roundSelectInfos[0].target.GetComponent<IBattleUnit>();

    }

    public void UpdatePointer(int round)
    {
        pointer.gameObject.SetActive(true);
        //Debug.Log("==============");
        //Debug.Log($"{battleOption.transform.position}");
        //Debug.Log($"{target.EndTargetPointTr.position}");
        
        pointer.Point(roundBehaviourInfos[round].roundSelectInfos[0].startPoint.position,
            roundBehaviourInfos[round].roundSelectInfos[0].endPoint.position, pointerColor);
    }
    List<BattleDice> diceList = new List<BattleDice>();
    public virtual void Roll(int round)
    {
        diceList.Clear();

        var info = roundBehaviourInfos[round].roundSelectInfos[0];
        for(int i =0;i< info.diceTypes.Length; i++)
        {
            BattleDice dice = BattleDice.Instantiate(info.diceTypes[i]);
            dice.transform.position = (Vector2)dicePoint.position + new Vector2(-1,0) * i*0.5f;
            dice.SetNumber(info.diceNumbers[i]);
            dice.transform.localScale = new Vector3(0, 0, 0);
            dice.transform.DOScale(1, 0.3f);
            diceList.Add(dice);
        }
    }


    public virtual void ApplySkill(int round, Action endEffect)
    {
        if (diceList.Count <= 0)
        {
            endEffect.Invoke();
            return;
        }
            
        StartCoroutine(CoApplySkill(round, endEffect));
    }

    IEnumerator CoApplySkill(int round, Action endEffect)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].MoveTo(target.DiceTargetPointTr.position, true, (dice) =>
            {

                endEffect?.Invoke();
            });
        }
        if (skillApplyInfos[round].damage != 0)
            targetEnemy.ChangeHp(-skillApplyInfos[round].damage);
        if(skillApplyInfos[round].heal != 0)
            targetEnemy.ChangeHp(skillApplyInfos[round].heal);
        targetEnemy.Debuff(skillApplyInfos[round].debuff);
    }
    public virtual void EndRound(int round)
    {
        pointer.gameObject.SetActive(false);
        battleOption.bottomInfoObject.gameObject.SetActive(false);
        battleOption.dialogueText.text = roundBehaviourInfos[round].roundEndScript;
    }

    public virtual void EndSelect(int round)
    {
        pointer.gameObject.SetActive(false);
    }
    public TMP_Text hpText;
    public void ChangeHp(int amount)
    {
        hpText.transform.localScale = Vector3.zero;
        hpText.text = null;
        if (amount > 0)
        {
            hpText.transform.DOScale(1, 0.1f).OnComplete(() => {
                hpText.transform.DOScale(0, 0.3f).SetDelay(2);
            });
            hpText.text = "+"+amount;
            hpText.color = Color.green;
            
        }
        else if(amount < 0)
        {
            hpText.transform.DOScale(1, 0.1f).OnComplete(()=> {
                hpText.transform.DOScale(0, 0.3f).SetDelay(2);
            });
            hpText.text = amount.ToString();
            hpText.color = Color.red;
        }

        curHp += amount;
        hpImg.fillAmount = curHp / maxHp;
    }
    public GameObject debuffObj;
    public void Debuff(int num)
    {
        if (num == 0)
        {

            return;
        }
            
        debuffObj.SetActive(true);
    }
}

public enum UnitType
{
    Fear, Brave
}