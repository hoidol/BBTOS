using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBattleUnit
{

    public Transform DiceTargetPointTr
    {
        get;
    }
    public Transform EndTargetPointTr
    {
        get;
    }

    public void InitUnit();
    public void StartBattle();
    public void EndBattle();

    public void StartRound(int round);
    public void StartSelect(int round);
    public void UpdatePointer(int round);

    public void ApplySkill(int round, Action endEffect);
    public void EndRound(int round);

    public void Roll(int round);
    public void EndSelect(int round);

}
