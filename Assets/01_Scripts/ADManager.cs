using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class ADManager : MonoBehaviour
{
	private RewardedAd rewardedAd;

	/// app id
	//test
	//ca-app-pub-3940256099942544~3347511713
	//real
	//ca-app-pub-5514917895492992~8912027324

	//private const string AD_KEY = "ca-app-pub-3940256099942544/5224354917"; //test
	private string AD_KEY = "ca-app-pub-5514917895492992/1775773339"; //real
	
	public void Init()
	{
		/*
		MobileAds.Initialize((initStatus) => 
		{
			Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
			foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
			{
				string className = keyValuePair.Key;
				AdapterStatus status = keyValuePair.Value;
				switch (status.InitializationState)
				{
					case AdapterState.NotReady:
						// The adapter initialization did not complete.
						MonoBehaviour.print("Adapter: " + className + " not ready.");
						break;
					case AdapterState.Ready:
						// The adapter was successfully initialized.
						MonoBehaviour.print("Adapter: " + className + " is initialized.");
						break;
				}
			}

			CreateAndLoadRewardedAd(); 
		});
		//CreateAndLoadRewardedAd();
		*/
	}

	public bool IsLoadAd() { return rewardedAd.IsLoaded();	}

	public void ShowAd()
	{
		/*
		Debug.Log("ShowAd");
		if (rewardedAd.IsLoaded()) // 광고가 로드 되었을 때
		{
			Debug.Log("rewardedAd.Show");
			rewardedAd.Show(); // 광고 보여주기
		}
		*/
	}

	public void CreateAndLoadRewardedAd() // 광고 다시 로드하는 함수
	{
		/*
		Debug.Log("CreateAndLoadRewardedAd::" + PlayManager.ins.data.ad_id);
		AD_KEY = PlayManager.ins.data.ad_id;

#if UNITY_EDITOR
		AD_KEY = "unexpected_platform";
#endif

		rewardedAd = new RewardedAd(AD_KEY);

		rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
		rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

		rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
		rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		rewardedAd.OnAdFailedToLoad -= HandleAdLoadFail;
		rewardedAd.OnAdFailedToLoad += HandleAdLoadFail;

		rewardedAd.OnAdFailedToShow -= HandleAdShowFail;
		rewardedAd.OnAdFailedToShow += HandleAdShowFail;

		rewardedAd.OnAdLoaded -= HandleAdLoad;
		rewardedAd.OnAdLoaded += HandleAdLoad;

		AdRequest request = new AdRequest.Builder().Build();
		
		rewardedAd.LoadAd(request);
		*/
	}

	/*
	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{  // 사용자가 광고를 닫았을 때
		CreateAndLoadRewardedAd();  // 광고 다시 로드
	}

	private void HandleUserEarnedReward(object sender, Reward e)
	{   // 광고를 다 봤을 때
		PlayManager.ins.ui.RetryNow();
	}

	private void HandleAdLoadFail(object sender, AdFailedToLoadEventArgs arg) 
	{
		Debug.Log("AdLoadFail  ::" + arg.LoadAdError);
	}

	private void HandleAdShowFail(object sender, AdErrorEventArgs arg) 
	{
		Debug.Log("AdShowFail  ::" + arg.AdError);
	}

	private void HandleAdLoad(object sender, EventArgs arg)
	{
		Debug.Log("AD Load Complete");
	}
	*/
}
