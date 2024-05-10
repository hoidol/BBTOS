using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//스킬 마우스에 올리면 정보 보임
public class SkillInfoPopUp : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public static SkillInfoPopUp focusPopUp;
    public int skillNumber;
    public Transform placePoint;
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (SkillInfoPopUp.focusPopUp != null)
        {
            SkillInfoPopUp.focusPopUp.Focus(false);
        }

        SkillInfoPopUp.focusPopUp = this;
        SkillInfoPopUp.focusPopUp.Focus(true);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (SkillInfoPopUp.focusPopUp != null)
        {
            SkillInfoPopUp.focusPopUp.Focus(false);
        }
    }

    public void Focus(bool b)
    {
        if(placePoint != null)
        {
            SkillMgr.Instance.skillInfos[skillNumber - 1].transform.position = placePoint.position;
        }
        SkillMgr.Instance.skillInfos[skillNumber - 1].SetActive(b);
    }


}
