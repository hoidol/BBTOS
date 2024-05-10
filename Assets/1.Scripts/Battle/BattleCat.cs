using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
public class BattleCat : MonoSingleton<BattleCat>, IBattleUnit
{
    public GameObject catPanel;
    public CatBattleOption catBattleOption;

    public bool isBonus;

    public Transform DiceTargetPointTr => null;
    public Transform EndTargetPointTr => null;

    public string startBattleDialogue;
    public BattleCatBehaviour[] catBehaviours;


    public Color autoCheckSucColor;
    public Color autoCheckFailColor;

    public  void InitUnit()
    {
        catPanel.SetActive(false);
        catBattleOption.gameObject.SetActive(false);
        //optionPanel.SetActive(false);
        isBonus = false;
    }

    public  void ReadyBattle()
    {

    }

    public void StartRound(int round)
    {
    }

    public  void StartSelect(int round)
    {
        
        catBattleOption.gameObject.SetActive(false);
    }
    public void EndSelect(int rount)
    {

    }
    public Transform bonusDicePoint;
    public Transform successBonusDicePoint;
    public GameObject bonusDiceUI; 
    public void RollBonus(int round, Action endCallback)
    {
        catPanel.SetActive(catBehaviours[round].active);
        catBattleOption.gameObject.SetActive(catBehaviours[round].active);
        if (!catBehaviours[round].active)
        {
            endCallback?.Invoke();
            return;
        }
        catBattleOption.dialogueText.text = catBehaviours[round].script;

        if (catBehaviours[round].success)
        {

            catBattleOption.autoCheckImage.sprite = Resources.Load<Sprite>("Sprites/AutoCheck_Suc");
            catBattleOption.autoCheckText.color = autoCheckSucColor;
            catBattleOption.autoCheckText.text = "청각 굴림 성공";
            isBonus = true;
        }
        else
        {
            catBattleOption.autoCheckImage.sprite = Resources.Load<Sprite>("Sprites/AutoCheck_Fail");
            catBattleOption.autoCheckText.color = autoCheckFailColor;
            catBattleOption.autoCheckText.text = "청각 굴림 실패";

        }
        StartCoroutine(CoRollBonus(round,endCallback));
    }

    IEnumerator CoRollBonus(int round, Action endCallback)
    {
        BattleBonusFourDice dice = BattleBonusFourDice.Instantiate(DiceType.Four);
        dice.transform.position = bonusDicePoint.position;
        yield return new WaitForSeconds(1);
        if (catBehaviours[round].success)
        {
            dice.transform.DOMove(successBonusDicePoint.position, 1).OnComplete(()=> {
                bonusDiceUI.gameObject.SetActive(true);
            });
            dice.diceSpriteRdr.DOFade(0, 0.3f).SetDelay(.7f);
        }
        else
        {
            dice.diceSpriteRdr.DOFade(0, 0.3f).SetDelay(.7f);
        }

        yield return new WaitForSeconds(1);
        catPanel.SetActive(false);
        endCallback?.Invoke();
    }

    public  void EndRound(int round)
    {

    }

    public  void Roll(int round)
    {
    }

    public void ApplySkill(int round, Action endEffect)
    {

    }

    public void StartBattle()
    {
    }

    public void EndBattle()
    {
    }

    public void UpdatePointer(int round)
    {
        
    }
}
[System.Serializable]
public class BattleCatBehaviour
{
    public bool active;
    public string script;
    public bool success;
    public DiceType diceType;
    public int diceNumber;
    public Transform dicePoint;
}