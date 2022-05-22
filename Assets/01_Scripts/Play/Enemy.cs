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
    private Collider2D[] hit;
    private int hnum;
    private HitObject hitObj;
    private bool is_collide;

    private const string CHECK_NAME_PLAYER = "Player";
    private const string CHECK_NAME_BG = "Bg";

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
