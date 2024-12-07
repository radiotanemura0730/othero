using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HandicapManager : MonoBehaviour
{
    public GameObject handicapPanel;
    public List<Button> handicapButtons;
    public Dropdown handicapTargetDropdown; 
    public Dropdown chooseStarterDropdown;
    public Button startButton;
    public static int selectedHandicap;
    public static bool isHandicapForWhite = true;
    public static bool isStarterWhite = true;

    void Start()
    {
        handicapPanel.SetActive(true);
        startButton.onClick.AddListener(StartGame);

        // ハンデボタンの初期化
        for (int i = 0; i < handicapButtons.Count; i++)
        {
            int handicapValue = i;
            handicapButtons[i].onClick.AddListener(() => OnHandicapButtonClicked(handicapValue));
            OnHandicapButtonClicked(0);
        }

        InitializeHandicapTargetDropdown();
        InitializeChooseStarterDropdown();

        OnHandicapTargetChanged(0);
        StarterChanged(0);
    }

    void InitializeHandicapTargetDropdown()
    {
        handicapTargetDropdown.options.Clear();
        handicapTargetDropdown.options.Add(new Dropdown.OptionData("white"));
        handicapTargetDropdown.options.Add(new Dropdown.OptionData("black"));

        handicapTargetDropdown.value = 0;

        handicapTargetDropdown.onValueChanged.AddListener(OnHandicapTargetChanged);
    }
    void InitializeChooseStarterDropdown()
    {
        chooseStarterDropdown.options.Clear();
        chooseStarterDropdown.options.Add(new Dropdown.OptionData("white"));
        chooseStarterDropdown.options.Add(new Dropdown.OptionData("black"));

        chooseStarterDropdown.value = 0;

        chooseStarterDropdown.onValueChanged.AddListener(StarterChanged);
    }

    void OnHandicapTargetChanged(int value)
    {
        isHandicapForWhite = value == 0; // 0: White, 1: Black
    }

    void StarterChanged(int value)
    {
        isStarterWhite = value == 0; // 0: White, 1: Black
    }

    void OnHandicapButtonClicked(int handicapValue)
    {
        selectedHandicap = handicapValue;
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}

