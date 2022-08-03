using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDataItem
{
    public string id;
    public float[] checkTime;
    public float createTime;
    public int[] enemys;
    public int count;
    public int[] position_range;
	public string position_type;
}

public class WaveData : DataBase
{
	public const string TYPE_BOX = "box";
	public const string TYPE_CIRCLE = "circle";
	public const string TYPE_PREFAB = "prefab";
	public const string TYPE_VERTICAL = "vertical";
	public const string TYPE_HORIZON = "horizon";

    public List<WaveDataItem> list;

    public override void Init()
    {
        base.Init();
        list = new List<WaveDataItem>();
        Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=1829669208");
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
                list.Add(null);
                continue;
            }
            list.Add(new WaveDataItem());
            list[i].id = _row[i];
        }
    }
    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
        
        string[] values;
        
        for(int i=0; i < _row.Length; i++)
        {
            if( i == idx_max) break;
            if( i == idx_key || i == idx_desc) continue;
            //UnityEngine.Debug.Log(i + ":"+ listId[i] + ":" +_row[idx_key]+ ":"+ _row[i]);
            switch(_row[idx_key])
            {
                case "checkTime": 
                    values = _row[i].Split('|');             
                    {
                        list[i].checkTime = new float[values.Length];
                        for(int j = 0; j < values.Length; j++)
                            list[i].checkTime[j] = float.Parse(values[j]) * 0.001f; 
                    }
                    break;
                case "createTime":
                    list[i].createTime = float.Parse(_row[i]) * 0.001f;
                    break;
                case "enemys":
                    values = _row[i].Split('|');             
                    {
                        list[i].enemys = new int[values.Length];
                        for(int j = 0; j < values.Length; j++)
                            list[i].enemys[j] = int.Parse(values[j]); 
                    }
                    break;
                case "count": list[i].count = int.Parse(_row[i]); break;
                case "position_range":
                    values = _row[i].Split('|');             
                    {
                        list[i].position_range = new int[values.Length];
                        for(int j = 0; j < values.Length; j++)
                            list[i].position_range[j] = int.Parse(values[j]); 
                    }
                break;
				case "position_type": list[i].position_type = _row[i]; break;
            }
        }
    }

}
