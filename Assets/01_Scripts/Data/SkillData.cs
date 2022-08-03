using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataItem
{
    public int id;
    public int level;
    public string prefab;
    public SkillData.TYPE skill_type;
    public Dictionary<string, int> value;
    public Dictionary<string, int[]> values;
    public float activeTime;

	public int GetValue(string _key)
	{
		return value.ContainsKey(_key) == false ? 0 : value[_key];
	}
	public float GetValueTime(string _key)
	{
		return value.ContainsKey(_key) == false ? 0 : (float)value[_key] * 0.001f;
	}
}

public class SkillData : DataBase
{
    public enum TYPE
    {
        melee,
        range,
        buff
    }
    public const string hp = "hp";
    public const string attack = "attack";
    public const string defence = "defence";
    public const string attack_delay = "attack_delay";
    public const string size = "size";
    public const string speed = "speed";
    public const string range = "range";
    public const string move_speed = "move_speed";
    public const string move_speed_stamina = "move_speed_stamina";
    public const string stamina_max = "stamina_max";
    public const string stamina_use = "stamina_use";
    public const string stamina_add = "stamina_add";
    public const string groggy_value = "groggy_value";
	public const string groggy_speed = "groggy_speed";

	public const string back_speed = "back_speed";
	public const string back_time = "back_time";
	public const string stun_time = "stun_time";

    private List<int> listIdx;
    private List<int> listLevel;

    public Dictionary<int,Dictionary<int, SkillDataItem>> dic;

    private string[] types;

    public override void Init()
    {
        base.Init();
        dic = new Dictionary<int, Dictionary<int, SkillDataItem>>();
        listIdx = new List<int>();
        listLevel = new List<int>();

        Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=1530354356");
/*
#if UNITY_EDITOR
		Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
#else
		Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
#endif
*/
    }

    protected override void ParseDataFirst(string[] _row)
    {
        base.ParseDataFirst(_row);

        for(int i=0; i< _row.Length; i++)
        {
            if( i == idx_max) break;
            if( i == idx_key || i == idx_desc) 
            {
                listIdx.Add(-1);
                continue;
            }
            listIdx.Add(int.Parse(_row[i]));
            if(dic.ContainsKey(listIdx[i])) continue;
            dic.Add(listIdx[i], new Dictionary<int, SkillDataItem>());
        }
    }

    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
        //UnityEngine.Debug.Log("idx_key :" + idx_key + ",idx_desc:" + idx_desc);
        if(_row[idx_key] == "level")
        {
            for(int i =0; i< _row.Length; i++)
            {
                if( i == idx_max) break;
                if( i == idx_key || i == idx_desc) 
                {
                    listLevel.Add(-1);
                    continue;
                }
                listLevel.Add( int.Parse(_row[i]));

                dic[listIdx[i]].Add(listLevel[i], new SkillDataItem());
                dic[listIdx[i]][listLevel[i]].id = listIdx[i];
                dic[listIdx[i]][listLevel[i]].level = listLevel[i];
                dic[listIdx[i]][listLevel[i]].value = new Dictionary<string, int>();
                dic[listIdx[i]][listLevel[i]].values = new Dictionary<string, int[]>();
            }
            //UnityEngine.Debug.Log("key : " + _row[i]);
            return;
        }
        if(_row[idx_key] == "type")
        {
            types = _row;
            return;
        }
        int idx;
        int level;
        string type;
        int value;
        string[] values;
        //UnityEngine.Debug.Log("idx_key : " + idx_key + ", idx_desc : " + idx_desc+ ", _row.Length : " + _row.Length);
        for(int i=0; i < _row.Length; i++)
        {
            if( i == idx_max) break;
            if( i == idx_key || i == idx_desc) continue;
            //UnityEngine.Debug.Log(i + ":"+ listId[i] + ":" +_row[idx_key]+ ":"+ _row[i]);
            idx = listIdx[i];
            level = listLevel[i];
            switch(_row[idx_key])
            {
                case "prefab":              
                    dic[idx][level].prefab = _row[i]; 
                    break;
                case "skill_type":
                    dic[idx][level].skill_type = (TYPE)System.Enum.Parse(typeof(TYPE), _row[i]);
                    break;
                case "value":
                    //UnityEngine.Debug.Log("idx : " + idx + ", types[i] : " + types[i] + ",check : " + dic[idx].value.ContainsKey(types[i]));
                    type = types[i].Trim();
                    if(string.IsNullOrEmpty(type) || string.IsNullOrEmpty(_row[i])) break;
                    if(int.TryParse(_row[i], out value) == false)
                    {
                        if(dic[idx][level].values.ContainsKey(type) == false)
                        {
                            values = _row[i].Split('|');
                            dic[idx][level].values.Add(type, new int[values.Length]);
                            for(int j =0; j < values.Length; j++)
                            {
                                dic[idx][level].values[type][j] = int.Parse(values[j]);
                            }
                        }
                        break;
                    }
                    if(dic[idx][level].value.ContainsKey(type) == false)
                    {
                        dic[idx][level].value.Add(type, value);
                    }
                break;
            }
        }
    }

}
