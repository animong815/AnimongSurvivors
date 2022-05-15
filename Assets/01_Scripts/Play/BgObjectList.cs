using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgObjectList : MonoBehaviour
{
    public RectTransform rt;
    
    public BgObject[] prefabObject;

    private Dictionary<int, List<BgObject>> dicObject;
    private BgObject tmp;
    private List<BgObject> listView;
    private List<BgObject> listDelete; 
    
    private const float TILE_SIZE = PlayUI.TILE_SIZE * 0.5f;
    private const float TILE_SIZE_HALF = PlayUI.TILE_SIZE * 0.25f;
    private const int VIEW_W = 20;
    private const int VIEW_H = 12;
    private int loopI;
    private Vector3 vec;
    private float CHECK_W;
    private float CHECK_H;
    private Vector2 vecPos;
    
    public void Init()
    {        
        listView = new List<BgObject>();
        listDelete = new List<BgObject>();
        dicObject = new Dictionary<int, List<BgObject>>();
        for(int i = 0; i<prefabObject.Length; i++)
        {
            prefabObject[i].go.SetActive(false);
            dicObject.Add(prefabObject[i].idx, new List<BgObject>());     
        }

        CHECK_W = VIEW_W * 0.5F * TILE_SIZE;
        CHECK_H = VIEW_H * 0.5F * TILE_SIZE;

        ResetBgObject();
    }

    public void ResetBgObject()
    {
        DeleteObject(true);        

        vecPos = Vector2.zero;
        
        CreateObject(0,0,VIEW_W *2, VIEW_H *2);
        PlayManager.ins.stage.SortStage();
    }

    private void CreateObject(int _startw, int _starth, int _maxw, int _maxh)
    {
        //Debug.Log("CreateObject");
        //Debug.Log("CheckBgObject");
        int w, h;
        for(w = _startw; w < _maxw; w++)
        {
            for(h = _starth; h < _maxh; h++)
            {
                CreateBgObject(w,h);                
            }
        }
        //Debug.Log("bgObject.Count : " + listView.Count);
    }
    
    private void CreateBgObject(int _x, int _y)
    {
        //생성 확률
        if(Random.Range(0,2) == 0) return;

        tmp = GetObject(Random.Range(0, prefabObject.Length) + 1);
        vec.x = ((_x - (VIEW_W)) * TILE_SIZE) + TILE_SIZE_HALF - vecPos.x;
        vec.y = ((_y - (VIEW_H)) * TILE_SIZE) + TILE_SIZE_HALF - vecPos.y;
        vec.z = 0f;
        tmp.rt.localPosition = vec;
    }

    private void DeleteObject(bool is_all = false)
    {
        //Debug.Log("DeleteObject");
        int i;
        //Debug.Log("-CHECK_W : " + (-CHECK_W));
        //Debug.Log("vecPos.x : " + (vecPos.x));
        //Debug.Log("-CHECK_W + vecPos.x : " + (-CHECK_W + vecPos.x));

        for(i =0; i< listView.Count; i++)
        {
            if(is_all
            || listView[i].rt.anchoredPosition.x < (-CHECK_W * 2) - vecPos.x
            || listView[i].rt.anchoredPosition.x > (CHECK_W * 2) - vecPos.x
            || listView[i].rt.anchoredPosition.y < (-CHECK_H * 2) - vecPos.y
            || listView[i].rt.anchoredPosition.y > (CHECK_H * 2) - vecPos.y)
            {
                listDelete.Add(listView[i]);
            }
        }
        for(i = 0; i< listDelete.Count; i++)
        {
            ReturnObject(listDelete[i]);
        }
        listDelete.Clear();
    }

    public void CheckBgObject()
    {
        //가로 10, 세로 6
        //Debug.Log(PlayManager.ins.stage.rt.anchoredPosition);
        if(PlayManager.ins.stage.rt.anchoredPosition.x < -CHECK_W + vecPos.x)
        {   //우로 이동
            //Debug.Log("Right Move");
            vecPos.x -= CHECK_W;
            DeleteObject();
            CreateObject(Mathf.RoundToInt(VIEW_W * 1.5f),0,VIEW_W * 2, VIEW_H * 2);
        }
        if(PlayManager.ins.stage.rt.anchoredPosition.x > CHECK_W + vecPos.x)
        {   //좌로 이동
            //Debug.Log("Left Move");
            vecPos.x += CHECK_W;
            DeleteObject();
            CreateObject(0,0,Mathf.RoundToInt(VIEW_W * 0.5f), VIEW_H * 2);
        }
        if(PlayManager.ins.stage.rt.anchoredPosition.y > CHECK_H + vecPos.y)
        {   //아래로 이동
            //Debug.Log("Bottom Move");
            vecPos.y += CHECK_H;
            DeleteObject();
            CreateObject(0,0,VIEW_W * 2, Mathf.RoundToInt(VIEW_H * 0.5f));
        }
        if(PlayManager.ins.stage.rt.anchoredPosition.y < -CHECK_H + vecPos.y)
        {   //위로 이동
            //Debug.Log("Top Move");
            vecPos.y -= CHECK_H;
            DeleteObject();
            CreateObject(0,Mathf.RoundToInt(VIEW_H * 1.5f),VIEW_W * 2, VIEW_H * 2);
        }
        
    }

    public void ReturnObject(BgObject _obj)
    {
        listView.Remove(_obj);
        dicObject[_obj.idx].Add(_obj);
        if(_obj.type == ObjectBase.TYPE.BgOver)
        {
            PlayManager.ins.stage.RemoveList(_obj);
        }
        _obj.go.SetActive(false);
        _obj.rt.SetParent(rt);        
    }

    public BgObject GetObject(int _idx)
    {
        if(dicObject.ContainsKey(_idx) == false) return null;
        tmp = null;
        for(loopI = 0; loopI < dicObject[_idx].Count; loopI++)
        {
            if(dicObject[_idx][loopI].go.activeSelf == false)
            {
                tmp = dicObject[_idx][loopI];
                dicObject[_idx].Remove(tmp);
                break;
            }
        }

        if(tmp == null) tmp = CreateObject(_idx);

        if(tmp.type == ObjectBase.TYPE.BgBack)
        {   //뒤에 표시
            tmp.rt.SetParent(PlayManager.ins.stage.rtBgObject);
        }
        else
        {   //유저와 뎁스 계산
            tmp.rt.SetParent(PlayManager.ins.stage.rtGround);
            PlayManager.ins.stage.AddList(tmp);
        }
        tmp.rt.localScale = Vector3.one;
        tmp.rt.localRotation = Quaternion.identity;
        tmp.go.SetActive(true);
        listView.Add(tmp);
        return tmp;
    }

    private BgObject CreateObject(int _idx)
    {
        tmp = GameObject.Instantiate<BgObject>(prefabObject[_idx - 1]);        
        return tmp;
    }
}
