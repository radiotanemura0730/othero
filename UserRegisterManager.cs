using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UserRegistrationManager : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button registerButton;
    public Button goToDeleteSceneButton;
    public List<User> UserList = new List<User>();
    void Start()
    {
        // 登録ボタンにクリックイベントを設定
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        goToDeleteSceneButton.onClick.AddListener(LoadDeleteScene);
    }

    void OnRegisterButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("ユーザー名またはパスワードが空です。");
            // 必要に応じてエラーメッセージを表示する処理を追加
            return;
        }

        // ユーザー名とパスワードを使って登録処理を行う
        RegisterUser(username, password);
    }
    void LoadDeleteScene()
    {
        SceneManager.LoadScene("UserDeleteScene"); 
    }
    public bool RegisterUser(string username, string password)
    {
        if (usernameExists(username))
        {
            Debug.LogWarning("ユーザーネームが既に存在します。");
            return false;
        }

        User newUser = ScriptableObject.CreateInstance<User>();
        newUser.username = username;
        newUser.password = password;
        newUser.gameWins = 0;
        newUser.gamesPlayed = 0;

        AddUser(newUser);
        print(UserList);
        return true;
    }

    public bool usernameExists(string username)
    {
        foreach (User User in UserList)
        {
            if (User.username == username)
            {
                return true; /* ユーザーネームが重複している場合True */
            }
        }
        return false;
    }

    public bool AddUser(User newUser)
    {
        if (!usernameExists(newUser.username))
        {
            UserList.Add(newUser);
            return true;
        }
        return false; // ユーザーネームが重複している場合
    }
}
