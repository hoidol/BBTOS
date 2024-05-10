using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{

    public GameMode[] gameModes;
    GameMode curGameMode;
    public GameModeType curModeType;
    public override void Awake()
    {
        gameModes = FindObjectsOfType<GameMode>(true);
    }

    
    GameMode GetGameMode(GameModeType type)
    {
        for(int i =0;i< gameModes.Length; i++)
        {
            if (gameModes[i].type == type)
                return gameModes[i];
        }
        return null;
    }

    public void Start()
    {
        curModeType = GameModeType.Normal;
        curGameMode = GetGameMode(curModeType);
        curGameMode.StartMode();
        FadeEffect.Instance.PlayFadeIn(1, () =>
        {

            DialogueMgr.Instance.StartDialogue(0, () =>
            {

            });
        });
    }

    public void SwitchMode(GameModeType mType)
    {
        curModeType = mType;
        for (int i = 0; i < gameModes.Length; i++)
        {
            if (gameModes[i].type == curModeType)
            {
                gameModes[i].StartMode();
            }
        }


    }
}
