using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button logoutButton;
    public TextMeshProUGUI loginDisplayText;
    public Button startCPUBattleButton;
    public static bool isVsCPU;

    void Start()
    {
        startButton.onClick.AddListener(StartVsPlayerGame);
        startCPUBattleButton.onClick.AddListener(StartVsCPUGame);
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
        loginDisplayText.text = "Welcome, " + LoginManager.Instance.LoggedInUser.username;
    }

    void StartVsPlayerGame()
    {
        SceneManager.LoadScene("HandicapScene");
        isVsCPU = false;
    }
    void StartVsCPUGame()
    {
        SceneManager.LoadScene("HandicapScene");
        isVsCPU = true;
    }
    private void OnLogoutButtonClicked()
    {
        // ログアウト処理: ログイン情報をクリアする
        LoginManager.Instance.SetLoggedInUser(null);

        // ログイン画面に戻る
        SceneManager.LoadScene("UserSignupScene"); 
    }
}

