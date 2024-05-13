using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    public GameObject dragInfo;
    public GameMode[] gameModes;
    GameMode curGameMode;
    public GameModeType curModeType;
    public override void Awake()
    {
        //gameModes = FindObjectsOfType<GameMode>(true);
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
        SoundMgr.Instance.PlayBGM(0);
        curModeType = GameModeType.Normal;
        curGameMode = GetGameMode(curModeType);
        curGameMode.StartMode();
        
        Invoke("StartGame", 1);
    }

    void StartGame()
    {
        DialogueMgr.Instance.StartDialogue(0, () =>
        {
            dragInfo.SetActive(true);
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
