using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class UserDeleteManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button deleteButton;
    public TextMeshProUGUI resultText;
    public List<User> UserList = new List<User>();
    void Start()
    {
        deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }
    void OnDeleteButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        User user = FindUserByUsername(username);
        if (user != null)
        {
            if (user.password == password)
            {
                DeleteUser(user);
                resultText.text = "User deleted successfully.";
            }
            else
            {
                resultText.text = "Incorrect password.";
            }
        }
        else
        {
            resultText.text = "User not found.";
        }
    }
    User FindUserByUsername(string username)
    {
        string[] guids = AssetDatabase.FindAssets("t:User");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            User user = AssetDatabase.LoadAssetAtPath<User>(path);
            if (user.username == username)
            {
                return user;
            }
        }
        return null;
    }
    public void DeleteUser(User user)
    {
        string path = AssetDatabase.GetAssetPath(user);

        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
        }
    }
}
