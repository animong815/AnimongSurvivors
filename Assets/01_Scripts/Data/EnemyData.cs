using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataItem
{
    public int id;
    public string prefab;
    public int hp;
    public int speed;
    public int attack;
    public float attack_delay;
    public float life_time;
    public string direct_type;
    public string touch_type;
    public float scale;
    public Color color;

	public bool hit_bg;
	public bool hit_enemy;
}

public class EnemyData : DataBase
{
    public Dictionary<int, EnemyDataItem> dic;
    private List<int> listIdx;

    public override void Init()
    {
        base.Init();
        dic = new Dictionary<int, EnemyDataItem>();
        listIdx = new List<int>();

        Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=1660719681");
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
            dic.Add(listIdx[i], new EnemyDataItem());
            dic[listIdx[i]].id = listIdx[i];
        }        
    }
    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
        int idx;
        for(int i=0; i < _row.Length; i++)
        {
            if( i == idx_max) break;
            if( i == idx_key || i == idx_desc) continue;
            idx = listIdx[i];
            switch(_row[idx_key])
            {
                case "prefab":              
                    dic[idx].prefab = _row[i]; 
                    break;
                 case "hp": dic[idx].hp = int.Parse(_row[i]); break;
                case "speed": dic[idx].speed = int.Parse(_row[i]); break;
                case "attack": dic[idx].attack = int.Parse(_row[i]); break;
                case "attack_delay": dic[idx].attack_delay = float.Parse(_row[i]) * 0.001f; break;
                case "life_time": dic[idx].life_time = float.Parse(_row[i]) * 0.001f; break;
                case "direct_type": dic[idx].direct_type = _row[i]; break;
                case "touch_type": dic[idx].touch_type = _row[i]; break;
                case "scale": dic[idx].scale = float.Parse(_row[i]) * 0.01f; break;
                case "color": dic[idx].color = StrToColor(_row[i].Replace("#", "")); break;    
				case "hit_bg": dic[idx].hit_bg = _row[i]== "true"; break;
				case "hit_enemy": dic[idx].hit_enemy = _row[i]== "true"; break;
            }
        }
    }

    public Color StrToColor(string str)
    {
        str = str.ToLowerInvariant();
        if(str.Length == 6)
        {
            char[] arr 			= str.ToCharArray();
            char[] color_arr 	= new char[6];

            for(int i = 0 ; i < 6 ; i++)
            {
                if(arr[i] >= '0' && arr[i] <= '9')
                    color_arr[i] = (char)(arr[i] - '0');
                else if(arr[i] >= 'a' && arr[i] <= 'f')
                    color_arr[i] = (char)(10 + arr[i] - 'a');
                else
                    color_arr[i] = (char)0;
            }

            float red 	= (color_arr[0]*16 + color_arr[1])/255.0f;
            float green = (color_arr[2]*16 + color_arr[3])/255.0f;
            float blue 	= (color_arr[4]*16 + color_arr[5])/255.0f;

            return new Color(red, green, blue);
        }
        else
            return Color.clear;
    }

}
