using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Dice : MonoSingleton<Dice>
{
    public GameObject zoomCanvas;
    public Transform zoomInTr;
    public Image diceImage;
    //public static Dice Instantiate(Vector2 start, Action<int> endCallback)
    //{
    //    Dice dicePrefab = Resources.Load<Dice>("Prefabs/Dice");
    //    Dice dice = Instantiate(dicePrefab);
    //    dice.transform.position = start;
    //    dice.Roll(start + Vector2.down * 0.6f, endCallback);
    //    return dice;
    //}

    public void Roll(Vector2 start,int diceNumber, Action<int> endCallback)
    {
        SoundMgr.Instance?.PlaySound("Dice");
        gameObject.SetActive(true);
        diceImage.sprite = Resources.Load<Sprite> ($"Sprites/Dice_zoom_{diceNumber}");
        transform.position = start;
        zoomCanvas.gameObject.SetActive(false);
        StartCoroutine(CoRoll(start + Vector2.down * 1.5f, endCallback));
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
            transform.position = Vector2.Lerp(transform.position, end, Time.deltaTime * 6);
        }
        
        //CameraMgr.Instance.ZoomIn(zoomInTr.position);
        zoomCanvas.gameObject.SetActive(true);
        SoundMgr.Instance?.PlaySound("DiceResult");
        yield return new WaitForSeconds(1.5f);

        endCallback.Invoke(UnityEngine.Random.Range(0, 7));

        //yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        //yield return new WaitForSeconds(2);
        //Destroy(gameObject);
    }
}
