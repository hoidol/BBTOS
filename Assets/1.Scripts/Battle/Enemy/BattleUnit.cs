//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//public abstract class BattleUnit : MonoBehaviour, IBattleUnit
//{
//    public string key;
//    public float maxHp;
//    public float curHp;
//    public UnitType unitType;
//    public BattleUnit targetUnit;
//    public EnemyBattleOption battleOption;
//    public Transform DiceTargetPoint => battleOption.transform;

//    public abstract void InitUnit();

//    public virtual void ReadyBattle()
//    {

//    }

//    public virtual void TakeDamage(float damage)
//    {

//    }

//    public abstract void StartRound(int round);
//    public abstract void StartSelect(int round);
//    public abstract void EndSelect(int round);
//    public abstract void Roll(int round);

//    public virtual void ApplySkill(int round, Action endEffect)
//    {

//    }

//    public abstract void EndRound(int round);

//}
