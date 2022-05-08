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
	private Vector3 force;
	private Vector3 vecMove;

	private Collider[] colHit;
	private int num;

	private Vector3 prevPos;
	
	public float speed = 70f;

	public void Init(bool isRestart = false)
	{
		base.Init();
		prevPos = vec;
		type = TYPE.Player;
	}

	public void UpdatePlayer()
	{
		/*
		force.y -= PlayManager.ins.data.gravity * Time.smoothDeltaTime;
		vecMove = tran.position;
		vecMove += force * Time.smoothDeltaTime;
		tran.position = vecMove;

		colHit = Physics.OverlapCapsule(tranPoint1.position, tranPoint2.position, 1f, PlayManager.ins.stage.layerCoin);
		if (colHit != null)
		{
			for (num = 0; num < colHit.Length; num++)
			{
				if (colHit[num] == null || colHit[num].gameObject == null) continue;
				coin = colHit[num].GetComponent<PlayCoin>();
				if (coin == null) continue;
				coin.obj.SetActive(false);
				PlayManager.ins.ui.AddCoin();
			}
		}
		*/
		prevPos = tran.position;
		PlayManager.ins.ui.UpdateDistance();
	}

    public override void UpdateObject()
    {
        base.UpdateObject();

    }
}
