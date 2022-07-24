using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Stage : MonoBehaviour
{
    public GameObject go;
    public RectTransform rt;
    public RectTransform rtBgObject;
    public RectTransform rtGround;
    public RectTransform rtEnemy;
    public Player player;
    public BgObjectList bgObject;

    private List<ObjectBase> list;
    private List<ObjectBase> listView;

    private int sorti;
    private Vector3 vec;

    private ObjectBase tmp;

    private bool is_init = false;

    private float enemy_create_gap = 100f;
    private float createTime = 0f;
    private float createDelay = 3f;
    private int createCount = 5;
    private int trycnt = 0;

    public void Init()
    {
        if (is_init == false)
        {
            list = new List<ObjectBase>();
            listView =  new List<ObjectBase>();

            InitEnemy();
            InitSkill();

            player.rt.SetParent(rtGround);
            player.Init();
            list.Add(player);
            
            SortStage();
            
            createTime = Time.time;
            bgObject.Init();

            is_init = true;
        }
        vec = Vector3.zero;
        vec.y = -160f;

        PlayManager.ins.ui.rtBgTile.anchoredPosition =
        rt.anchoredPosition = vec;

        ResetStage();
        
    }
    public void ResetStage()
    {
        List<ObjectBase> listDelete = new List<ObjectBase>();
        int i;
        for(i =0; i< listView.Count; i++)
        {
            listDelete.Add(listView[i]);
        }

        for(i =0; i< listDelete.Count; i++)
        {
            switch(listDelete[i].type)
            {
                case ObjectBase.TYPE.Enemy:
                    ReturnEnemy(listDelete[i] as Enemy);
                    break;
                case ObjectBase.TYPE.Skill:
                    ReturnSkill(listDelete[i] as Skill);
                    break;
            }
        }
        listDelete.Clear();
        listDelete = null;

        vec = Vector3.zero;
        vec.y = -160f;
        PlayManager.ins.ui.rtBgTile.anchoredPosition =
        rt.anchoredPosition = vec;

        vec.y = 50f;
        PlayManager.ins.player.rt.anchoredPosition = vec;

        PlayManager.ins.ui.txtTime.text = "00:00";
        PlayManager.ins.ui.imgStamina.fillAmount = 1f;

        bgObject.ResetBgObject();
    }

    public void StartStage()
    {
        PlayManager.ins.player.SetData();
        StartWave();
    }

    public void ReturnEnemy(Enemy _enemy) 
    {
        listView.Remove(_enemy);
        list.Remove(_enemy);
        listEnemy[_enemy.idx].Add(_enemy);
        _enemy.go.SetActive(false);
        _enemy.rt.SetParent(rtEnemy);
    }

    public void UpdateMove(Vector3 _vecMove)
    {
        //Debug.Log("_vecMove : " + _vecMove);
        _vecMove.z = 0;
        rt.localPosition -= _vecMove;
    }

    public void UpdateStage()
    {
        if (PlayManager.ins.is_play == false) return;
        if (Time.time > createTime) CreateEnemy();

        SortStage();
        UpdateWave();
    }

    public void AddList(ObjectBase _obj)
    {
        list.Add(_obj);
    }
    public void RemoveList(ObjectBase _obj)
    {
        list.Remove(_obj);
    }

    public void SortStage()
    {
        list.Sort((a, b) =>
        {
            if(a.rt.localPosition.y == b.rt.localPosition.y)
                return (a.rt.localPosition.x < b.rt.localPosition.x) ? 1 : -1;
            return (a.rt.localPosition.y < b.rt.localPosition.y) ? 1 : -1; 
        });
        
        for (sorti = 0; sorti < list.Count; sorti++)
        {
            list[sorti].rt.SetSiblingIndex(sorti);
            //if(listView.Count <= sorti) continue;
            //listView[sorti].UpdateObject();
        }

        for(sorti = 0; sorti < listView.Count; sorti++)
        {
            switch(listView[sorti].type)
            {
                case ObjectBase.TYPE.Enemy:
                case ObjectBase.TYPE.Skill:
                    listView[sorti].UpdateObject();
                    break;
            }
        }
    }
}
