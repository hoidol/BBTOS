using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public GameModeType type;

    public virtual void StartMode()
    {

    }
        
}

public enum GameModeType
{
    Normal,
    Battle
}