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

        if(wallet < lengthCost)
            lengthButton.interactable = false;
        else
            lengthButton.interactable = true;

        if(wallet < strengthCost)
            strengthButton.interactable = false;
        else
            strengthButton.interactable = true;

        if(wallet < offlineEarningsCost)
            offlineButton.interactable = false;
        else
            offlineButton.interactable = true;
    }

}
