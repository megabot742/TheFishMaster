using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{

    public static ScreensManager instance;

    GameObject currentScreen;

    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject returnScreen;

    [SerializeField] Button lengthButton;
    [SerializeField] Button strengthButton;
    [SerializeField] Button offlineButton;


    [SerializeField] TMP_Text gameScreenMoney;
    [SerializeField] TMP_Text lengthCostText;
    [SerializeField] TMP_Text lengthValueText;
    [SerializeField] TMP_Text strengthCostText;
    [SerializeField] TMP_Text strengthValueText;
    [SerializeField] TMP_Text offlineCostText;
    [SerializeField] TMP_Text offlineValueText;
    [SerializeField] TMP_Text endScreenMoney;
    [SerializeField] TMP_Text returnScreenMoney;

    private int gameCount;

    void Awake()
    {
        if(ScreensManager.instance)
            Destroy(base.gameObject);
        else
            ScreensManager.instance = this;
        
        currentScreen = mainScreen;
    }
    void Start()
    {
        CheckIdles();
        UpdateTexts();
    }

    public void ChangeScreen(Screens screen)
    {
        currentScreen.SetActive(false);
        switch (screen)
        {
            case Screens.MAIN:
                currentScreen = mainScreen;
                UpdateTexts();
                CheckIdles();
                break;
            case Screens.GAME:
                currentScreen = gameScreen;
                gameCount++;
                break;
            case Screens.END:
                currentScreen = endScreen;
                SetEndScreenMoney();
                break;
            case Screens.RETURN:
                currentScreen = returnScreen;
                SetReturnScreenMoney();
                break;
        }
        currentScreen.SetActive(true);
    }
    public void SetEndScreenMoney()
    {
        endScreenMoney.text = "$" + IdleManager.instance.totalGain;
    }

    public void SetReturnScreenMoney()
    {
        returnScreenMoney.text = "$" + IdleManager.instance.totalGain + "  Gained";
    }

    public void UpdateTexts()
    {
        gameScreenMoney.text = "$" + IdleManager.instance.wallet;

        lengthCostText.text = "$" + IdleManager.instance.lengthCost;
        lengthValueText.text = -IdleManager.instance.length + "m";

        strengthCostText.text = "$" + IdleManager.instance.strengthCost;
        strengthValueText.text = IdleManager.instance.strength + " fishes.";

        offlineCostText.text = "$" + IdleManager.instance.offlineEarningsCost;
        offlineValueText.text = "$" + IdleManager.instance.offlineEarnings + "/main";
    }

    public void CheckIdles()
    {
        int lengthCost = IdleManager.instance.lengthCost;
        int strengthCost = IdleManager.instance.strengthCost;
        int offlineEarningsCost = IdleManager.instance.offlineEarningsCost;
        int wallet = IdleManager.instance.wallet;
        
         int length = IdleManager.instance.length;
        int strength = IdleManager.instance.strength;
        int offlineEarnings = IdleManager.instance.offlineEarnings;

        //lengthButton
        if (wallet >= lengthCost && length > IdleManager.instance.MaxLength)
        {
            lengthButton.interactable = true;
        }
        else
        {
            lengthButton.interactable = false;
        }

        //strengthButton
        if (wallet >= strengthCost && strength < IdleManager.instance.MaxStrength)
        {
            strengthButton.interactable = true;
        }
        else
        {
            strengthButton.interactable = false;
        }

        //offlineButton
        if (wallet >= offlineEarningsCost && offlineEarnings < IdleManager.instance.MaxOfflineEarnings)
        {
            offlineButton.interactable = true;
        }
        else
        {
            offlineButton.interactable = false;
        }
    }

}
