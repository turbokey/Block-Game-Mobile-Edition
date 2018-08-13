using System;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdvertiseService : MonoBehaviour , IInterstitialAdListener {

	// Use this for initialization
	void Start () {
		Appodeal.setTesting(false);
		Appodeal.setAutoCache(Appodeal.INTERSTITIAL, false);
		Appodeal.cache(Appodeal.INTERSTITIAL);
		Appodeal.setInterstitialCallbacks(this);
		Appodeal.initialize("40d95be0c1c5a566284086764a329dd3440f6e4f3c88c1ae", Appodeal.BANNER | Appodeal.INTERSTITIAL);
		Appodeal.show (Appodeal.BANNER_BOTTOM);
		Appodeal.show(Appodeal.INTERSTITIAL);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void onInterstitialLoaded() {
		Appodeal.show(Appodeal.INTERSTITIAL);
	}
	public void onInterstitialFailedToLoad() { print("Interstitial failed"); }
	public void onInterstitialShown() { print("Interstitial opened"); }
	public void onInterstitialClosed() {  print("Interstitial closed"); }
	public void onInterstitialClicked() { print("Interstitial clicked"); }
}
