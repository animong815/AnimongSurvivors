using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    public GameObject obj;
    public RectTransform rectBg;
    public ButtonObject btnPlay;
    public ButtonObject btnBack;

    public CanvasGroup group;

    private int cur_idx;

    public void Init()
    {
        obj.SetActive(false);
        //Debug.Log(Mathf.CeilToInt(Screen.height / PlayManager.ins.canvas.scaleFactor));
        //배경 사이즈 리사이즈
        rectBg.sizeDelta = Vector2.one * Mathf.CeilToInt(Screen.height / PlayManager.ins.canvas.scaleFactor);
        
        btnBack.btn.onClick.AddListener(Hide);
        btnPlay.btn.onClick.AddListener(ClickPlay);
    }

    public void Show() 
    {
        obj.SetActive(true);
        cur_idx = 0;
        group.alpha = 0f;
        LeanTween.alphaCanvas(group, 1f, 0.3f);
    }

    private void ClickPlay() 
    {
        Debug.Log(cur_idx);
        Hide();
    }

    public void Hide() 
    {
        obj.SetActive(false);
    }

}
