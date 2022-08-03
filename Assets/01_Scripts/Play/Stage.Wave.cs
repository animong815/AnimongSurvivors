using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Stage
{
    private DateTime startTime;
    private float fStart;
    private float fSec;
    private float nextTime;
    private List<WaveDataItem> listWave;
    private WaveDataItem tmpWave;
    private int trycnt = 0;
	private float timeGap;
    public void StartWave()
    {
        listWave = new List<WaveDataItem>();
        startTime = new DateTime();
        fStart = Time.time;
        fSec = 0f;
        nextTime = 0f;
		timeGap = 0f;
    }

    public void UpdateWave()
    {
        //Debug.Log(DateTime.Now - startTime);
        fSec = Time.time - fStart;
        //Debug.Log(fSec);
        PlayManager.ins.ui.txtTime.text = (startTime.AddSeconds(fSec)).ToString("mm':'ss");
        
        if(Time.time < nextTime) return;
        listWave.Clear();
        for(int i =0; i< PlayManager.ins.data.wave.list.Count; i++)
        {
            if(PlayManager.ins.data.wave.list[i] == null) continue;
            if(PlayManager.ins.data.wave.list[i].checkTime[0] < fSec 
            && PlayManager.ins.data.wave.list[i].checkTime[1] > fSec)
            {
                listWave.Add(PlayManager.ins.data.wave.list[i]);
            }
        }
		//Debug.Log("listWave.Count:" + listWave.Count);
		if(listWave.Count < 1)
		{ 
			nextTime = Time.time + timeGap;
			return;
		}
        tmpWave = listWave[UnityEngine.Random.Range(0, listWave.Count)];

        CreateWaveEnemy();

		timeGap = tmpWave.createTime;
        nextTime = Time.time + timeGap;
        //Debug.Log("CreateEnemy : " + tmpWave.id);
        //Debug.Log("NextWaveTime : " + nextTime + "/" + Time.time);
    }

    private void CreateWaveEnemy() 
    {
        for (int i = 0; i < tmpWave.count; i++)
        {
            tmpEnemy = GetEnemy(tmpWave.enemys[UnityEngine.Random.Range(0, tmpWave.enemys.Length)]);
            trycnt = 0;
            while(trycnt == 0 || (tmpEnemy.HitCheck() && trycnt < 30))
            {
                trycnt++;
                vec = Vector3.one;
                switch (UnityEngine.Random.Range(0, 4)) 
                {
                    case 0: vec.x *= -1; break;
                    case 1: vec.y *= -1; break;
                    case 2: vec.x *= -1; vec.y *= -1;break;
                }
                vec.x = player.rt.localPosition.x + (UnityEngine.Random.Range(tmpWave.position_range[0], tmpWave.position_range[1]) * vec.x);
                vec.y = player.rt.localPosition.y + (UnityEngine.Random.Range(tmpWave.position_range[0], tmpWave.position_range[1]) * vec.y);
                vec.z = 0;
                tmpEnemy.rt.localPosition = vec;
            }
        }
        
    }
}