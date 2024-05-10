using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleBonusFourDice : MonoBehaviour
{
    public int minNumber = 1;
    public int maxNumber;
    public int curNumber;
    public SpriteRenderer diceSpriteRdr;
    public static BattleBonusFourDice Instantiate(DiceType diceType)
    {
        BattleBonusFourDice dicePrefab = Resources.Load<BattleBonusFourDice>($"Prefabs/BattleBonusFourDice");
        BattleBonusFourDice dice = Instantiate(dicePrefab);
        dice.diceSpriteRdr.color = Color.white;
        dice.transform.localScale = new Vector3(0, 0, 0);
        dice.transform.DOScale(1, 0.4f);
        return dice;

    }
    public void SetNumber(int number)
    {

    }

    public void Roll(Vector2 end, Action<int> endCallback)
    {
        StartCoroutine(CoRoll(end, endCallback));
    }

    public void MoveTo(Vector2 end, bool fade, Action endCallback)
    {
        transform.DOMove(end, 1).OnComplete(() => {
            endCallback.Invoke();
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
