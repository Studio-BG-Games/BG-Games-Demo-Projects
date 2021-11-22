using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeAd
{
	Interstitial,
	Reward
}
public class ShowAd : MonoBehaviour
{
	public TypeAd TypeAd;

	public void ShowYandexAd()
	{
		switch (TypeAd)
		{
			case TypeAd.Interstitial:
				YandexSDK.instance.ShowInterstitial();
				break;
		}
	}
}
