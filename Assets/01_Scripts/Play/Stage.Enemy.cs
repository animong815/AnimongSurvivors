using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Stage
{
    public List<Enemy> enemies;

    private Dictionary<int, List<Enemy>> listEnemy;
    private Enemy tmpEnemy;

    private void InitEnemy()
    {
        listEnemy = new Dictionary<int, List<Enemy>>();
        for(int i = 0; i< enemies.Count; i++) 
        {
            listEnemy.Add(i, new List<Enemy>());
        }
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
    }

    public void UpdateEnemy()
    {

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
}
