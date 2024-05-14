using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMgr : MonoBehaviour
{
    public bool cliched;
    public void OnClickedStart()
    {
        //if (cliched)
        //    return;

        //cliched = true;
        FadeEffect.Instance.PlayFadeOutAndIn(1,
            ()=> {
                SceneManager.LoadScene("Game");
            },()=> {
            });
        
    }
}
