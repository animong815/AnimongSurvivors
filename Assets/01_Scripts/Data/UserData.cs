using System.Collections.Generic;

public class UserDataItem
{
    public int id;
    public int level;
    public string prefab;
    public int skill;
    public Dictionary<string, int> value;
    public Dictionary<string, int[]> values;
}

public class UserData : DataBase
{
    public Dictionary<int,Dictionary<int, UserDataItem>> dic;
    private List<int> listIdx;
    private List<int> listLevel;
    private string[] types;


    public override void Init()
    {
        base.Init();
        dic = new Dictionary<int, Dictionary<int, UserDataItem>>();
        listIdx = new List<int>();
        listLevel = new List<int>();
        Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=1846910812");
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
                listIdx.Add(-1);
                continue;
            }
            listIdx.Add(int.Parse(_row[i]));
            if(dic.ContainsKey(listIdx[i])) continue;
            dic.Add(listIdx[i], new Dictionary<int, UserDataItem>());
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
                if( i == idx_key || i == idx_desc) 
                {
                    listLevel.Add(-1);
                    continue;
                }
                listLevel.Add( int.Parse(_row[i]));
                dic[listIdx[i]].Add(listLevel[i], new UserDataItem());
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
            if( i == idx_key || i == idx_desc) continue;
            //UnityEngine.Debug.Log(i + ":"+ listId[i] + ":" +_row[idx_key]+ ":"+ _row[i]);
            idx = listIdx[i];
            level = listLevel[i];
            switch(_row[idx_key])
            {
                case "prefab":              
                    dic[idx][level].prefab = _row[i]; 
                    break;
                case "skill":
                    dic[idx][level].skill = int.Parse(_row[i]);
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