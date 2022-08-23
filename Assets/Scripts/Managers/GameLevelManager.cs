using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Managers/LevelManager")]
[Serializable]
public class GameLevelManager : ScriptableObject
{
    public int NoOfLevels;
    // Array of Levels
    public List<Level> Levels = new List<Level>();

    public void AddNewLevel(string LevelName)
    {
        Level _level = new Level();
        _level.LevelName = LevelName;
        _level.Unlocked = false;
        _level.TimeToBeat = 0;
        _level.UsersTime = 0;
        _level.CollectibleFound = false;
        _level.Finished = false;
        _level.AmountOfStarsEarned = 0;
        Levels.Add(_level);
    }

    public void AddLevel(Level _level)
    {
        Levels.Add(_level);
    }

    public bool CheckAllLevelsFinished()
    {
        var finished = 0;
        bool AllFinished = false;
        for(int i = 1; i <= Levels.Count; i++)
        {
            var currentLevel = MasterManager.GameLevelManager.Levels.Find(x => x.LevelName == i.ToString());
            Debug.Log(currentLevel.LevelName);
            if(currentLevel.CollectibleFound)
            {
                //Debug.Log("No need to add this level as it already exists");
                finished += 1;
            }
        }
        
        if(finished == Levels.Count)
        {
            AllFinished = true;
        }

        return AllFinished;
    }

    public bool CheckAllCollectiblesFound()
    {
        bool CollectiblesFound = false;
        int collectibles_found = GetAmountOfCollectibles();
        
        if(collectibles_found == Levels.Count)
        {
            CollectiblesFound = true;
        }

        return CollectiblesFound;
    }

    public void AddPlayerTime(string _thisLevel, float time)
    {
        var currentLevel = MasterManager.GameLevelManager.Levels.Find(x => x.LevelName == _thisLevel.ToString());
        var UsersCurrentTime = currentLevel.UsersTime;

        if(time == 0)
        {
            currentLevel.UsersTime = time;
        }
        else if(time <= UsersCurrentTime)
        {
            currentLevel.UsersTime = time;
        }

        currentLevel.Finished = true;

        CalculateStars(_thisLevel);
    }

    public void CalculateStars(string _thisLevel)
    {
        // We need to divide the time I've put in the json file in 3, so that a user can get one star for just finishing the level
        // one star for doing it in 75% of the time, and finally 90% of the time
        // so, 3 stars is below the amount, 2 stars is within 25% of the required time, 1 star is just doing the level
        float _timeFinished = MasterManager.PlayerGameManager.LevelTime;
        //int _currentLevel = int.Parse(SceneManager.GetActiveScene().name);
        var currentLevel = MasterManager.GameLevelManager.Levels.Find(x => x.LevelName == _thisLevel.ToString());
        float timeToBeat = currentLevel.TimeToBeat;
        float timeToBeatTwoStar = timeToBeat * 1.25f;
        Debug.Log("Two star time is: " + timeToBeatTwoStar);
        int starsEarned = 0;

        if(_timeFinished <= timeToBeat)
        {
            starsEarned = 3;
        }
        else if (_timeFinished <= timeToBeatTwoStar)
        {
            starsEarned = 2;
        }
        else
        {
            starsEarned = 1;
        }

        currentLevel.AmountOfStarsEarned = starsEarned;

        UnlockNextLevel(_thisLevel);
    }

    public void GotCollectible()
    {
        var currentLevel = GetCurrentLevel();
        currentLevel.CollectibleFound = true;
    }

    public void UnlockNextLevel(string _thisLevel)
    {
        var currentLevel = GetCurrentLevel();
        int _nextLevelInt = Int32.Parse(currentLevel.LevelName) + 1;
        var _nextLevel = MasterManager.GameLevelManager.Levels.Find(x => x.LevelName == _nextLevelInt.ToString());

        if(!_nextLevel.Unlocked)
        {
            _nextLevel.Unlocked = true;
        }
    }

    public int GetAmountOfCollectibles()
    {
        var collectibles_unlocked = 0;
        for(int i = 1; i <= Levels.Count; i++)
        {
            var currentLevel = MasterManager.GameLevelManager.Levels.Find(x => x.LevelName == i.ToString());
            Debug.Log(currentLevel.LevelName);
            if(currentLevel.CollectibleFound)
            {
                //Debug.Log("No need to add this level as it already exists");
                collectibles_unlocked += 1;
            }
            else
            {
                //Debug.Log("Need to add the following level: " + i);
                //MasterManager.GameLevelManager.AddNewLevel(i.ToString());
            }
        }
        Debug.Log("Collectibles found: " + collectibles_unlocked);
        return collectibles_unlocked;
    }

    public Level GetCurrentLevel()
    {
        string _thisLevel = SceneManager.GetActiveScene().name;
        var currentLevel = MasterManager.GameLevelManager.Levels.Find(x => x.LevelName == _thisLevel);
        return currentLevel;
    }

    [ContextMenu("PushLevelsToMainJsonFile")]  
    public void SaveLevelstoMainJson()
    {
        MasterManager.SaveLoadManager.SaveMainLevelJson();
    }

    [ContextMenu("LoadLevelsFromMainJsonFile")]  
    public void LoadLevelsFromMainJson()
    {
        MasterManager.SaveLoadManager.LoadLevelFromMainJson(); 
        MasterManager.SaveLoadManager.UpdateMainLevelList();  
    }

    [ContextMenu("InitialiseLevels")]
    public void InitialiseLevels()
    {
        for(int i = 0; i <= NoOfLevels; i++)
        {
            Debug.Log("Checking the following level: " + i);
            if(MasterManager.GameLevelManager.Levels.Exists(x => x.LevelName == i.ToString()))
            {
                Debug.Log("No need to add this level as it already exists");
            }
            else
            {
                Debug.Log("Need to add the following level: " + i);
                MasterManager.GameLevelManager.AddNewLevel(i.ToString());
            }
        }

        // Need to add this to ensure the player can at least play the first level
        Levels[0].Unlocked = true;
    }

    public int GetStars( )
    {
        var currentLevel = GetCurrentLevel();
        return currentLevel.AmountOfStarsEarned;
    }
}

[Serializable]
public class Level
{
    // Tracks the Level name
    public string LevelName;
    // Tracks whether the player has unlocked the Level
    public bool Unlocked;
    public float TimeToBeat;
    public float UsersTime;
    public bool CollectibleFound;

    public bool Finished;
    public int AmountOfStarsEarned;
}
