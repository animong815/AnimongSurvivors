using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
	public ButtonObject btnHitArea;

	public UnityEngine.UI.Text txtDistance;
	public UnityEngine.UI.Text txtDistanceTop;

	public UnityEngine.UI.Text txtCoin;
	public UnityEngine.UI.Text txtTopDistance;
	
	public GameObject objIngame;
	public GameObject objMain;

	public GameObject objTopDistance;
	public GameObject objIngameScoreBox;
	public GameObject objCoin;

	public GameObject objTitle;
	public ButtonObject btnRetry;
	public Image imgRetryTime;
	public ButtonObject btnRetryNow;
	

	public ButtonObject btnLogin;
	public ButtonObject btnGuest;

	public ButtonObject btnChar;
	public ButtonObject btnShop;
	public ButtonObject btnRanking;
	public ButtonObject btnOption;

	public ButtonObject btnPause;

	public GameObject objGuide;
	public RectTransform rectGuide;

	public UICharacter character;

	private float timeRetryWaitStart = 0f;
	private float timeRetryWaitEnd = 0f;

	//
	private int num;
	[SerializeField]
	private int distance;
	//[SerializeField]
	//private int coin;
	
	private CloudOnce.CloudPrefs.CloudInt cloudCoin;

	private int coin
	{
		get { return cloudCoin.Value; }
		set { cloudCoin.Value = value; }
	}

	private CloudOnce.CloudPrefs.CloudInt cloudScore;

	public int topScore
	{
		get { return cloudScore.Value; }
		set { cloudScore.Value = value; }
	}
	
	public void Init()
	{
		btnHitArea.fncPress = PressHit;

		btnPause.btn.onClick.AddListener(ClickPause);
		btnRetry.btn.onClick.AddListener(ClickRetry);
		btnRetryNow.btn.onClick.AddListener(ClickRetryNow);
		btnRanking.btn.onClick.AddListener(ClickRanking);
		btnChar.btn.onClick.AddListener(ClickCharacter);

		objMain.SetActive(true);
		objIngame.SetActive(false);

		btnRetry.obj.SetActive(false);
		btnRetryNow.obj.SetActive(false);

		character.Init();
	}
	
	public void InitVariables()
	{
		//objGuide.SetActive(true);
		//rectGuide.localPosition = PlayManager.ins.get
		//PlayManager.ins.player.tranPoint0.position;
		
		Vector3 vec = PlayManager.ins.cam3d.WorldToScreenPoint(PlayManager.ins.player.tranPoint1.position);
		vec.x -= PlayManager.ins.cam2d.pixelWidth * 0.5f;
		vec.y -= PlayManager.ins.cam2d.pixelHeight * 0.5f;
		vec.x /= PlayManager.ins.canvas.scaleFactor;
		vec.y /= PlayManager.ins.canvas.scaleFactor;
		vec.z = 0f;
		rectGuide.localPosition = vec;
		
		cloudCoin = new CloudOnce.CloudPrefs.CloudInt("Coin", CloudOnce.PersistenceType.Latest);//CloudOnce.CloudVariables.Coin;
		UpdateCoin();

		cloudScore = new CloudOnce.CloudPrefs.CloudInt("Score", CloudOnce.PersistenceType.Latest);
		topScore = topScore;
		txtTopDistance.text = string.Format(PlayManager.STR_DISTANCE, topScore);
		objTopDistance.SetActive(true);
		txtDistanceTop.text = string.Format(PlayManager.STR_TOP_DISTANCE, topScore);
	}

	public int GetDistance() { return distance; }
	public int GetCoin() { return coin; }

	public void InitData()
	{
		distance = -1;
		UpdateDistance();
		//UpdateCoin();
	}

	private void PressHit(bool value)
	{
		if (btnRetry.obj.activeSelf == true) return;
		if (PlayManager.ins.is_play == false) return;

		if (value == true && objMain.activeSelf)
		{
			objMain.SetActive(false);
			objIngame.SetActive(true);
			btnRetry.obj.SetActive(false);
			btnRetryNow.obj.SetActive(false);
			objIngameScoreBox.SetActive(true);
			UpdateDistance(0);
			return;
		}
		if (objMain.activeSelf) return;

		PlayManager.ins.stage.CheckLine(value);
	}

	public void AddCoin()
	{
		coin++;
		UpdateCoin();
	}

	private void UpdateCoin()
	{
		txtCoin.text = $"{coin}";//:000}";
	}

	public void UpdateDistance(int value = 0)
	{
		if(distance < 0) txtDistance.text = string.Format(PlayManager.STR_DISTANCE, 0);
		if (PlayManager.IS_CHEK_LINE)
		{
			if (distance < value)
			{
				distance = value;
				txtDistance.text = string.Format(PlayManager.STR_DISTANCE, distance);
			}
			return;
		}
		num = Mathf.FloorToInt(PlayManager.ins.player.tran.localPosition.y);
		if (distance < num)
		{
			distance = num;
			txtDistance.text = string.Format(PlayManager.STR_DISTANCE, distance);
		}
	}

	public void InitRetryUI() 
	{
		timeRetryWaitStart = Time.time;
		timeRetryWaitEnd = PlayManager.ins.data.ad_wait_time;

		btnRetry.obj.SetActive(true);
		if (PlayManager.ins.stage.linePool != null
			&& PlayManager.ins.stage.linePool.jump_index != -1
			&& PlayManager.ins.show_ad < PlayManager.ins.data.ad_cnt
			&& PlayManager.ins.mgrAD.IsLoadAd()) btnRetryNow.obj.SetActive(true);
	}

	public void UpdateRetryTime() 
	{
		if (timeRetryWaitEnd == 0f || timeRetryWaitStart == 0f) return;
		imgRetryTime.fillAmount = (Time.time - timeRetryWaitStart) / timeRetryWaitEnd;

		if (imgRetryTime.fillAmount >= 1f)
		{
			imgRetryTime.fillAmount = 1f;
			timeRetryWaitStart = 0f;
			timeRetryWaitEnd = 0f;
			ClickRetry();
		}
	}

	private void ClickPause()
	{
		SceneManager.ins.LoadScene(SceneManager.SCENE.LOBBY);
	}

	private void ClickRetry()
	{
		PlayManager.ins.Init(false);
		btnRetry.obj.SetActive(false);
		btnRetryNow.obj.SetActive(false);

		//txtDistance.text = string.Empty;
		txtDistance.text = string.Format(PlayManager.STR_DISTANCE, 0);
		objIngameScoreBox.SetActive(false);
		//objMain.SetActive(false);
		btnChar.obj.SetActive(true);
		btnRanking.obj.SetActive(true);
		objTitle.SetActive(true);
	}

	private void ClickRetryNow()
	{
		//ca-app-pub-3940256099942544/5224354917
//#if UNITY_EDITOR
//		RetryNow();
//#else
		PlayManager.ins.mgrAD.ShowAd();
//#endif
	}

	public void RetryNow()
	{
		PlayManager.ins.show_ad++;
		PlayManager.ins.stage.linePool.CreateLine(true);
		PlayManager.ins.player.Init(true);
		PlayManager.ins.stage.linePool.actLine.FirstTake();

		btnRetry.obj.SetActive(false);
		btnRetryNow.obj.SetActive(false);
		objMain.SetActive(false);
	}

	private void ClickRanking()
	{
		CloudOnce.Cloud.Leaderboards.ShowOverlay();
		//CloudOnce.Leaderboards.HigeScore.ShowOverlay();
	}

	private void ClickCharacter() 
	{
		character.Show();
	}

}
