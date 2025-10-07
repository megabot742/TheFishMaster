using System;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [HideInInspector] public int length;
    [HideInInspector] public int strength;
    [HideInInspector] public int offlineEarnings;

    [HideInInspector] public int lengthCost;
    [HideInInspector] public int strengthCost;
    [HideInInspector] public int offlineEarningsCost;

    [HideInInspector] public int totalGain;
    [HideInInspector] public int wallet;

    int[] coast = new int[]
    {
        120, 151, 197, 250, 324, 414, 537, 687, 892, 1145, 1484, 1911, 2479, 3196, 4148, 5359, 6954, 9000, 11687
    };
    public int MaxLength => -(coast.Length - 3) * 10; // -210 => coast.Length = 19
    public int MaxStrength => coast.Length + 2; // 21 => coast.Length = 19
    public int MaxOfflineEarnings => coast.Length + 2; // 21 => coast.Length = 19
    public static IdleManager instance;

    void Awake()
    {
        if(IdleManager.instance) Destroy(gameObject);
        else
        {
            IdleManager.instance = this;
        }

        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength",3);
        offlineEarnings = PlayerPrefs.GetInt("Offline",3);
        lengthCost = coast[Mathf.Min(-length / 10 - 3, coast.Length - 1)];
        strengthCost = coast[Mathf.Min(strength - 3, coast.Length - 1)];
        offlineEarningsCost = coast[Mathf.Min(offlineEarnings - 3, coast.Length - 1)];
        wallet = PlayerPrefs.GetInt("Wallet", 0);

    }

    private void OnApplicationPause(bool paused) 
    {
        if(paused)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            Debug.Log(now);
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date", string.Empty);
            if(@string != string.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineEarnings + 1.0);
                ScreensManager.instance.ChangeScreen(Screens.RETURN);
            }
        }
    }

    private void OnApplicationQuit() {
        OnApplicationPause(true);
    }
    public void BuyLength()
    {
        int nextStrengthIndex = strength - 2; // Next index after increment
        if (nextStrengthIndex < coast.Length && wallet >= strengthCost)
        {
            strength++;
            wallet -= strengthCost;
            strengthCost = coast[Mathf.Min(nextStrengthIndex, coast.Length - 1)];
            PlayerPrefs.SetInt("Strength", strength);
            PlayerPrefs.SetInt("Wallet", wallet);
            ScreensManager.instance.ChangeScreen(Screens.MAIN);
        }
    }
    public void BuyStrength()
    {
        int nextStrengthIndex = strength - 2; // Next index after increment
        if (nextStrengthIndex < coast.Length && wallet >= strengthCost)
        {
            strength++;
            wallet -= strengthCost;
            strengthCost = coast[Mathf.Min(nextStrengthIndex, coast.Length - 1)];
            PlayerPrefs.SetInt("Strength", strength);
            PlayerPrefs.SetInt("Wallet", wallet);
            ScreensManager.instance.ChangeScreen(Screens.MAIN);
        }
    }
    public void BuyOfflineEarning()
    {
        int nextStrengthIndex = strength - 2; // Next index after increment
        if (nextStrengthIndex < coast.Length && wallet >= strengthCost)
        {
            strength++;
            wallet -= strengthCost;
            strengthCost = coast[Mathf.Min(nextStrengthIndex, coast.Length - 1)];
            PlayerPrefs.SetInt("Strength", strength);
            PlayerPrefs.SetInt("Wallet", wallet);
            ScreensManager.instance.ChangeScreen(Screens.MAIN);
        }
    }

    public void CollectMoney()
    {
        wallet += totalGain;
        PlayerPrefs.SetInt("Wallet", wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectDoubleMoney()
    {
        wallet += totalGain * 2;
        PlayerPrefs.SetInt("Wallet", wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }
}
