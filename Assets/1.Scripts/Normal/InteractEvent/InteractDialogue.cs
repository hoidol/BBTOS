using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractDialogue : InteractEvent
{
    public int startIdx;
    
    public override void Interact(Action endCallback)
    {
        DialogueMgr.Instance.StartDialogue(startIdx, endCallback);
    }
}
