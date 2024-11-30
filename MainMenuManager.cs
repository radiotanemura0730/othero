using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Todo:対人戦でCPUが動いてしまうバグの修正

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI loginDisplayText;
    public Button startCPUBattleButton;

    void Start()
    {
        startButton.onClick.AddListener(StartVsPlayerGame);
        startCPUBattleButton.onClick.AddListener(StartVsCPUGame);
    }

    void StartVsPlayerGame()
    {
        GameModeManager.Instance.SetMode(false);
        SceneManager.LoadScene("HandicapScene");
    }
    void StartVsCPUGame()
    {
        GameModeManager.Instance.SetMode(true);
        SceneManager.LoadScene("HandicapScene");
    }
}

