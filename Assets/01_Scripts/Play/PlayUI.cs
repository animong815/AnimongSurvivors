using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
	public ButtonObject btnHitArea;

	public Text txtDistance;
	public Text txtDistanceTop;

	public Text txtCoin;
	public Text txtTopDistance;
	
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
		cloudCoin = new CloudOnce.CloudPrefs.CloudInt("Coin", CloudOnce.PersistenceType.Latest);
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
		PlayManager.ins.mgrAD.ShowAd();
	}

	public void RetryNow()
	{
		PlayManager.ins.show_ad++;
		PlayManager.ins.player.Init(true);

		btnRetry.obj.SetActive(false);
		btnRetryNow.obj.SetActive(false);
		objMain.SetActive(false);
	}

	private void ClickRanking()
	{
		CloudOnce.Cloud.Leaderboards.ShowOverlay();
	}

	private void ClickCharacter() 
	{
		character.Show();
	}

}
