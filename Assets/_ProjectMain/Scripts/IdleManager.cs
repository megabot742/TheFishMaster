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
        lengthCost = coast[-length / 10 - 3];
        strengthCost = coast[strength - 3];
        offlineEarningsCost = coast[offlineEarnings - 3];
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
        length -= 10;
        wallet -= lengthCost;
        lengthCost = coast[-length / 10 - 3];
        PlayerPrefs.SetInt("Length", -length);
        PlayerPrefs.SetInt("Wallet", wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }
    public void BuyStrength()
    {
        strength++;
        wallet -= strengthCost;
        strengthCost = coast[strength - 3];
        PlayerPrefs.SetInt("Strength", strength);
        PlayerPrefs.SetInt("Wallet", wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }
    public void BuyOfflineEarning()
    {
        offlineEarnings++;
        wallet -= offlineEarningsCost;
        offlineEarningsCost = coast[offlineEarnings - 3];
        PlayerPrefs.SetInt("Offline", offlineEarnings);
        PlayerPrefs.SetInt("Wallet", wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
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
