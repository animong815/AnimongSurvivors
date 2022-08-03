using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ObjectBase
{
    public int idx;
    public RectTransform rtIdle;
    public GameObject goIdle;
    public RectTransform rtRun;
    public GameObject goRun;
    public UnityEngine.UI.Image img;
	public Animator ani;


    private Vector3 vec;
    private Vector3 vecBack;
    private bool is_right_back = true;
    private float fnum;
    private float speed = 50f;
    private float hp = 0f;
    private Collider2D[] hit;
    private int hnum;
    private HitObject hitObj;
    private bool is_collide;
    private float lifeTime = 0f;
    private const string CHECK_NAME_PLAYER = "Player";
    private const string CHECK_NAME_BG = "Bg";

	private float time_die;
	private float time_damage;
	private float time_stun;
	private SkillDataItem skill;
	private Vector2 vecDir;

    [HideInInspector]
    public EnemyDataItem data;

    public override void Init() 
    {
        base.Init();
        type = TYPE.Enemy;
        //Debug.Log( "canvas.scaleFactor :: " + PlayManager.ins.canvas.scaleFactor );
        //Debug.Log( "canvas.scaleFactor :: " + PlayManager.ins.canvas.referencePixelsPerUnit );
        //Debug.Log( "canvas.size :: " + PlayManager.ins.canvas.transform.localScale.x);
        //Debug.Log("canvasSize : " + PlayManager.ins.canvasSize);

        //goIdle.SetActive(true);
        //goRun.SetActive(false);
    }

    public void SetData(EnemyDataItem _data)
    {
		vecDir = Vector2.zero;
        data = _data;
        speed = data.speed;
        hp = data.hp;
        lifeTime = (_data.life_time < 0) ? -1f : Time.time + _data.life_time;
		InitMove();
		img.color = Color.white;
        if(img != null)
        {
			img.color = _data.color;             
        }
    }
    
    public void Damage(SkillDataItem _data)
    {
		if(hp <= 0) return;
		skill = _data;
        hp -= skill.value[SkillData.attack];

		time_stun = 0f;
		time_damage = 0f;

		time_damage = Time.time + skill.GetValueTime(SkillData.back_time); 
		if(ani != null)
		{
			ani.Play("damage");
			if(Time.time + ani.GetCurrentAnimatorClipInfo(0)[0].clip.length > time_damage)
				time_damage = Time.time + ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			
			if(hp <= 0)
			{
				time_die = -1;	
			}
			return;
		}

        if(hp <= 0)
        {
            PlayManager.ins.stage.ReturnEnemy(this);
        }

    }
	private void InitMove()
	{
		time_die = 0f;
		time_damage = 0f;
		time_stun = 0f;
		skill = null;
		ani.Play("run");
	}

    public override void UpdateObject()
    {
        base.UpdateObject();
        
		if (Vector3.Distance(rt.localPosition, PlayManager.ins.stage.player.rt.localPosition) > 1500f)
        {   //플레이어와 일정 거리 이상 멀어지면 제거 
            PlayManager.ins.stage.ReturnEnemy(this);
            return;
        }
        if(PlayManager.ins.is_play == false) return;

		//데미지 입고 있는 중
		if(time_damage > 0)
		{
			if(skill.GetValue(SkillData.back_speed) != 0)
			{
				
			}

			if(Time.time >= time_damage)
			{
				time_damage = 0;
				if(time_die == -1)
				{
					ani.Play("die");
					time_die = Time.time + ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
					return;
				}

				if(skill.GetValue(SkillData.stun_time) != 0)
				{	//경직 시간
					time_stun = Time.time + skill.GetValueTime(SkillData.stun_time);
					return;
				}
				InitMove();
			}			
			return;
		}

		if(time_stun > 0)
		{	//경직
			if(Time.time > time_stun)
			{
				InitMove();
			}
			return;
		}
		//사라지는 중
		if(time_die > 0) 
		{
			if(time_die < Time.time)
			{
				PlayManager.ins.stage.ReturnEnemy(this);
			}
			return;
		}

        MoveDirect();
        SetSize();
        if(lifeTime != -1f && lifeTime < Time.time) PlayManager.ins.stage.ReturnEnemy(this);
    }
	private void SetDirect()
	{

	}
    private void MoveDirect()
    {
        is_right_back = is_right;
        //vecBack = 
        vec = rt.localPosition;

        fnum = PlayManager.ins.stage.player.rt.localPosition.x - rt.localPosition.x;

        if (fnum > 0)
        {
            is_right = true;
            vec.x += speed * Time.smoothDeltaTime;
        }
        if (fnum < -speed * Time.smoothDeltaTime)
        {
            is_right = false;
            vec.x -= speed * Time.smoothDeltaTime;
        }

        if (PlayManager.ins.stage.player.rt.localPosition.y > rt.localPosition.y) vec.y += speed * Time.smoothDeltaTime;
        else vec.y -= speed * Time.smoothDeltaTime;

        prevPos = 
        //vecBack =
        rt.localPosition = vec;
        
        if(HitCheck() == false) return;
        //충돌했다면 x축으로만 이동
        //vec = rt.localPosition;
        //vec.x = vecBack.x;
        //rt.localPosition = vec;
        //if(HitCheck() == false) return;
        //충돌 했다면 y축으로만 이동
        vec = rt.localPosition;
        vec.y = vecBack.y;
        rt.localPosition = vec;
        if(HitCheck() == false) return;
        //그래도 충돌하면 밀린 위치
    }

    private void SetSize()
    {
        if(is_right) 
        {
            if (rtRun.localScale.x > 0f) 
            {
                vec = Vector3.one;
                vec.x = -1;
                rtRun.localScale = vec;
            }
        }
        else 
        {
            if (rtRun.localScale.x < 0f)
            {
                rtRun.localScale = Vector3.one;
            }
        }

    }
    public bool HitCheck()
    {
        col.enabled = false;
        vec = rt.position;
        vec.y += col.offset.y * PlayManager.ins.canvasSize;        
        hit = Physics2D.OverlapCircleAll(vec, col.radius * PlayManager.ins.canvasSize);
        col.enabled = true;
        //Debug.Log("hit : " + hit);
        is_collide = false;
        if(hit != null)
        {
            for(hnum = 0; hnum < hit.Length; hnum++)
            {
                if(hit[hnum] == null) continue;
                hitObj = hit[hnum].GetComponent<HitObject>();

                if(hitObj != null)
                {
                    switch(hitObj.CurrentType)
                    { 
                        case ObjectBase.TYPE.Player:
                        //Debug.Log("Hit Player");
                        PlayManager.ins.GameOver();
                        is_collide = true;
                        break;
                        case ObjectBase.TYPE.Enemy:
                        /*
                        is_collide = true;   
                        //몬스터와 충돌한 경우 몬서의 뒤로 이동
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
                        break;           
                        */
                        case ObjectBase.TYPE.BgOver:
                        //몬스터 배경 오브젝트와 충돌한 경우
                        is_collide = false;
                        CheckBgObject(hitObj);
                        
                        break;          
                        default: break;
                    }
                }
            }
            //Debug.Log("hit.name :: " +  hit.transform.gameObject.name);
        }
        return is_collide;

        //Collider.HitCheck();
        //Physics.Collections.
    }

}
