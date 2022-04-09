using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCloudPoolGroup : MonoBehaviour
{
    public ObjectCloudPool[] pool;

    public void Init()
    {
        for (int i = 0; i < pool.Length; i++) pool[i].Init();
    }

    public void InitCloud(bool isStartY = false)
    {
        for (int i = 0; i < pool.Length; i++) pool[i].InitCloud(isStartY);
    }

    public float CheckMoveX()
    {
        //Debug.Log("player line pos ::" + Mathf.RoundToInt(PlayManager.ins.player.tran.localPosition.y / PlayManager.ins.data.lineGap));
        int lineIdx = Mathf.RoundToInt(PlayManager.ins.player.tran.localPosition.y / PlayManager.ins.data.lineGap);
        for (int i = 0; i < PlayManager.ins.data.totalWind.Count; i++)
        {
            if (lineIdx >= PlayManager.ins.data.totalWind[i][0])
            {
                return PlayManager.ins.data.totalWind[i][1] * PlayManager.ins.data.totalWindRate * Time.smoothDeltaTime;
            }
        }
        return 0f;
    }

    public void UpdateCloud()
    {
        for (int i = 0; i < pool.Length; i++) pool[i].UpdateCloud();
    }
}
