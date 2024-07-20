using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    public Button restartButton;
    public Button backToMainMenuButton;
    public PieceManager pieceManager;
    public Canvas gameOverCanvas;
    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        backToMainMenuButton.onClick.AddListener(GoBackToMainMenu);
        gameOverCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (pieceManager.isGameEnds)
        {
            gameOverCanvas.gameObject.SetActive(true);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

