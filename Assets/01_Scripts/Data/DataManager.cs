using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
	public static DataManager ins;

	public PopupLoaing loading;
	public GameObject go;

	private bool is_load;
	private int load_cnt;
	private bool is_first_load;
	private bool is_restart;


	public GlobalData global;
	public SkillData skill;
	public UserData user;
	public WaveData wave;
	public EnemyData enemy;
	public StageData stage;
	private List<DataBase> listData;

	public void Awake()
	{
		if (DataManager.ins != null && DataManager.ins != this)
		{
			Destroy(gameObject);
			return;
		}

		DataManager.ins = this;
		DontDestroyOnLoad(gameObject);

		load_cnt = 0;
		is_load = false;
		is_first_load = false;

		global = go.AddComponent<GlobalData>();
		user = go.AddComponent<UserData>();
		enemy = go.AddComponent<EnemyData>();
		skill = go.AddComponent<SkillData>();
		wave = go.AddComponent<WaveData>();
		stage = go.AddComponent<StageData>();

		listData= new List<DataBase>();
		listData.Add(global);
		listData.Add(user);
		listData.Add(enemy);
		listData.Add(skill);
		listData.Add(wave);
		listData.Add(stage);
	}

	public void LoadFirstLoad()
	{
		if (is_first_load == true)
			return;
		LoadData();
	}

	public void LoadData(bool restart = false)
	{
		if (is_load == true) return;
		
		is_restart = restart;

		loading.obj.SetActive(true);
		UnityWebRequest.ClearCookieCache();
		is_load = true;
		load_cnt = 0;
		
		for(int i =0; i < listData.Count; i++)
		{
			listData[i].Init();
		}
	}

	public void CheckLoad()
	{
		load_cnt++;

		if (load_cnt < listData.Count) return;

		loading.obj.SetActive(false);
		is_load = false;
		is_first_load = true;

		//Debug.Log("*** " + user.dic[2].value.ContainsKey(SkillData.move_speed));
		//Debug.Log("*** " + user.dic[2].value[SkillData.move_speed]);
	

		if (PlayManager.ins != null) 
		{
			PlayManager.ins.Init(false);
			PlayManager.ins.stage.bgObject.Init();
		}
	}
}
