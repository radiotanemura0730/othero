using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    public Button selectUserButton;
    public Button registerUserButton;

    void Start()
    {
        selectUserButton.onClick.AddListener(OnSelectUserClicked);
        registerUserButton.onClick.AddListener(OnRegisterUserClicked);
    }

    void OnSelectUserClicked()
    {
        // ユーザー選択の処理をここに記述
        Debug.Log("ユーザー選択ボタンがクリックされました。");
        // 例えば、ユーザーリストを表示するなどの処理
    }

    void OnRegisterUserClicked()
    {
        // 新しいユーザーを登録する処理をここに記述
        Debug.Log("新しくユーザーを登録するボタンがクリックされました。");
        // 例えば、新しいユーザー名を入力するUIを表示するなどの処理
    }
}
