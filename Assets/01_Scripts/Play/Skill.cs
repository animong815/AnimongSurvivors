using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ObjectBase
{
    public RectTransform rtEff;
    public SkillDataItem data;
    private float endtime;
    public Animator ani;
    private Vector3 vec;
    private Collider2D[] hit;
    private bool is_collide;
    private int hnum;
    private HitObject hitObj;
    private Vector2 col_size;
    private float scale;
    public override void Init()
    {
        base.Init();
        type = TYPE.Skill;
        col_size = col_box.size;
    }

    public void SetData(SkillDataItem _data, ObjectBase _actor)
    {
        data = _data;
        endtime = Time.time + ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        rt.localScale = Vector3.one;
        scale = 1f;
        if(_data.value.ContainsKey(SkillData.size))
        {
            vec = Vector3.one;
            scale = (float)_data.value[SkillData.size] * 0.01f;
            vec.x *= scale;
            vec.y *= scale;
            rt.localScale = vec;            
            //col_box.size = col_size * vec;
        }

        vec = Vector3.one;
        if(_actor.is_right == false) vec.x = -1;
        rtEff.localScale = vec; 
        rt.position = _actor.rt.position;

        vec = rt.anchoredPosition;
        if(_actor.is_right) vec.x += rt.sizeDelta.x * 0.5f * scale;
        else vec.x -= rt.sizeDelta.x * 0.5f * scale;
        rt.anchoredPosition = vec;

        col_box.offset = Vector2.zero;
        //Debug.Log(ani.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    public override void UpdateObject()
    {
        base.UpdateObject();
        //Debug.Log("Update Skill");
        if(endtime < Time.time)
        {   //사라짐
            PlayManager.ins.stage.ReturnSkill(this);
        }
        HitCheck();
    }

    public bool HitCheck()
    {
        col_box.enabled = false;
        vec = rt.position;
        vec.x += col_box.offset.x * PlayManager.ins.canvasSize;     
        vec.y += col_box.offset.y * PlayManager.ins.canvasSize;
        hit = Physics2D.OverlapBoxAll(vec, col_box.size * PlayManager.ins.canvasSize * scale, 0f);
        col_box.enabled = true;
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
                        case ObjectBase.TYPE.Enemy:
                            PlayManager.ins.stage.ReturnEnemy(hitObj.enemy);
                            is_collide = true;
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
