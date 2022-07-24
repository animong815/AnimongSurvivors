using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Stage 
{
    public RectTransform rtSkill;
    public List<Skill> skills;
    private Dictionary<string, int> keySkill;
    private Dictionary<string, List<Skill>> listSkill;
    private Skill tmpSkill;
    public void InitSkill()
    {
        listSkill = new Dictionary<string, List<Skill>>();
        keySkill = new Dictionary<string, int>();
        for(int i =0; i< skills.Count; i++)
        {
            skills[i].go.SetActive(false);
            keySkill.Add(skills[i].go.name, i);
            listSkill.Add(skills[i].go.name, new List<Skill>());
        }
    }

    public void CreateSkill(ObjectBase _actor, string _key, SkillDataItem _data)
    {
        tmpSkill = GetSkill(_key);
        if(tmpSkill == null) return;
        tmpSkill.SetData(_data, _actor);        
    }

    private Skill GetSkill(string _idx) 
    {
        if(listSkill.ContainsKey(_idx) == false)
        { 
            Debug.LogWarning(_idx + " prefab not have");
            return null;
        }
        for(int i = 0; i < listSkill[_idx].Count; i++)
        {
            if(listSkill[_idx][i].go.activeSelf == false)
            {
                tmpSkill = listSkill[_idx][i];
                listSkill[_idx].Remove(tmpSkill);
                tmpSkill.rt.SetParent(rtSkill);
                tmpSkill.go.SetActive(true);
                listView.Add(tmpSkill);
                return tmpSkill;
            }
        }

        tmpSkill = Instantiate<Skill>(skills[keySkill[_idx]], rtSkill);
        tmpSkill.rt.localScale = Vector3.one;
        tmpSkill.rt.localRotation = Quaternion.identity;
        tmpSkill.go.SetActive(true);
        tmpSkill.Init();
        listView.Add(tmpSkill);

        return tmpSkill;
    }

    public void ReturnSkill(Skill _skill)
    {
        listView.Remove(_skill);
        _skill.go.SetActive(false);
        listSkill[_skill.data.prefab].Add(_skill);
    }
}
