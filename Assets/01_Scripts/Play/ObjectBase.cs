using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
	public enum TYPE 
	{
		Player,
		Enemy,
		BgOver,
		BgBack,
		Skill,
	}

	public GameObject go;
	public Transform tran;
	public RectTransform rt;

	public TYPE type = TYPE.Player;

    public CircleCollider2D col;
	public BoxCollider2D col_box;
	[HideInInspector]
	public bool is_right = true;
	private Vector3 beforePos;
	protected Vector3 prevPos;
	protected Vector2 vecX;
	protected Vector2 vecY;
	public virtual void Init()
	{

	}

	public void SetBeforePos(Vector3 _vec)
	{
		beforePos = _vec;
	}
	public virtual void UpdateObject() 
	{
		
	}

	private float GetHalf(HitObject _hitObj, bool _isX)
	{
		if(_hitObj.CurrentType == ObjectBase.TYPE.Enemy)
		{
			return _hitObj.enemy.col.radius;
		}
		if(_hitObj.CurrentType == ObjectBase.TYPE.BgOver)
		{
			return (_isX ? _hitObj.bgObject.col_box.size.x : _hitObj.bgObject.col_box.size.y) * 0.5f;
		}
		return 0f;
	}
	private float GetOffset(HitObject _hitObj, bool _isX)
	{
		if(_hitObj.CurrentType == ObjectBase.TYPE.Enemy)
		{
			return _isX ? 
					_hitObj.enemy.col.offset.x + _hitObj.enemy.rt.localPosition.x : 
					_hitObj.enemy.col.offset.y + _hitObj.enemy.rt.localPosition.y;
		}
		if(_hitObj.CurrentType == ObjectBase.TYPE.BgOver)
		{
			return _isX ? 
					_hitObj.bgObject.col_box.offset.x + _hitObj.bgObject.rt.localPosition.x : 
					_hitObj.bgObject.col_box.offset.y + _hitObj.bgObject.rt.localPosition.y;
		}
		return 0f;
	}


	protected void CheckBgObject(HitObject _hitObj)
	{
		prevPos= rt.localPosition;

		vecX = rt.localPosition;
		vecX.x = GetOffset(_hitObj, true);
		if(GetOffset(_hitObj, true) < rt.localPosition.x + col.offset.x)
			vecX.x += GetHalf(_hitObj,true) + col.radius;
		else
			vecX.x -= GetHalf(_hitObj,true) + col.radius;

		vecY = rt.localPosition;
		vecY.y = GetOffset(_hitObj, false);
		if(GetOffset(_hitObj, false) < rt.localPosition.y + col.offset.y)
			vecY.y += GetHalf(_hitObj, false) + col.radius;
		else
			vecY.y -= GetHalf(_hitObj, false) + col.radius;

		if(Vector3.Distance(prevPos,vecX) > Vector3.Distance(prevPos,vecY))
		{	//X??????????????? ????????? ??? ?????? ????????? ?????? x????????? ????????? ?????? ????????? y????????? ??????
			vecX.y = rt.localPosition.y;
			vecX.x -= col.offset.x;
			rt.localPosition = vecX;
		}  
		else
		{
			vecY.x = rt.localPosition.x;
			vecY.y -= col.offset.y;
			rt.localPosition = vecY;
		}

	}

}
