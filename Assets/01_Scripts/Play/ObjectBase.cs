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
		BgBack
	}

	public GameObject go;
	public Transform tran;
	public RectTransform rt;

	public TYPE type = TYPE.Player;

    public CircleCollider2D col;
	public BoxCollider2D col_box;

	protected Vector3 prevPos;
	protected Vector2 vecX;
	protected Vector2 vecY;
	public virtual void Init()
	{

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

		if(Vector3.Distance(prevPos,vecX) < Vector3.Distance(prevPos,vecY))
		{	//X축기준으로 이동이 더 적게 이동한 경우 x축으로 충돌체 끝에 위치후 y이동만 적용
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
