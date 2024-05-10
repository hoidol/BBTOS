using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TMP_Text text;

    public static void DisplayText(Vector2 point, string damage)
    {
        DamageText textPrefab = Resources.Load<DamageText>("Prefabs/DamageText");
        DamageText txt = Instantiate(textPrefab);
        txt.transform.position = point;
        txt.text.text = damage;

        Destroy(txt.gameObject, 1.5f);
    }
    
}
