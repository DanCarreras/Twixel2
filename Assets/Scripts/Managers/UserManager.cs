using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Managers/UserManager")]
[SerializeField]
public class UserManager : ScriptableObject
{
    public GameObject p_Background;
    public GameObject p_Head;
    public GameObject p_Body;
    public GameObject p_Frame;
    public GameObject p_Accessory;
    public string p_NickName;
    public string p_email;
    public string username;
    public string password;
    public string displayName;
    public bool seenTutorial;
    public int TotalXp;
    public int Level;
    public int LevelXP;
    public int NextLevelXP;
    public bool Levelledup;
    public bool WatchAd;
    public int BoxesEarned;
    public int TotalCoins;

    public void AddXP(int xpEarned)
    {
        TotalXp += xpEarned;
        AddToLevelXP(xpEarned);
    }

    public void AddToLevelXP(int xpEarned)
    {
        LevelXP += xpEarned;
        CheckLevelUp();
    }

    public void CheckLevelUp()
    {
        if(LevelXP >= NextLevelXP)
        {
            // Level up the player
            Level += 1;
            // Play Level up animation

            // Subtract Current next levelxp from level xp, so that whatever's leftover is for the next level
            LevelXP -= NextLevelXP;

            //Get the new amount of xp required
            NextLevelXP = NextLevelXPRequired(Level);
            BoxesEarned += 1;
            CheckLevelUp();
        }
        else
        {
            // No need to do anything as the player hasn't levelled up
        }
    }

    public int NextLevelXPRequired(int currentLevel)
    {
        int xpRequired = Mathf.RoundToInt((4 * (currentLevel ^ 3)) / 5);
        Debug.Log("player needs: " + xpRequired + " to level up");
        return xpRequired;
    }

    public int GiveCoins(int amount)
    {
        TotalCoins += amount;
        return amount;
    }

    public void SubtractBox()
    {
        BoxesEarned -= 1;
        MasterManager.SaveLoadManager.SaveAll();
    }
}