using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class InteractEvent : MonoBehaviour
{
    public abstract void Interact(Action endCallback);
}
