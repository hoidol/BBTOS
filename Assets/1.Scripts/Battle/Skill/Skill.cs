using System;
using UnityEngine;

[System.Serializable]
public abstract class Skill : MonoBehaviour
{
    public string key;
    public int skillNumber;
    public Sprite sprite;
    public string skillName;
    public string skillInfo;
    public string targetEnemyKey;
    public DiceType diceType;
    public int diceCount;


    public abstract void ExcuteSkill(IBattleUnit unit, params int[] diceNumber);
}


public enum DiceType
{
    Four,
    Six,
    Twenty
}