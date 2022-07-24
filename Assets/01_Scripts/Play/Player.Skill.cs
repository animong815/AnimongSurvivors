using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public List<SkillDataItem> listSkill;
    private int skill_i;
    private SkillDataItem tmpSkill;

    public void SetSkillData()
    {
        listSkill = new List<SkillDataItem>();
        listSkill.Add(PlayManager.ins.data.skill.dic[data.skill][1]);
        listSkill[0].activeTime = Time.time;
    }

    public void AddSkillData(int _id, int _level)
    {

    }

    public void UpdateSkill()
    {
        for(skill_i = 0; skill_i < listSkill.Count; skill_i++)
        {
            tmpSkill = listSkill[skill_i];
            if(tmpSkill.activeTime + ((float)tmpSkill.value[SkillData.attack_delay] * 0.001f) < Time.time)
            {
                //Debug.Log("Skill " + skill_i + " Active");
                PlayManager.ins.stage.CreateSkill(this, tmpSkill.prefab, tmpSkill);
                tmpSkill.activeTime = Time.time;
            }

        }
    }

}
