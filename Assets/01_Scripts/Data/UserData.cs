using System.Collections.Generic;

public class UserDataItem
{
    public string prefab;
    public int hp;
    public int move_speed;
    public int move_speed_stamina;
    public int stamina_max;
    public int stamina_use;
    public int stamina_add;
    public int groggy_value;
    public string skill;
    public int attack;
    public int delay;
    public int size;
    public int range;
    public int target_count;
    public int defence;
}

public class UserData : DataBase
{
    public Dictionary<int, UserDataItem> data;
    private List<int> listId;
    public override void Init()
    {
        base.Init();
        data = new Dictionary<int, UserDataItem>();
        listId = new List<int>();
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
                listId.Add(-1);
                continue;
            }
            listId.Add( int.Parse(_row[i]));
            //UnityEngine.Debug.Log("key : " + _row[i]);
            data.Add(int.Parse(_row[i]), new UserDataItem());
        }
    }

    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
        //UnityEngine.Debug.Log("idx_key :" + idx_key + ",idx_desc:" + idx_desc);
        
        for(int i=0; i < _row.Length; i++)
        {
            if( i == idx_key || i == idx_desc) continue;
            //UnityEngine.Debug.Log(i + ":"+ listId[i] + ":" +_row[idx_key]+ ":"+ _row[i]);
            switch(_row[idx_key])
            {
                case "attack": data[listId[i]].attack = int.Parse(_row[i]); break;
                
            }
        }
    }

}