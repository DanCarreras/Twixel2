using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Worth renaming this to "player gamestate manager"
[CreateAssetMenu(menuName = "Managers/PlayerGameManager")]
public class PlayerGameManager : ScriptableObject
{
    //This class/singleton should track whether the player in question has powerups, what state they're in, and scores
    public float LevelTime;
    public int CurrentLevel;
    public bool PlayedBefore;
    public bool GreenKey, BlueKey, RedKey;

    public void LevelFinished()
    {
        MasterManager.GameLevelManager.AddPlayerTime(CurrentLevel.ToString(), LevelTime);
        ResetKeys();
    }

    public void ResetKeys()
    {
        GreenKey = false;
        BlueKey = false;
        RedKey = false;

        var _collectibles = GameObject.Find("Collectibles");
    
        if(_collectibles)
        {
            _collectibles.SendMessage("ResetKeys");
        }
    }

    public void SetKey(string _keyColour)
    {
        var _collectibles = GameObject.Find("Collectibles");
        switch (_keyColour)
        {
            case "Green":
            GreenKey = true;
            break;

            case "Blue":
            BlueKey = true;
            break;

            case "Red":
            RedKey = true;
            break;

            default:
            Debug.Log("You didn't give me the name of a key I could activate");
            break;
        }
        _collectibles.SendMessage("ActivateKey", _keyColour);
    }
}
