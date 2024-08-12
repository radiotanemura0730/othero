using UnityEngine;
using UnityEngine.UI;
public class UserManager : MonoBehaviour
{
    public UserDatabase userDatabase;
    public InputField usernameInput;
    public InputField passwordInput;
    public Text feedbackText;
    public Button submitButton;
    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
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
}
