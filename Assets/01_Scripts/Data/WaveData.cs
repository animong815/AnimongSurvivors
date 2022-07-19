using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDataItem
{
    public string id;
    public int[] checkTime;
    public int createTime;
    public int[] enemys;
    public int count;
    public int[] position_range;
}

public class WaveData : DataBase
{
    public Dictionary<string, WaveDataItem> dic;
    private List<string> listIdx;

    public override void Init()
    {
        base.Init();
        dic = new Dictionary<string, WaveDataItem>();
        listIdx = new List<string>();
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
            if( i == idx_key || i == idx_desc) 
            {
                listIdx.Add(string.Empty);
                continue;
            }
            listIdx.Add(_row[i]);
            if(dic.ContainsKey(listIdx[i])) continue;
            dic.Add(listIdx[i], new WaveDataItem());
            dic[listIdx[i]].id = listIdx[i];
        }
    }
    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
        
        string idx;
        string[] values;
        
        for(int i=0; i < _row.Length; i++)
        {
            if( i == idx_key || i == idx_desc) continue;
            //UnityEngine.Debug.Log(i + ":"+ listId[i] + ":" +_row[idx_key]+ ":"+ _row[i]);
            idx = listIdx[i];
            switch(_row[idx_key])
            {
                case "checkTime": 
                    values = _row[i].Split('|');             
                    {
                        dic[idx].checkTime = new int[values.Length];
                        for(int j = 0; j < values.Length; j++)
                            dic[idx].checkTime[j] = int.Parse(values[j]); 
                    }
                    break;
                case "createTime":
                    dic[idx].createTime = int.Parse(_row[i]);
                    break;
                case "enemys":
                    values = _row[i].Split('|');             
                    {
                        dic[idx].enemys = new int[values.Length];
                        for(int j = 0; j < values.Length; j++)
                            dic[idx].enemys[j] = int.Parse(values[j]); 
                    }
                    break;
                case "count": dic[idx].count = int.Parse(_row[i]); break;
                case "position_range":
                    values = _row[i].Split('|');             
                    {
                        dic[idx].position_range = new int[values.Length];
                        for(int j = 0; j < values.Length; j++)
                            dic[idx].position_range[j] = int.Parse(values[j]); 
                    }
                break;
            }
        }
    }

}
