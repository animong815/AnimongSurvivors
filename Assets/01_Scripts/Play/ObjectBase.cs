using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
	public enum TYPE 
	{
		Player,
		Enemy
	}

	public GameObject go;
	public Transform tran;
	public RectTransform rt;

	public TYPE type = TYPE.Player;

    public CircleCollider2D col;

	public virtual void Init()
	{

	}
	public virtual void UpdateObject() 
	{
		
	}
}
