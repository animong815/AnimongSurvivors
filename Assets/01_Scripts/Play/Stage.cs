using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject go;
    public RectTransform rt;
    public RectTransform rtBgObject;
    public RectTransform rtGround;
    public RectTransform rtEnemy;
    public Player player;
    public List<Enemy> enemies;
    public BgObjectList bgObject;

    private Dictionary<int, List<Enemy>> listEnemy;
    private List<ObjectBase> list;
    private List<ObjectBase> listView;

    private int sorti;
    private Vector3 vec;

    private ObjectBase tmp;
    private Enemy tmpEnemy;
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
            listEnemy = new Dictionary<int, List<Enemy>>();
            for(int i = 0; i< enemies.Count; i++) 
            {
                listEnemy.Add(i, new List<Enemy>());
            }

            player.rt.SetParent(rtGround);
            player.Init();

            list.Add(player);
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].go.SetActive(false);
            /*
            for (int i = 0; i < 300; i++)
            {
                tmpEnemy = Instantiate<Enemy>(enemies[0], rtGround);
                tmpEnemy.rt.localScale = Vector3.one;
                tmpEnemy.rt.localRotation = Quaternion.identity;
                vec.x = Random.Range(-1000, 1000);
                vec.y = Random.Range(-500, 500);
                tmpEnemy.rt.localPosition = vec;
                tmpEnemy.go.SetActive(true);
                tmpEnemy.go.name = "Mozzi_" + i;
                tmpEnemy.Init();
                list.Add(tmpEnemy);
            }
            */
            
            SortStage();
            
            createTime = Time.time;

            bgObject.Init();

            is_init = true;
        }
        vec = Vector3.zero;
        vec.y = -160f;

        PlayManager.ins.ui.rtBgTile.anchoredPosition =
        rt.anchoredPosition = vec;
        
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
            if(listDelete[i].type != ObjectBase.TYPE.Enemy) continue;
            ReturnEnemy(listDelete[i] as Enemy);
        }
        listDelete.Clear();
        listDelete = null;

        vec = Vector3.zero;
        vec.y = -160f;
        PlayManager.ins.ui.rtBgTile.anchoredPosition =
        rt.anchoredPosition = vec;

        vec.y = 50f;
        PlayManager.ins.player.rt.anchoredPosition = vec;

        bgObject.ResetBgObject();
    }

    public void ReturnEnemy(Enemy _enemy) 
    {
        listView.Remove(_enemy);
        list.Remove(_enemy);
        listEnemy[_enemy.idx].Add(_enemy);
        _enemy.go.SetActive(false);
        _enemy.rt.SetParent(rtEnemy);
    }

    private Enemy GetEnemy(int _idx) 
    {
        for(int i = 0; i < listEnemy[_idx].Count; i++)
        {
            if(listEnemy[_idx][i].go.activeSelf == false)
            {
                tmpEnemy = listEnemy[_idx][i];
                listEnemy[_idx].Remove(tmpEnemy);
                tmpEnemy.rt.SetParent(rtGround);
                tmpEnemy.go.SetActive(true);
                list.Add(tmpEnemy);
                listView.Add(tmpEnemy);
                return tmpEnemy;
            }
        }
        tmpEnemy = Instantiate<Enemy>(enemies[_idx], rtGround);
        tmpEnemy.rt.localScale = Vector3.one;
        tmpEnemy.rt.localRotation = Quaternion.identity;
        tmpEnemy.go.SetActive(true);
        tmpEnemy.go.name = $"Mozzi_{_idx}";
        tmpEnemy.idx = _idx;
        tmpEnemy.Init();
        list.Add(tmpEnemy);
        listView.Add(tmpEnemy);
        return tmpEnemy;
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
    }

    private void CreateEnemy() 
    {
        for (int i = 0; i < createCount; i++)
        {
            tmpEnemy = GetEnemy(Random.Range(0, enemies.Count));
            trycnt = 0;
            while(trycnt == 0 || (tmpEnemy.HitCheck() && trycnt < 30))
            {
                trycnt++;
                switch (Random.Range(0, 3)) 
                {
                    case 0:
                        vec.x = player.rt.localPosition.x + Random.Range((Screen.width * -0.5f) - enemy_create_gap, (Screen.width * 0.5f) + enemy_create_gap);
                        vec.y = player.rt.localPosition.y + ((Random.Range(0f, enemy_create_gap) + (Screen.height * 0.5f)) * (Random.Range(0, 2) == 0 ? 1f : -1f));
                        break;
                    case 1:
                        vec.x = player.rt.localPosition.x + ((Random.Range(0f, enemy_create_gap) + (Screen.width * 0.5f)) * (Random.Range(0, 2) == 0 ? 1f : -1f));
                        vec.y = player.rt.localPosition.y + Random.Range((Screen.height * -0.5f) - enemy_create_gap, (Screen.height * 0.5f) + enemy_create_gap);
                        break;
                    default:
                        vec.x = player.rt.localPosition.x + ((Random.Range(0f, enemy_create_gap) + (Screen.width * 0.5f)) * (Random.Range(0, 2) == 0 ? 1f : -1f));
                        vec.y = player.rt.localPosition.y + ((Random.Range(0f, enemy_create_gap) + (Screen.height * 0.5f)) * (Random.Range(0, 2) == 0 ? 1f : -1f));
                        break;
                }

                tmpEnemy.rt.localPosition = vec;
            }
        }
        createTime = Time.time + createDelay;
        //createTime = Time.time + 999999999999;
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
            if(listView[sorti].type != ObjectBase.TYPE.Enemy) continue;
            listView[sorti].UpdateObject();
        }
    }
}
