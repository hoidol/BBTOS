using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
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
    public Image[] diceImages;
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
        hpImg.fillAmount = curHp/maxHp;
    }

    public virtual void StartBattle()
    {
        battleOption.gameObject.SetActive(true);

        battleOption.bottomInfoObject.gameObject.SetActive(false);
        battleOption.dialogueText.text = startBattleDialogue;
        //SoundMgr.Instance?.PlaySound(key);
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
        Debug.Log($"Key {key}   releaseDebuff {skillApplyInfos[round].releaseDebuff}");
        


    }

    public virtual void StartSelect(int round)
    {
        battleOption.gameObject.SetActive(true);
        battleOption.bottomInfoObject.gameObject.SetActive(true);
        battleOption.dialogueText.text = roundBehaviourInfos[round].roundSelectInfos[0].script;

        SoundMgr.Instance?.PlaySound(key);

        target = roundBehaviourInfos[round].roundSelectInfos[0].target.GetComponent<IBattleUnit>();

        for(int i =0;i< diceImages.Length; i++)
        {
            if(i < roundBehaviourInfos[round].roundSelectInfos[0].diceTypes.Length)
            {

                if(roundBehaviourInfos[round].roundSelectInfos[0].diceTypes[i] == DiceType.Four)
                {
                    diceImages[i].sprite = Resources.Load<Sprite>($"Sprites/DiceThum_Four");
                }
                else
                {
                    diceImages[i].sprite = Resources.Load<Sprite>($"Sprites/DiceThum_Six");
                }

                diceImages[i].gameObject.SetActive(true);
            }
            else
            {
                diceImages[i].gameObject.SetActive(false);
            }
            
            
        }
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


        if(skillApplyInfos[round].replaceDiceObject != null)
        {
            skillApplyInfos[round].replaceDiceObject.transform.localScale = Vector3.zero;
            skillApplyInfos[round].replaceDiceObject.transform.position = dicePoint.position;
            skillApplyInfos[round].replaceDiceObject.transform.DOScale(1, 0.3f);
            skillApplyInfos[round].replaceDiceObject.SetActive(true);
        }
    }


    public virtual void ApplySkill(int round, Action endEffect)
    {
        if (diceList.Count > 0)
        {
            for (int i = 0; i < diceList.Count; i++)
            {
                diceList[i].MoveTo(target.DiceTargetPointTr.position, true, (dice) =>
                {
                    if (skillApplyInfos[round].displayDamage)
                        targetEnemy.ChangeHp(skillApplyInfos[round].damage);

                    targetEnemy.Debuff(skillApplyInfos[round].debuff);
                    endEffect?.Invoke();
                });
            }
        }
        else
        {
            if (skillApplyInfos[round].replaceDiceObject != null)
            {
                Transform replaceDiceTr = skillApplyInfos[round].replaceDiceObject.transform;
                replaceDiceTr.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                replaceDiceTr.position = dicePoint.position;
                replaceDiceTr.DOMove(targetEnemy.battleOption.transform.position, 1).OnComplete(() => {


                    if (skillApplyInfos[round].displayDamage)
                        targetEnemy.ChangeHp(skillApplyInfos[round].damage);
                    targetEnemy.Debuff(skillApplyInfos[round].debuff);

                    endEffect.Invoke();
                });

                replaceDiceTr.GetComponent<Image>().DOFade(0, 0.3f).SetDelay(0.7f);
               
            }
            else
            {
                if (skillApplyInfos[round].displayDamage)
                    targetEnemy.ChangeHp(skillApplyInfos[round].damage);
                targetEnemy.Debuff(skillApplyInfos[round].debuff);

                endEffect.Invoke();
            }

           
        }
        
 
    }

    public virtual void EndRound(int round)
    {
        pointer.gameObject.SetActive(false);
        battleOption.bottomInfoObject.gameObject.SetActive(false);
        battleOption.dialogueText.text = roundBehaviourInfos[round].roundEndScript;
        //SoundMgr.Instance?.PlaySound(key);

        if (skillApplyInfos[round].releaseDebuff != 0)
        {
            Debug.Log($"버프 해제 Key {key}   releaseDebuff {skillApplyInfos[round].releaseDebuff}");
            debuffObj.SetActive(false);
        }

        if (skillApplyInfos[round].replaceDiceObject != null)
        {
            skillApplyInfos[round].replaceDiceObject.transform.DOScale(0, 0.2f);
            skillApplyInfos[round].replaceDiceObject.SetActive(false);
        }
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
            hpText.color = Color.green;
            hpText.transform.DOScale(1, 0.1f).OnComplete(() => {
                hpText.transform.DOScale(0, 0.3f).SetDelay(2);
            });
            hpText.text = "+"+amount;
            SoundMgr.Instance?.PlaySound("Heal");
        }
        else if(amount <= 0)
        {
            SoundMgr.Instance?.PlaySound("Attack");
            hpText.transform.DOScale(1, 0.1f).OnComplete(()=> {
                hpText.transform.DOScale(0, 0.3f).SetDelay(2);
            });
            hpText.text = amount.ToString();
            if(amount== 0)
            {
                hpText.text = "-0";
            }
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