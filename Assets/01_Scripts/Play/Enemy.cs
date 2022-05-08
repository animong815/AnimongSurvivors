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
    private Vector3 vecBack;
    private bool is_right = true;
    private bool is_right_back = true;
    private float fnum;
    private float speed = 50f;
    private Collider2D hit;
    private HitObject hitObj;

    public override void Init() 
    {
        type = TYPE.Enemy;
        //Debug.Log( "canvas.scaleFactor :: " + PlayManager.ins.canvas.scaleFactor );
        //Debug.Log( "canvas.scaleFactor :: " + PlayManager.ins.canvas.referencePixelsPerUnit );
        //Debug.Log( "canvas.size :: " + PlayManager.ins.canvas.transform.localScale.x);
        //Debug.Log("canvasSize : " + PlayManager.ins.canvasSize);

        //goIdle.SetActive(true);
        //goRun.SetActive(false);
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
        MoveDirect();
        SetSize();
    }
    private void MoveDirect()
    {
        is_right_back = is_right;
        vecBack = 
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

        rt.localPosition = vec;

        //HitCheck();
        fnum = rt.localPosition.y;
        if(HitCheck() == false) return;
        //충돌했다면 x축으로만 이동
        vec = vecBack;        
        vec.x = rt.localPosition.x;
        rt.localPosition = vec;
        if(HitCheck() == false) return;
        //충돌 했다면 y축으로만 이동
        is_right = is_right_back;
        vec = vecBack;
        vec.y = fnum;
        rt.localPosition = vec;
        if(HitCheck() == false) return;        
        //그래도 충돌하면 본래 위치
        rt.localPosition = vecBack;
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
        hit = Physics2D.OverlapCircle(vec, col.radius * PlayManager.ins.canvasSize);
        col.enabled = true;
        //Debug.Log("hit : " + hit);
        if(hit != null)
        {
            hitObj = hit.GetComponent<HitObject>();
            if(hitObj != null)
            {
                switch(hitObj.CurrentType)
                { 
                    case HitObject.TYPE.Player:
                    Debug.Log("Hit Player");
                    break;
                    case HitObject.TYPE.Enemy:
                    
                    //hitObj.enemy.rt.
                    //vec = vecBack;
                    //vec.x += speed * Time.smoothDeltaTime * (vec.x < hitObj.enemy.rt.localPosition.x ? 1f : -1f) ;
                    //rt.localPosition = vec;
                    break;
                }
            }
            //Debug.Log("hit.name :: " +  hit.transform.gameObject.name);
            return true;
        }
        return false;

        //Collider.HitCheck();
        //Physics.Collections.
    }

}
