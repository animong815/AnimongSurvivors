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
        prefab.obj.SetActive(false);
        ReturnViewAll();
    }

    private void CheckCloud() 
    {
        if (is_init)
        {
            PlayManager.ins.data.SetTotalWind();

            if (LINE_GAP == 0)
            {
                Vector3 vec = Vector3.zero;
                vec.y = PlayManager.ins.data.lineGap;
                vec = PlayManager.ins.cam3d.WorldToScreenPoint(vec);
                vec.y -= PlayManager.ins.cam2dB.pixelHeight * 0.5f;
                vec.y /= PlayManager.ins.canvasB.scaleFactor;
                LINE_GAP = vec.y;
                //Debug.Log("LINE_GAP::" + LINE_GAP);
            }
            is_init = false;
        }
        vec = PlayManager.ins.GetBack3Dto2D(PlayManager.ins.stage.ground.tranScroll.position);
        vec.y = vec.y - (screenH * 0.5f);
        tran.localPosition = vec;

        int lineIdx;
        float createPos;
        //화면에 보이는 구름 생성
        while ((curIdx * GAP) + vec.y < screenH * 0.5f)
        {
            //if ( (curIdx % 2 == 0 && Random.Range(0, 100) < 50) //작은 구름 안만듬
            //    || (curIdx % 2 != 0 && Random.Range(0, 100) < 20) //큰 구름 안만듬
            //    )
            if(Random.Range(0, 100) > RandomRate)
            {   
                curIdx++;
                continue;
            }

            cloud = CreateCloud();
            cloud.obj.SetActive(true);
            //cloud.obj1.SetActive(curIdx % 2 != 0);
            //cloud.obj2.SetActive(curIdx % 2 == 0);

            vec = Vector3.zero;
            vec.x = Random.Range(0f, screenW) - (screenW * 0.5f);
            vec.y += (curIdx * GAP);
            cloud.tran.localPosition = vec;
            
            if (CLOUD_START == 0)
            {
                createPos = tran.localPosition.y + vec.y;
                CLOUD_START = tran.localPosition.y;
            }
            else createPos = CLOUD_START + vec.y;
            
            if (createPos < 0) createPos = 0;
            lineIdx = Mathf.RoundToInt(createPos / LINE_GAP);

            //Debug.Log("curIdx:"+ curIdx +",CreatePos:"+ cloud.CreatePos + ",lineIdx:" + lineIdx);
            //cloud.CreatePos = createPos;
            //cloud.lineIdx = lineIdx;
            //방향 설정
            cloud.force = 0f;
            for (int i = 0; i < PlayManager.ins.data.totalWind.Count; i++)
            {
                if (lineIdx >= PlayManager.ins.data.totalWind[i][0])
                {
                    cloud.force = PlayManager.ins.data.totalWind[i][1] * ForceRate;
                    //Debug.Log("***" + cloud.force);
                    //if (PlayManager.ins.data.totalWind[i].Length < 3) cloud.force = PlayManager.ins.data.totalWind[i][1];
                    //else cloud.force = Random.Range( PlayManager.ins.data.totalWind[i][1], cloud.force = PlayManager.ins.data.totalWind[i][2]);

                    break;
                }
            }
            cloud.force += 5;
            vec = Vector3.one;
            vec.x = cloud.force > 0 ? -1f : 1f;
            cloud.rectImage.localScale = vec;
           
            //cloud.obj1.transform.localScale = vec;
            //cloud.obj2.transform.localScale = vec;

            curIdx++;
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
                view[n].obj.SetActive(false);
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
            if (list[i].obj.activeSelf == false)
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
        list[list.Count - 1].obj.SetActive(false);
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
