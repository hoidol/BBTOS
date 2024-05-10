using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoSingleton<SkillMgr>
{
    public Dictionary<string, Skill> skillDic = new Dictionary<string, Skill>();
    public Skill[] skills;

    public GameObject[] skillInfos;
    public override void Awake()
    {
        skills = GetComponentsInChildren<Skill>();
        for(int i =0; i < skills.Length; i++)
        {
            skillDic.Add(skills[i].key, skills[i]);
        }
    }

    public Skill GetSkill(int number)
    {
        for(int i =0;i< skills.Length; i++)
        {
            if (skills[i].skillNumber == number)
                return skills[i];
        }
        return null;
    }
}
