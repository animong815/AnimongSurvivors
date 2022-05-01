using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public RectTransform rt;
    public RectTransform rtGround;
    public RectTransform rtEnemy;
    public Player player;
    public List<Enemy> enemies;

    private Dictionary<int, List<Enemy>> listEnemy;
    private List<ObjectBase> list;

    private int sorti;
    private Vector3 vec;

    private ObjectBase tmp;
    private Enemy tmpEnemy;
    private bool is_init = false;

    private float enemy_create_gap = 100f;
    private float createTime = 0f;
    private float createDelay = 3f;
    public void Init()
    {
        if (is_init == false)
        {
            list = new List<ObjectBase>();
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
            
            createTime = Time.time + createDelay;
            is_init = true;
        }
    }

    public void ReturnEnemy(Enemy _enemy) 
    {
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

        return tmpEnemy;
    }
    public void UpdateMove(Vector3 _vecMove)
    {
        //Debug.Log("_vecMove : " + _vecMove);
        _vecMove.z = 0;
        rt.localPosition -= _vecMove;
        player.tran.localPosition += _vecMove;
    }
    public void UpdateStage()
    {
        if (PlayManager.ins.is_play == false) return;
        if (Time.time > createTime) CreateEnemy();
        SortStage();
    }

    private void CreateEnemy() 
    {
        for (int i = 0; i < 5; i++)
        {
            tmpEnemy = GetEnemy(0);
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
            createTime = Time.time + createDelay;
        }
    }

    private void SortStage()
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
            list[sorti].UpdateObject();
        }
    }
}
