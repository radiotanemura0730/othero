using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button logoutButton;
    public TextMeshProUGUI loginDisplayText;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
        loginDisplayText.text = "Welcome, " + LoginManager.Instance.LoggedInUser.username;
    }

    void StartGame()
    {
        SceneManager.LoadScene("HandicapScene");
    }
    private void OnLogoutButtonClicked()
    {
        // ログアウト処理: ログイン情報をクリアする
        LoginManager.Instance.SetLoggedInUser(null);

        // ログイン画面に戻る
        SceneManager.LoadScene("UserSignupScene"); 
    }
}

