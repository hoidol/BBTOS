using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractBattle : InteractEvent
{
    bool interacting;
    public GameObject candy;
    public override void Interact(Action eCallback)
    {
        interacting = true;
        StartCoroutine(CoWaitEndBattle(eCallback));
    }

    IEnumerator CoWaitEndBattle(Action eCallback)
    {
        WaitUntil waitUntil = new WaitUntil(() =>
        {
            return GameMgr.Instance.curModeType == GameModeType.Normal;
        });

        FadeEffect.Instance.PlayFadeOutAndIn(1, () =>
        {
            SoundMgr.Instance?.PlayBGM(1);
            GameMgr.Instance.gameModes[0].gameObject.SetActive(false);
            GameMgr.Instance.gameModes[1].gameObject.SetActive(true);
            candy.SetActive(false);
        }, () => {
            GameMgr.Instance.SwitchMode(GameModeType.Battle);
        });
        
        yield return new WaitForSeconds(5);
        yield return waitUntil;
        eCallback?.Invoke();

    }


}
