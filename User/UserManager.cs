using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
public class UserManager : MonoBehaviour
{
    public UserDatabase userDatabase;
    public InputField usernameInput;
    public InputField passwordInput;
    public TextMeshProUGUI feedbackText;
    public Button submitButton;
    public Button loginButton;
    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        feedbackText.text = "";
    }
    public void OnSubmitButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Username and Password cannot be empty.";
            return;
        }

        if (UserExists(username))
        {
            feedbackText.text = "User already exists.";
            return;
        }

        AddUser(username, password);
        feedbackText.text = "User added successfully!";
    }
    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Username and Password cannot be empty.";
            return;
        }

        User loggedInUser = userDatabase.users.FirstOrDefault(user => user.username == username && user.password == password);

        if (loggedInUser != null)
        {
            LoginManager.Instance.SetLoggedInUser(loggedInUser);
            SceneManager.LoadScene("MainMenuScene");
        }
        else
        {
            feedbackText.text = "Invalid username or password.";
        }
    }

    private bool UserExists(string username)
    {
        foreach (var user in userDatabase.users)
        {
            if (user.username == username)
            {
                return true;
            }
        }
        return false;
    }

    private void AddUser(string username, string password)
    {
        User newUser = new User();
        newUser.username = username;
        newUser.password = password;

        userDatabase.users.Add(newUser);
    }
    private bool ValidateUser(string username, string password)
    {
        return userDatabase.users.Any(user => user.username == username && user.password == password);
    }
}
