using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuide : MonoBehaviour
{
	public GameObject obj;
	public LineRenderer render;

	public int guideFrame;
	private Vector3[] pos;

	private float deltaTime;
	private Vector3 force;
	private int num;
	
	public void Init()
	{
		//���̵� ����Ʈ ���� ����
		pos = new Vector3[PlayManager.ins.data.guideLength];
		render.positionCount = pos.Length;
		guideFrame = PlayManager.ins.data.guideDetail;
		force = Vector3.zero;
	}
	/// <summary>
	/// ���̵� ��������, �������� ���� ��� 1������ ����� ������ �ʵ���.
	/// </summary>
	public void Show()
	{
		if (PlayManager.ins.stage.linePool.actLine == null) return;

		//deltaTime = Time.smoothDeltaTime;
		deltaTime = 0.018f;
		UpdateGuideLine(PlayManager.ins.player.tran.position - PlayManager.ins.stage.linePool.actLine.tran.position);
		obj.SetActive(true);
		
	}
	/// <summary>
	/// ���̵� ���� ����
	/// </summary>
	public void UpdateGuideLine(Vector2 vecLine)
	{
		if (PlayManager.ins.stage.linePool.actLine == null) return;

		pos[0] = PlayManager.ins.player.tran.position;
		pos[0].y += PlayManager.ins.stage.linePool.actLine.render.startWidth * 0.5f;

		PlayManager.ins.player.SetForceStart(ref vecLine, ref force);
		
		num = 1;
		int i = 0;

		//deltaTime = Time.smoothDeltaTime;
		while (true)
		{   //���̵� ���� ����
			pos[num] = pos[num - 1];
			for (i = 0; i < guideFrame; i++)
			{
				force.y -= PlayManager.ins.data.gravity * deltaTime;
				pos[num] += force * deltaTime;
			}
			num++;
			if (num >= pos.Length) break;
		}
		//pos[0].x = PlayManager.ins.player.tranPoint2.position.x;
		//pos[0].y = PlayManager.ins.player.tranPoint2.position.y;
		pos[0] = pos[1];

		render.SetPositions(pos);
	}

}
