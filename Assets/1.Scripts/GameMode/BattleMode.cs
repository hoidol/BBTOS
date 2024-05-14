using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BattleMode : GameMode
{
    BattleEnemy[] battleEnemies;
    public GameObject playerArea;
    public Transform[] playerAreaPoint; //0 -> 1
    public GameObject enemyArea;
    public Transform[] enemyAreaPoint; //0 -> 1

    public InteractEvent[] endBattleEvents;
    private void OnEnable()
    {
        playerArea.transform.position = playerAreaPoint[0].position;
        enemyArea.transform.position = enemyAreaPoint[0].position;
    }

    private void Start()
    {
        //Time.timeScale = 2.5f;
        //StartMode();
    }

    public override void StartMode()
    {
        if (battleEnemies == null)
            battleEnemies = FindObjectsOfType<BattleEnemy>(true);

        gameObject.SetActive(true);
        for (int i =0;i< battleEnemies.Length; i++)
        {
            battleEnemies[i].InitUnit();
        }
        BattlePlayer.Instance.InitUnit();
        BattleCat.Instance.InitUnit();

        playerArea.transform.DOMove(playerAreaPoint[1].position, 2f);
        enemyArea.transform.DOMove(enemyAreaPoint[1].position, 2f);

        Invoke("ReadyBattle", 1.5f);
    }

    

    public void EndMode()
    {
        playerArea.transform.DOMove(playerAreaPoint[0].position, 1.5f);
        enemyArea.transform.DOMove(enemyAreaPoint[0].position, 1.5f);
        Invoke("FinishMode", 1.5f);
    }
     void ReadyBattle()
    {
        BattleMgr.Instance.ReadyBattle();
    }
     void FinishMode()
    {
        //GameMgr.Instance.SwitchMode(GameModeType.Normal);
        FadeEffect.Instance.PlayFadeOutAndIn(1, () =>
        {
            SoundMgr.Instance.PlayBGM(2);

            GameMgr.Instance.gameModes[0].gameObject.SetActive(true);
            GameMgr.Instance.gameModes[1].gameObject.SetActive(false);
        }, () => { 
            GameMgr.Instance.SwitchMode(GameModeType.Normal);
            /*endBattleEvents[0].Interact(() => {

                FadeEffect.Instance.PlayFadeOutAndIn(1, () =>
                {
                    SoundMgr.Instance.StopBGM();
                    endBattleEvents[1].Interact(() =>
                    {
                    });
                }, () =>
                {
                });
                
            });*/

        });
        gameObject.SetActive(false);
    }
}
