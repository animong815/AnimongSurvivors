using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Stage
{
    private DateTime startTime;
    private float fStart;
    private float fSec;
    public void StartWave()
    {
        startTime = new DateTime();
        fStart = Time.time;
        fSec = 0f;
    }

    public void UpdateWave()
    {
        //Debug.Log(DateTime.Now - startTime);
        fSec = Time.time - fStart;
        //Debug.Log(fSec);
        PlayManager.ins.ui.txtTime.text = (startTime.AddSeconds(fSec)).ToString("mm':'ss");
        
    }
}