using System.Collections.Generic;


public class StageBgDataItem
{
	public int show;
	public int[] case_type;
}

public class StageDataItem
{
    public int id;
    public int width;
    public int height;
	public Dictionary<string, StageBgDataItem> dicCase;
	/*
	public int[] tree_case;
	public int tree_show;
	public int[] grass_case;
	public int grass_show;
	public int[] flower_case;
	public int flower_show;	
	public int[] bush_case;
	public int bush_show;
	*/
}

public class StageData : DataBase
{
    public Dictionary<int, StageDataItem> dic;
    private List<int> listIdx;
	
	private int currentStage = 1;

    public override void Init()
    {
        base.Init();
        dic = new Dictionary<int, StageDataItem>();
        listIdx = new List<int>();
        Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=421417172");
    }

	public void SetStage(int _value)
	{
		currentStage = _value;
	}
	public StageDataItem CurrentStage
	{
		get 
		{
			if(dic == null)
			{	
				//UnityEngine.Debug.LogError("STAGE null");
				return null;
			}
			if(dic.ContainsKey(currentStage))
				return dic[currentStage];
			else
			{
				UnityEngine.Debug.LogError("NOT HAVE STAGE " + currentStage);
				return null;
			}
		}
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
            dic.Add(listIdx[i], new StageDataItem());
			dic[listIdx[i]].dicCase = new Dictionary<string, StageBgDataItem>();
        }
    }
	private void SetCaseData(int _idx, string _case)
	{
		if(dic[_idx].dicCase.ContainsKey(_case) == false) 
			dic[_idx].dicCase.Add(_case, new StageBgDataItem());
	}
	private void SetCaseDataShow(int _idx, string _case, string _value)
	{
		SetCaseData(_idx, _case);
		dic[_idx].dicCase[_case].show = int.Parse(_value);
	}
	private void SetCaseDataCase(int _idx, string _case, string _value)
	{
		SetCaseData(_idx, _case);
		string[] values = _value.Split('|');
		dic[_idx].dicCase[_case].case_type = new int[values.Length];
		for(int j =0; j < values.Length; j++)
		{
			dic[_idx].dicCase[_case].case_type[j] = int.Parse(values[j]);
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
                case "width": dic[idx].width = int.Parse(_row[i]); break;
                case "height": dic[idx].height = int.Parse(_row[i]); break;
                case "tree_case": SetCaseDataCase(idx, "tree", _row[i]); break;
				case "tree_show": SetCaseDataShow(idx, "tree", _row[i]); break;
				case "flower_case": SetCaseDataCase(idx, "flower", _row[i]);break;
				case "flower_show": SetCaseDataShow(idx, "flower", _row[i]);break;
				case "bush_case": SetCaseDataCase(idx, "bush", _row[i]);break;
				case "bush_show":  SetCaseDataShow(idx, "bush", _row[i]);break;
				case "grass_case": SetCaseDataCase(idx, "grass", _row[i]);break;
				case "grass_show":  SetCaseDataShow(idx, "grass", _row[i]);break;

            }
        }
    }

}