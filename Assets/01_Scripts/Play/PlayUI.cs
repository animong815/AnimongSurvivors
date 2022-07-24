using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
	public ButtonObject btnHitArea;

	public Text txtDistance;
	public Text txtDistanceTop;

	public Text txtCoin;
	public Text txtTopDistance;
	
	public Text txtTime;
	public Image imgStamina;

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
	public const int TILE_SIZE = 160;
	//
	private int num;
	[SerializeField]
	private int distance;
	//[SerializeField]
	//private int coin;
	
	private CloudOnce.CloudPrefs.CloudInt cloudCoin;

	public GameObject goControl;
	public RectTransform rtControl;
	public RectTransform rtDrag;
	public GameObject goBgTile;
	public RectTransform rtBgTile;

	private bool isPress = false;

	private Vector3 vec3;
	private float drag_distance;
	private const float MAX_DRAG = 1f;

	private Vector3 vecMove = Vector3.zero;
	
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

		goControl.SetActive(false);

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
		if (PlayManager.ins.is_init == false) return;

		if (value == true && objMain.activeSelf)
		{
			objMain.SetActive(false);
			objIngame.SetActive(true);
			btnRetry.obj.SetActive(false);
			btnRetryNow.obj.SetActive(false);
			objIngameScoreBox.SetActive(true);
			PlayManager.ins.is_play = false;
			PlayManager.ins.player.goIdle.SetActive(false);
			PlayManager.ins.player.goRun.SetActive(true);
			
			LeanTween.moveLocalY(PlayManager.ins.player.go, 0f, 1f);
			LeanTween.moveLocalY(goBgTile, 0f, 1f);
			LeanTween.moveLocalY(PlayManager.ins.stage.go, 0f, 1f).setOnComplete(()=> 
			{
				PlayManager.ins.is_play = true;
				PlayManager.ins.player.goIdle.SetActive(true);
				PlayManager.ins.player.goRun.SetActive(false);
			}).setOnUpdate((float f)=>
			{
				PlayManager.ins.stage.SortStage();
			});//.setEaseOutQuad();
			//LeanTween.moveLocalY(rtBgTile.gameObject, -100f, 1f);//.setEaseOutQuad();
			UpdateDistance(0);
			PlayManager.ins.stage.StartStage();

			return;
		}
		if (objMain.activeSelf) return;
		isPress = value;
	}

	public void UpdateMove() 
	{
		if (btnRetry.obj.activeSelf == true) return;
		if (PlayManager.ins.is_play == false) return;
#if UNITY_EDITOR
		vecMove = Vector3.zero;
		bool isKey = false;
		float speed = 71f;
		int dir = 0;
		if(Input.GetKey("up") || Input.GetKey("w"))
        {   //Debug.Log("↑ 키를 누르고 있는 동안 출력");
			vecMove.y = speed * 0.05f * Time.smoothDeltaTime * PlayManager.ins.player.speed;
			isKey = true;
        }
        if(Input.GetKey("down") || Input.GetKey("s"))
        {   //Debug.Log("↓ 키를 누르고 있는 동안 출력");
			speed *= -1;
			vecMove.y = speed * 0.05f * Time.smoothDeltaTime * PlayManager.ins.player.speed;
			isKey = true;
		}
        
		if(speed < 0) speed *= -1;
		
		if(Input.GetKey("left") || Input.GetKey("a"))
        {   //Debug.Log(" ← 키를 누르고 있는 동안 출력");
			speed *= -1;
			vecMove.x = speed * 0.05f * Time.smoothDeltaTime * PlayManager.ins.player.speed;
			isKey = true;
			dir = 1;
		}
        if(Input.GetKey("right") || Input.GetKey("d"))
        {   //Debug.Log(" → 키를 누르고 있는 동안 출력");
			vecMove.x = speed * 0.05f * Time.smoothDeltaTime * PlayManager.ins.player.speed;
			isKey = true;
			dir = 2;
        }

		PlayManager.ins.player.SetUseStamina(isKey && !Input.GetKey(KeyCode.LeftShift));

		if(isKey)
		{
			MovePlayer();
			if (PlayManager.ins.player.goIdle.activeSelf == true) PlayManager.ins.player.goIdle.SetActive(false);
			if (PlayManager.ins.player.goRun.activeSelf == false) PlayManager.ins.player.goRun.SetActive(true);

			if (dir == 1)
			{
				if (PlayManager.ins.player.tfRun.localScale.x > 0)
				{
					vec3 = Vector3.one;
					vec3.x = -1;
					PlayManager.ins.player.tfRun.localScale = vec3;
					PlayManager.ins.player.is_right = false;
				}
			}
			if (dir == 2)
			{
				if (PlayManager.ins.player.tfRun.localScale.x < 0)
					PlayManager.ins.player.tfRun.localScale = Vector3.one;
				PlayManager.ins.player.is_right = true;
			}
			return;
		}

#endif
		if (isPress == false)
		{   //컨트롤러 화면에 안보이도록
			if (goControl.activeSelf) goControl.SetActive(false);
			if (PlayManager.ins.player.goIdle.activeSelf == false)
			{
				PlayManager.ins.player.goIdle.SetActive(true);
				PlayManager.ins.player.tfIdle.localScale = PlayManager.ins.player.tfRun.localScale;
			}
			if (PlayManager.ins.player.goRun.activeSelf == true) PlayManager.ins.player.goRun.SetActive(false);
			return;
		}
	
#if UNITY_EDITOR
#else
		if (Input.touches != null && Input.touches.Length == 1)
#endif
		{
			vec3 = Input.mousePosition;

			vec3.x -= PlayManager.ins.cam2d.pixelWidth * 0.5f;
			vec3.y -= PlayManager.ins.cam2d.pixelHeight * 0.5f;

			vec3.x /= PlayManager.ins.canvas.scaleFactor;
			vec3.y /= PlayManager.ins.canvas.scaleFactor;

		}
#if UNITY_EDITOR
#else
		else
		{
			if (goControl.activeSelf) goControl.SetActive(false);
			if (PlayManager.ins.player.goIdle.activeSelf == false)
			{
				PlayManager.ins.player.goIdle.SetActive(true);
				PlayManager.ins.player.tfIdle.localScale = PlayManager.ins.player.tfRun.localScale;
			}
			if (PlayManager.ins.player.goRun.activeSelf == true) PlayManager.ins.player.goRun.SetActive(false);
			return;
		}
#endif


		//컨트롤러 화면에 보이도록
		if (goControl.activeSelf == false) 
		{ //처음 화면에 보이면서 위치 설정
			goControl.SetActive(true);
			rtControl.localPosition = vec3;
			rtDrag.localPosition = Vector3.zero;
		}
		rtDrag.localPosition = vec3 - rtControl.localPosition;
		drag_distance = Vector3.Distance(rtControl.position, rtDrag.position);
		if (drag_distance > MAX_DRAG)
		{
			vec3 = rtDrag.localPosition;
			vec3.x = vec3.x *  (MAX_DRAG / drag_distance);
			vec3.y = vec3.y *  (MAX_DRAG / drag_distance);
			rtDrag.localPosition = vec3;
			//화면 끝으로 드래그 한 경우 
			PlayManager.ins.player.SetUseStamina(true);
		}
		else
		{
			PlayManager.ins.player.SetUseStamina(false);
		}
		if (PlayManager.ins.player.goIdle.activeSelf == true) PlayManager.ins.player.goIdle.SetActive(false);
		if (PlayManager.ins.player.goRun.activeSelf == false) PlayManager.ins.player.goRun.SetActive(true);

		if (rtDrag.localPosition.x < 0)
		{
			if (PlayManager.ins.player.tfRun.localScale.x > 0)
			{
				vec3 = Vector3.one;
				vec3.x = -1;
				PlayManager.ins.player.tfRun.localScale = vec3;
			}
		}
		else
		{
			if (PlayManager.ins.player.tfRun.localScale.x < 0)
				PlayManager.ins.player.tfRun.localScale = Vector3.one;
		}

		//Debug.Log(rtDrag.localPosition.x);

		vecMove.x = rtDrag.localPosition.x * 0.05f * Time.smoothDeltaTime * PlayManager.ins.player.speed;
		vecMove.y = rtDrag.localPosition.y * 0.05f * Time.smoothDeltaTime * PlayManager.ins.player.speed;
		MovePlayer();		
	}

	private void MovePlayer()
	{
		PlayManager.ins.player.MoveUpdate(vecMove);
		vecMove += PlayManager.ins.player.GetGap();
		PlayManager.ins.stage.UpdateMove(vecMove);
		
		vec3 = rtBgTile.localPosition;
		vec3.x -= vecMove.x;
		vec3.y -= vecMove.y;
		
		vecMove = vec3;
		//배경 타일 반복 움직임
		if (vec3.x > 0) vec3.x -= TILE_SIZE;
		if (vec3.x < -TILE_SIZE) vec3.x += TILE_SIZE;
		if (vec3.y > 0) vec3.y -= TILE_SIZE;
		if (vec3.y < -TILE_SIZE) vec3.y += TILE_SIZE;

		rtBgTile.localPosition = vec3;
		if(vecMove.x != vec3.x || vecMove.y != vec3.y)
		{	//화면 끝에 도달해서 이동한 상황
			PlayManager.ins.stage.bgObject.CheckBgObject();
		}
		vec3.x = -PlayManager.ins.stage.rt.localPosition.x;
		vec3.y = -PlayManager.ins.stage.rt.localPosition.y;
		PlayManager.ins.player.rt.localPosition = vec3;
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
		timeRetryWaitEnd = PlayManager.ins.data.global.ad_wait_time;

		goControl.SetActive(false);
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
		objIngame.SetActive(false);

		//stage 초기화
		PlayManager.ins.stage.ResetStage();
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
