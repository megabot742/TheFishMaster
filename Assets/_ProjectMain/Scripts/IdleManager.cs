using System;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    [HideInInspector] public int length;
    [HideInInspector] public int strength;
    [HideInInspector] public int offlineEarnings;

    [HideInInspector] public int lengthCost;
    [HideInInspector] public int strengthCost;
    [HideInInspector] public int offlineEarningsCost;

    [HideInInspector] public int totalGain;
    [HideInInspector] public int wallet;

    [SerializeField] private int[] costs = new int[]
    {
        120, 151, 197, 250, 324, 414, 537, 687, 892, 1145, 1484, 1911, 2479, 3196, 4148, 5359, 6954, 9000, 11687
    };

    //Maximum limit for attributes
    public int MaxLength => -(costs.Length - 3) * 10; // -160 with costs.Length = 19
    public int MaxStrength => costs.Length + 2; // 21 with costs.Length = 19
    public int MaxOfflineEarnings => costs.Length + 2; // 21 with costs.Length = 19
    public static IdleManager instance;

    void Awake()
    {
        // Singleton pattern
        if (IdleManager.instance)
        {
            Destroy(gameObject);
        }
        else
        {
            IdleManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitializePlayerPrefs();
    }

    private void InitializePlayerPrefs()
    {
        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        wallet = PlayerPrefs.GetInt("Wallet", 0);

        //Base costs
        lengthCost = costs[Mathf.Min(-length / 10 - 3, costs.Length - 1)];
        strengthCost = costs[Mathf.Min(strength - 3, costs.Length - 1)];
        offlineEarningsCost = costs[Mathf.Min(offlineEarnings - 3, costs.Length - 1)];

        Debug.Log($"Play: Length = {length}, Strength = {strength}, OfflineEarnings = {offlineEarnings}, Wallet = {wallet}");
    }

    void Update()
    {
        //Clear PlayerPrefs
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            InitializePlayerPrefs();
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            //Save time before pause
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            Debug.Log($"Pause: {now}");
        }
        else
        {
            //Calculate offline income when returning
            string dateString = PlayerPrefs.GetString("Date", string.Empty);
            if (!string.IsNullOrEmpty(dateString))
            {
                DateTime lastTime = DateTime.Parse(dateString);
                totalGain = (int)((DateTime.Now - lastTime).TotalMinutes * offlineEarnings);
                ScreensManager.instance.ChangeScreen(Screens.RETURN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLength()
    {
        if (length > MaxLength && wallet >= lengthCost)
        {
            int nextLengthIndex = -length / 10 - 2;
            if (nextLengthIndex < costs.Length)
            {
                length -= 10;
                wallet -= lengthCost;
                lengthCost = costs[Mathf.Min(-length / 10 - 3, costs.Length - 1)];
                PlayerPrefs.SetInt("Length", -length);
                PlayerPrefs.SetInt("Wallet", wallet);
                ScreensManager.instance.ChangeScreen(Screens.MAIN);
            }
        }
    }

    public void BuyStrength()
    {
        if (strength < MaxStrength && wallet >= strengthCost)
        {
            int nextStrengthIndex = strength - 2;
            if (nextStrengthIndex < costs.Length)
            {
                strength++;
                wallet -= strengthCost;
                strengthCost = costs[Mathf.Min(strength - 3, costs.Length - 1)];
                PlayerPrefs.SetInt("Strength", strength);
                PlayerPrefs.SetInt("Wallet", wallet);
                ScreensManager.instance.ChangeScreen(Screens.MAIN);
            }
        }
    }

    public void BuyOfflineEarning()
    {
        if (offlineEarnings < MaxOfflineEarnings && wallet >= offlineEarningsCost)
        {
            int nextOfflineIndex = offlineEarnings - 2;
            if (nextOfflineIndex < costs.Length)
            {
                offlineEarnings++;
                wallet -= offlineEarningsCost;
                offlineEarningsCost = costs[Mathf.Min(offlineEarnings - 3, costs.Length - 1)];
                PlayerPrefs.SetInt("Offline", offlineEarnings);
                PlayerPrefs.SetInt("Wallet", wallet);
                ScreensManager.instance.ChangeScreen(Screens.MAIN);
            }
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
