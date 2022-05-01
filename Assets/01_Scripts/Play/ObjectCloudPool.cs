using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCloudPool : ObjectBasePool
{
    public const float MAX_POSITION = 450;

    public int GAP = 100;
    public float ForceRate = 1f;
    public int RandomRate = 50;

    private float LINE_GAP;
    private float CLOUD_START;

    private float screenW;
    private float screenH;
    private int curIdx;
    public ObjectCloud prefab;
    private List<ObjectCloud> list;
    public List<ObjectCloud> view;

    private ObjectCloud cloud;
    private int n;
    private Vector3 vec;

    private bool is_init;

    public void Init() 
    {

        is_init = true;
        screenW = Screen.width / PlayManager.ins.canvasB.scaleFactor;
        screenH = Screen.height / PlayManager.ins.canvasB.scaleFactor;
        list = new List<ObjectCloud>();
        view = new List<ObjectCloud>();
        InitCloud();
        CLOUD_START = 0f;
        LINE_GAP = 0f;
    }

    public void InitCloud(bool isStartY = false)
    {
        is_init = true;
        curIdx = 0;
        prefab.go.SetActive(false);
        ReturnViewAll();
    }

    private void CheckCloud() 
    {
        if (is_init)
        {
            is_init = false;
        }
    }

    public void UpdateCloud()
    {
        CheckCloud();
        //화면 아래로 내려간 내용 안보이도록
        for (n = 0; n < view.Count; n++)
        {
            if (tran.localPosition.y + view[n].tran.localPosition.y < screenH * -0.5f)
            {
                view[n].go.SetActive(false);
                list.Add(view[n]);
                view.RemoveAt(n);
                n--;
            }
        }
        
        //바람 방향에 의한 이동
        for (n = 0; n < view.Count; n++)
        {
            vec = view[n].tran.localPosition;
            //vec.x += wind_force * Time.smoothDeltaTime;
            vec.x += view[n].force * Time.smoothDeltaTime;

            if (view[n].force > 0 && vec.x >= MAX_POSITION)
            {
                vec.x = -MAX_POSITION;
            }
            else if(view[n].force < 0 && vec.x <= -MAX_POSITION)
            {
                vec.x = MAX_POSITION;
            }

            view[n].tran.localPosition = vec;
        }
    }

    private ObjectCloud CreateCloud() 
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].go.activeSelf == false)
            {
                cloud = list[i];
                list.RemoveAt(i);
                view.Add(cloud);
                return cloud;
            }
        }
        cloud = GameObject.Instantiate<ObjectCloud>(prefab);
        cloud.tran.SetParent(tran);
        cloud.tran.localScale = Vector3.one;
        cloud.tran.localRotation = Quaternion.identity;
        cloud.tran.localPosition = Vector3.zero;

        view.Add(cloud);

        return cloud;
    }

    public void ReturnCloud(ObjectCloud item)
    {
        list.Add(item);
        list[list.Count - 1].go.SetActive(false);
        list[list.Count - 1].Init();
        item.tran.SetParent(tran);

        view.Remove(item);
    }

    public void ReturnViewAll() 
    {
        int cnt = view.Count;
        for (int i = 0; i < cnt; i++)
        {
            ReturnCloud(view[0]);
        }
    }
}
