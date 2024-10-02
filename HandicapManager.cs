using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HandicapManager : MonoBehaviour
{
    public GameObject handicapPanel; 
    public List<Button> handicapButtons;
    public Button startButton;
    public static int selectedHandicap;
    void Start()
    {
        handicapPanel.SetActive(true);
        startButton.onClick.AddListener(StartGame);

        for (int i = 0; i < handicapButtons.Count; i++)
        {
            int handicapValue = i; 
            handicapButtons[i].onClick.AddListener(() => OnHandicapButtonClicked(handicapValue));
        }
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
