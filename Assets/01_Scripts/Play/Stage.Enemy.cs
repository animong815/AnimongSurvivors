using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Stage
{
    public List<Enemy> enemies;

    private Dictionary<string, List<Enemy>> listEnemy;
    private Dictionary<string, int> keyEnemy;
    private Enemy tmpEnemy;

    //private float enemy_create_gap = 100f;
    //private float createTime = 0f;
    //private float createDelay = 3f;
    //private int createCount = 5;

    private void InitEnemy()
    {
        //createTime = Time.time;
        listEnemy = new Dictionary<string, List<Enemy>>();
        keyEnemy = new Dictionary<string, int>();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            listEnemy.Add(enemies[i].go.name, new List<Enemy>());   
            keyEnemy.Add(enemies[i].go.name, i); 
            enemies[i].go.SetActive(false);
        }
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
    }

    public void UpdateEnemy()
    {

    }
    private Enemy GetEnemy(int _idx) 
    {
        if(PlayManager.ins.data.enemy.dic.ContainsKey(_idx) == false)
        {
            Debug.LogWarning("enemy " + _idx + " not have");
            return null;
        }
        string key = PlayManager.ins.data.enemy.dic[_idx].prefab;
        if(keyEnemy.ContainsKey(key) == false)
        {
            Debug.LogWarning("enemy " + key + " not have prefab");
            return null;
        }

        for(int i = 0; i < listEnemy[key].Count; i++)
        {
            if(listEnemy[key][i].go.activeSelf == false)
            {
                tmpEnemy = listEnemy[key][i];
                listEnemy[key].Remove(tmpEnemy);
                tmpEnemy.rt.SetParent(rtGround);
                tmpEnemy.go.SetActive(true);
                tmpEnemy.SetData(PlayManager.ins.data.enemy.dic[_idx]);
                list.Add(tmpEnemy);
                listView.Add(tmpEnemy);
                return tmpEnemy;
            }
        }

        tmpEnemy = Instantiate<Enemy>(enemies[keyEnemy[key]], rtGround);
        tmpEnemy.rt.localScale = Vector3.one;
        tmpEnemy.rt.localRotation = Quaternion.identity;
        tmpEnemy.go.SetActive(true);
        tmpEnemy.idx = _idx;
        tmpEnemy.Init();
        tmpEnemy.SetData(PlayManager.ins.data.enemy.dic[_idx]);
        list.Add(tmpEnemy);
        listView.Add(tmpEnemy);
        return tmpEnemy;
    }
    /*
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
    */
}
