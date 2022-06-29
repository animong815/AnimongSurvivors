using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : DataBase
{
	public string ad_id;
	public int ad_cnt;
	public float ad_wait_time;

    public override void Init()
    {
        base.Init();
		
		ad_id = "";
		ad_cnt = 1;
		ad_wait_time = 1f;

		Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
/*
#if UNITY_EDITOR
		Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
#else
		Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
#endif
*/
    }

    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
		
		switch (_row[idx_key])
		{
			case "ad_id": ad_id = _row[idx_value]; break;
			case "ad_cnt": ad_cnt = int.Parse(_row[idx_value]); break;
			case "ad_wait_time": ad_wait_time = float.Parse(_row[idx_value]); break;
		}
    }

}
