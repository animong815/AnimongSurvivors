using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ObjectBase
{
	public GameObject goIdle;
	public Transform tfIdle;
	public GameObject goRun;
	public Transform tfRun;

	private Vector3 vec;

	private Collider[] colHit;
	private int num;

	public float speed = 70f;
	private Collider2D[] hit;
	private HitObject hitObj;

	private Vector2 vecBack;
	private int h;

	public void Init(bool isRestart = false)
	{
		base.Init();
		prevPos = vec;
		type = TYPE.Player;
	}
	public void MoveUpdate(Vector3 _vecMove)
	{
		vecBack = rt.localPosition;
		rt.localPosition += _vecMove;
		prevPos = rt.localPosition;
		UpdateObject();
	}

    public override void UpdateObject()
    {
        base.UpdateObject();
		
		//PlayManager.ins.ui.UpdateDistance();
		//Debug.Log("Player.UpdateObject()");

		HitCheck();
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
