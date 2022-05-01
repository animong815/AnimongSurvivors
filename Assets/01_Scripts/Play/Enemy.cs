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

    private Vector3 vec;
    private bool is_right = true;
    private float fnum;
    public override void Init() 
    {
        type = TYPE.Enemy;
        //goIdle.SetActive(true);
        //goRun.SetActive(false);
    }
    public override void UpdateObject()
    {
        base.UpdateObject();

        vec = rt.localPosition;

        fnum = PlayManager.ins.stage.player.rt.localPosition.x - rt.localPosition.x;

        if (fnum > 0)
        {
            is_right = true;
            vec.x += 1f;
        }
        if (fnum < -1)
        {
            is_right = false;
            vec.x -= 1f;
        }


        if (PlayManager.ins.stage.player.rt.localPosition.y > rt.localPosition.y) vec.y += 1f;
        else vec.y -= 1f;
        rt.localPosition = vec;

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

        if (Vector3.Distance(rt.localPosition, PlayManager.ins.stage.player.rt.localPosition) > 1500f) PlayManager.ins.stage.ReturnEnemy(this);

    }
}
