using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class InteractLoadScene : InteractEvent
{
    public string sceneName;
    
    public override void Interact(Action endCallback)
    {
        SceneManager.LoadScene(sceneName);   
    }
}
