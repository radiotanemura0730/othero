using UnityEngine;
 
[CreateAssetMenu]
[SerializeField]
public class User : ScriptableObject
{
    public string username;
    public string password;
    public int gamesPlayed;
    public int gameWins;
}