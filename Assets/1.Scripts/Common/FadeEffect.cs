using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FadeEffect : MonoSingleton<FadeEffect>
{
    CanvasGroup canvasGroup;

    public override void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">페이드 인아웃 시 시간 설정</param>
    /// <param name="endFadeOut">화면이 안보이게됐을 때 호출되는 함수</param>
    /// <param name="endFadeIn">화면이 완전히 밝아졌을 때 호출되는 함수</param>
    public void PlayFadeOutAndIn(float time, Action endFadeOut, Action endFadeIn)
    {
        gameObject.SetActive(true);
        StartCoroutine(CoFadeOut(time, endFadeOut, endFadeIn));
    }

    public void PlayFadeIn(float time, Action endFadeInEffect)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        StartCoroutine(CoFadeIn(time, endFadeInEffect));

    }

    IEnumerator CoFadeOut(float time, Action endFadeEffect, Action endFadeIn)
    {
        //0 -> 1
        float timer = 0;
        while (true)
        {
            if (canvasGroup.alpha >= 1)
            {
                break;
            }
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / time;
            yield return null;
        }


        endFadeEffect.Invoke();
        yield return new WaitForSeconds(1);
        yield return CoFadeIn(timer, endFadeIn);
    }
    IEnumerator CoFadeIn(float time, Action endFadeEffect)
    {
        //1 -> 0
        float timer = time;
        while (true)
        {
            if (canvasGroup.alpha <= 0)
            {
                break;
            }
            timer -= Time.deltaTime;
            canvasGroup.alpha = timer / time;
            yield return null;
        }
        endFadeEffect.Invoke();
        gameObject.SetActive(false);

    }
}
