using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
	public static PlayManager ins;

	public Camera cam2d;
	public Canvas canvas;
	public float canvasSize;
	public Camera cam2dB;
	public Canvas canvasB;

	public ADManager mgrAD;
	public PlayUI ui;
	public Player player;
	public PopupLoaing loading;

	public Stage stage;

	public DataManager data;

	public const float MAX_W = 70f;
	public bool is_init = false;
	private bool is_showGuide;

	public const string STR_LEADER = "CgkI4ZSUsegQEAIQBA";
	public bool is_play;

	public int show_ad;
	//public const string STR_DISTANCE = "{0:000}m";
	public const string STR_DISTANCE = "{0}";//"{0:000}";
	public const string STR_TOP_DISTANCE = "TOP{0}";

	/// <summary> 점수 체크를 라인 기준으로 할지 여부 </summary>
	public static bool IS_CHEK_LINE = true;

	private void Awake()
	{
		if (PlayManager.ins != null)
		{
			Destroy(this);
			return;
		}
		is_showGuide = false;
		is_init = false;
		is_play = false;
		PlayManager.ins = this;

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		//UnityEngine.Caching.ClearCache();
	}

	void Start()
    {
		//mgrAD.Init();
		
		ui.btnRetry.obj.SetActive(false);
		ui.btnRetryNow.obj.SetActive(false);

		ui.btnRanking.obj.SetActive(false);
		ui.btnShop.obj.SetActive(false);
		ui.btnChar.obj.SetActive(false);
		ui.btnOption.obj.SetActive(false);

		ui.btnGuest.obj.SetActive(false);
		ui.btnGuest.btn.onClick.AddListener(InitPlay);
		ui.btnLogin.obj.SetActive(false);
		ui.btnLogin.btn.onClick.AddListener(SetLogin);
		
		player.go.SetActive(false);
		
		ui.txtCoin.text = string.Empty;
		ui.txtDistance.text = string.Empty;
		ui.txtTopDistance.text = string.Empty;
		ui.txtDistanceTop.text = string.Empty;

		ui.objTopDistance.SetActive(false);
		ui.objCoin.SetActive(false);

		canvasSize = canvas.transform.localScale.x;

		stage.Init();

		CloudOnce.Cloud.OnInitializeComplete += CloudInitComplete;
		CloudOnce.Cloud.Initialize(false, false, false);
	}
	
	private void CloudInitComplete()
	{
		Debug.Log("CloudOnce Init");
		CloudOnce.Cloud.OnInitializeComplete -= CloudInitComplete;

		SetLogin();
		//test guest
		//OnFailLogin();
	}

	public void SetLogin() 
	{
		CloudOnce.Cloud.OnSignedInChanged += OnLogin;
		CloudOnce.Cloud.OnSignInFailed += OnFailLogin;
		CloudOnce.Cloud.SignIn();
	}

	private void OnFailLogin() 
	{
		CloudOnce.Cloud.OnSignedInChanged -= OnLogin;
		CloudOnce.Cloud.OnSignInFailed -= OnFailLogin;

		ui.btnLogin.obj.SetActive(true);
		ui.btnGuest.obj.SetActive(true);
	}

	private void InitUI()
	{
		ui.btnRanking.obj.SetActive(true);
		ui.btnChar.obj.SetActive(true);

		player.go.SetActive(true);
	}

	private void OnLogin(bool value) 
	{
		Debug.Log("Google Play Login");
		CloudOnce.Cloud.OnSignedInChanged -= OnLogin;
		CloudOnce.Cloud.OnSignInFailed -= OnFailLogin;
		if (value) InitPlay();
		else 
		{
			ui.btnLogin.obj.SetActive(true);
			ui.btnGuest.obj.SetActive(true);
		}
	}

	private void InitPlay()
	{
		//ui.InitCoin();
		ui.btnLogin.obj.SetActive(false);
		ui.btnGuest.obj.SetActive(false);
		InitUI();
		Init();
		DataManager.ins.loading = loading;
		DataManager.ins.LoadFirstLoad();

		//is_init = true;

		//코인 정보 불러옴
		CloudOnce.Cloud.OnCloudLoadComplete += LoadCloud;
		CloudOnce.Cloud.Storage.Load();
		//ui.InitCoin();
	}

	private void LoadCloud(bool value)
	{
		CloudOnce.Cloud.OnCloudLoadComplete -= LoadCloud;
		ui.InitVariables();
		ui.objCoin.SetActive(true);

		//리더보드에서 정보 불러옴
		CloudOnce.Cloud.Leaderboards.LoadScores(STR_LEADER, LoadScores);
	}

	private void LoadScores(UnityEngine.SocialPlatforms.IScore[] values)
	{
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i].userID == CloudOnce.Cloud.PlayerID)
			{
				if (ui.topScore < (int)values[i].value)
				{
					ui.topScore = (int)values[i].value;
					ui.txtTopDistance.text = string.Format(STR_DISTANCE, ui.topScore);
					ui.objTopDistance.SetActive(true);
					ui.txtDistanceTop.text = string.Format(STR_TOP_DISTANCE, ui.topScore);
				}
				//ui.txtDistanceTop.text = "TOP" + values[i].rank;
				break;
			}
		}
	}



	public void Init(bool isFirst = true, bool isStartY = true)
	{
		if (isFirst)
		{
			ui.Init();
		}
		else
		{
			if (is_showGuide == false)
			{
				mgrAD.Init();
			}
			is_showGuide = true;
		}
		is_init = true;
		show_ad = 0;
		
		player.Init();
		ui.InitData();		
	}
	
    void Update()
    {
		if (is_init == false) return;

		if (ui.btnRetry.obj.activeSelf == true)
		{
			ui.UpdateRetryTime();
			return;
		}
		ui.UpdateMove();
		player.UpdateObject();
		stage.UpdateStage();
	}

	public void GameOver()
	{
		Debug.Log("GAME OVER");
		is_play = false;

		if (ui.GetDistance() > ui.topScore)
		{	//최고 점수 갱신?
			ui.topScore = ui.GetDistance();
			ui.txtTopDistance.text = string.Format(STR_DISTANCE, ui.topScore);
			ui.txtDistanceTop.text = string.Format(STR_TOP_DISTANCE, ui.topScore);
		}

		CloudOnce.Cloud.Leaderboards.SubmitScore(STR_LEADER, ui.GetDistance());
		CloudOnce.Cloud.Storage.Save();
		ShowRetryUI(true);
	}

	private void ShowRetryUI(bool value)
	{
		CloudOnce.Cloud.OnCloudSaveComplete -= ShowRetryUI;

		ui.objIngame.SetActive(true);
		ui.objIngameScoreBox.SetActive(true);
		ui.objMain.SetActive(true);
		ui.objTitle.SetActive(false);		
		
		ui.btnChar.obj.SetActive(false);
		ui.btnRanking.obj.SetActive(false);

		player.goRun.SetActive(false);
		player.goIdle.SetActive(true);

		ui.InitRetryUI();
	}

	/*
	public Vector3 Get3Dto2D(Vector3 vec)
	{
		vec = cam3d.WorldToScreenPoint(vec);
		vec.x -= cam2d.pixelWidth * 0.5f;
		vec.y -= cam2d.pixelHeight * 0.5f;
		vec.x /= canvas.scaleFactor;
		vec.y /= canvas.scaleFactor;
		vec.z = 0f;

		return vec;
	}

	public Vector3 GetBack3Dto2D(Vector3 vec)
	{
		vec = cam3d.WorldToScreenPoint(vec);
		vec.x -= cam2dB.pixelWidth * 0.5f;
		vec.y -= cam2dB.pixelHeight * 0.5f;
		vec.x /= canvasB.scaleFactor;
		vec.y /= canvasB.scaleFactor;
		vec.z = 0f;

		return vec;
	}
	*/
}
