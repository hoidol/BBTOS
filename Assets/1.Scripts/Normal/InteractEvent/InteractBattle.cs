using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractBattle : InteractEvent
{
    bool interacting;
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
            
        }, () => {
            GameMgr.Instance.SwitchMode(GameModeType.Battle);
        });
        
        yield return new WaitForSeconds(5);
        yield return waitUntil;
        eCallback?.Invoke();

    }


}
