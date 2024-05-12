using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class BattleDice : MonoBehaviour
{
    public DiceType diceType;
    public int minNumber=1;
    public int maxNumber;
    public int curNumber;

    public DiceNumberInfo[] numberInfos;
    public SpriteRenderer diceSpriteRdr;

    public static BattleDice Instantiate(DiceType diceType)
    {
        BattleDice dicePrefab = Resources.Load<BattleDice>($"Prefabs/Battle{diceType}Dice");
        BattleDice dice = Instantiate(dicePrefab);
        dice.diceSpriteRdr.color = Color.white;
        dice.transform.DOScale(dice.initScale, 0.3f);
        SoundMgr.Instance?.PlaySound("Dice");
        return dice;
    }
    public float initScale;
    private void Awake()
    {
        initScale = transform.localScale.x;
    }
    public virtual void SetNumber(int number)
    {
        Debug.Log($"{number}_{diceType}");
        diceSpriteRdr.sprite = Resources.Load<Sprite>($"Sprites/{number}_{diceType}");
    }

    public void Roll(Vector2 end, Action<int> endCallback)
    {
        StartCoroutine(CoRoll(end, endCallback));
    }

    public void MoveTo(Vector2 end,bool fade, Action<BattleDice> endCallback)
    {
        transform.DOMove(end, 1).OnComplete(()=> {
            endCallback.Invoke(this);
        });
        
        if (fade)
        {
            diceSpriteRdr.DOFade(0, 0.3f).SetDelay(0.7f);
        }
    }

    IEnumerator CoRoll(Vector2 end, Action<int> endCallback)
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, end) < 0.01f)
            {
                break;
            }
            yield return null;
            transform.position = Vector2.Lerp(transform.position, end, Time.deltaTime * 10);
        }

        CameraMgr.Instance.ZoomIn(end);
        yield return new WaitForSeconds(2);
        endCallback.Invoke(UnityEngine.Random.Range(0, 7));
    }
}

[System.Serializable]
public class DiceNumberInfo
{
    public int number;
    public Sprite sprite;
}