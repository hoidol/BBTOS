using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractActive : InteractEvent
{
    public GameObject[] activeObject;
    public GameObject[] inactiveObject;
    public override void Interact(Action endCallback)
    {
        for (int i = 0; i < activeObject.Length; i++)
            activeObject[i].SetActive(true);
        for (int i = 0; i < inactiveObject.Length; i++)
            inactiveObject[i].SetActive(false);

        endCallback.Invoke();
    }
}
