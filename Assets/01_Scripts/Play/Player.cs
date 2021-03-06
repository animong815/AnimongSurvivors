using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : ObjectBase
{
	public int idx;
	public int level;

	private UserDataItem data;

	public GameObject goIdle;
	public Transform tfIdle;
	public GameObject goRun;
	public Transform tfRun;

	private Vector3 vec;

	private Collider[] colHit;
	private int num;
	private bool useStamina;
	[HideInInspector]
	public float speed = 70f;
	private float stamina = 0f;
	private float groggy = 0f;
	private float beforeTime = 0f;
	private Collider2D[] hit;
	private HitObject hitObj;

	private Vector2 vecBack;
	private int h;

	public void Init(bool isRestart = false)
	{
		base.Init();
		prevPos = vec;
		type = TYPE.Player;
		is_right = true;
	}
	public void SetUseStamina(bool _use)
	{
		useStamina = _use;
	}
	public void SetData()
	{
		data = new UserDataItem();
		data.id = idx;
		data.level = level;
		data.skill = PlayManager.ins.data.user.dic[idx][level].skill;
		data.prefab = PlayManager.ins.data.user.dic[idx][level].prefab;
		data.value = new Dictionary<string, int>();
		data.values = new Dictionary<string, int[]>();
		foreach(string key in PlayManager.ins.data.user.dic[idx][level].value.Keys)
		{
			data.value.Add(key, PlayManager.ins.data.user.dic[idx][level].value[key]);
		}
		foreach(string key in PlayManager.ins.data.user.dic[idx][level].values.Keys)
		{
			data.values.Add(key, PlayManager.ins.data.user.dic[idx][level].values[key]);
		}

		useStamina = false;
		speed = data.value[SkillData.move_speed];
		stamina = data.value[SkillData.stamina_max];		
		groggy = 0;
		beforeTime = Time.time;

		SetSkillData();
	}

	public void MoveUpdate(Vector3 _vecMove)
	{
		vecBack = rt.localPosition;
		rt.localPosition += _vecMove;
		prevPos = rt.localPosition;		
		HitCheck();
	}

    public override void UpdateObject()
    {
        base.UpdateObject();
		if(PlayManager.ins.is_play == false) return;
		UpdateSkill();

		//PlayManager.ins.ui.UpdateDistance();
		//Debug.Log("Player.UpdateObject()");
		PlayManager.ins.ui.imgStamina.color = Color.white;
		if(groggy != 0)
		{//지친 상태
			PlayManager.ins.ui.imgStamina.color = Color.red;
			if(stamina >= groggy) groggy = 0;
			useStamina = false;
		}

		if(useStamina)
		{
			speed = data.value[SkillData.move_speed_stamina];
			//스테미너 줄임
			stamina -= (float)data.value[SkillData.stamina_use] * (Time.time - beforeTime);
			if(stamina <= 0) 
			{	//스테미나 모두 사용
				stamina = 0;
				groggy = data.value[SkillData.groggy_value];
			}
		}
		else
		{	//걷는 상태 스테미나 증가
			speed = data.value[SkillData.move_speed];
			stamina += (float)data.value[SkillData.stamina_add] * (Time.time - beforeTime);
			if(stamina > data.value[SkillData.stamina_max])
				stamina = data.value[SkillData.stamina_max];
		}
			
		//스테미너 수치 업데이트
		PlayManager.ins.ui.imgStamina.fillAmount =  stamina / (float)data.value[SkillData.stamina_max];
		beforeTime = Time.time;		
    }

	public Vector3 GetGap()
	{
		return	rt.localPosition - prevPos;
	}

	public void HitCheck()
    {
        vec = rt.position;
        vec.y += col.offset.y * PlayManager.ins.canvasSize;        
        hit = Physics2D.OverlapCircleAll(vec, col.radius * PlayManager.ins.canvasSize);

		if(hit == null) return;

		for(h = 0; h < hit.Length; h++)
		{
			if(hit[h] == null) continue;
			hitObj = hit[h].GetComponent<HitObject>();

			if(hitObj == null) continue;
			
			switch(hitObj.CurrentType)
			{ 
				case ObjectBase.TYPE.Enemy:
				/*
				if(hitObj.enemy.rt.localPosition.x < rt.localPosition.x)
				{
					vec = rt.localPosition;
					vec.x = hitObj.enemy.rt.localPosition.x;
					vec.x += hitObj.enemy.col.radius + col.radius;
					rt.localPosition = vec;
				}
				else
				{
					vec = rt.localPosition;
					vec.x = hitObj.enemy.rt.localPosition.x;
					vec.x -= hitObj.enemy.col.radius + col.radius;
					rt.localPosition = vec;
				}
				*/
				break;           
				case ObjectBase.TYPE.BgOver:
					CheckBgObject(hitObj);
					break;          
				default: break;
			}
		}
	}

}
