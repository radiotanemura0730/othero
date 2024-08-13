using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UserDatabase", menuName = "Database/UserDatabase")]
public class UserDatabase : ScriptableObject
{
    public List<User> users = new List<User>();
}
