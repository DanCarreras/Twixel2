using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(menuName = "Managers/SaveLoadManager")]
public class SaveLoadManager : ScriptableObject
{
    public string StatsJson, UserJson, ItemJson, LevelJson, MainLevelJson;
    private string StatsFile, UserFile, ItemsFile, LevelFile;
    static readonly string JSON_ENCRYPTED_KEY = "#kJ83DAlowjkf39(#($%0_+[]:#dDA'a";
    public GameLevelManager NewGameLevelTemp;

    private void OnEnable() 
    {
        NewGameLevelTemp = new GameLevelManager();
    }
    public void Awake()
    {
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/" + "saveFiles"); if(!dirInf.Exists){ Debug.Log ("Creating subdirectory"); dirInf.Create(); 
        }
        Debug.Log(Application.persistentDataPath);
        StatsFile = dirInf + "/Stats_Save.json";
        UserFile = dirInf + "/User_Save.json";
        ItemsFile = dirInf + "/Item_Save.json";
        LevelFile = dirInf + "/Level_Save.json";
    }
    
    public void SaveAll()
    {
        SaveStats();
        SaveUser();
        SaveLevel();
    }

    //Save as json
    public void SaveStats()
    {
        // Save Stats Manager
        StatsJson = JsonUtility.ToJson(MasterManager.StatsManager);
        Rijndael crypto = new Rijndael();
        byte[] StatSoup = crypto.Encrypt(StatsJson, JSON_ENCRYPTED_KEY);
        File.WriteAllBytes(StatsFile, StatSoup);
    }

    public void SaveUser()
    {
        // Save User Manager
        UserJson = JsonUtility.ToJson(MasterManager.UserManager);
        Rijndael crypto = new Rijndael();
        byte[] UserSoup = crypto.Encrypt(UserJson, JSON_ENCRYPTED_KEY);
        File.WriteAllBytes(UserFile, UserSoup);
    }
    public void SaveLevel()
    {
        // Save User Manager
        LevelJson = JsonUtility.ToJson(MasterManager.GameLevelManager);
        Rijndael crypto = new Rijndael();
        byte[] LevelSoup = crypto.Encrypt(LevelJson, JSON_ENCRYPTED_KEY);
        File.WriteAllBytes(LevelFile, LevelSoup);
    }

    [ContextMenu("ConvertToMainJsonFile")]  
    public void SaveMainLevelJson()
    {
        string MainLevelFile = Application.dataPath + "/Json/Levels.json";
        Debug.Log("MainLevelJson is: " + MainLevelFile);
        // Save User Manager
        LevelJson = JsonUtility.ToJson(MasterManager.GameLevelManager);
        Rijndael crypto = new Rijndael();
        byte[] LevelSoup = crypto.Encrypt(LevelJson, JSON_ENCRYPTED_KEY);
        File.WriteAllBytes(MainLevelFile, LevelSoup);
    }

    // load json
    public void Load()
    {
        //Load Stats File back in
        Rijndael StatCrypto = new Rijndael();
        byte[] StatSoupBackIn = File.ReadAllBytes(StatsFile);
        StatsJson = StatCrypto.Decrypt(StatSoupBackIn, JSON_ENCRYPTED_KEY);
        JsonUtility.FromJsonOverwrite(StatsJson, MasterManager.StatsManager);

        //Load User File back in
        Rijndael UserCrypto = new Rijndael();
        byte[] UserSoupBackIn = File.ReadAllBytes(UserFile);
        UserJson = UserCrypto.Decrypt(UserSoupBackIn, JSON_ENCRYPTED_KEY);
        JsonUtility.FromJsonOverwrite(UserJson, MasterManager.UserManager);

        //Load Level File back in
        Rijndael LevelCrypto = new Rijndael();
        byte[] LevelSoupBackIn = File.ReadAllBytes(LevelFile);
        LevelJson = LevelCrypto.Decrypt(LevelSoupBackIn, JSON_ENCRYPTED_KEY);
        JsonUtility.FromJsonOverwrite(LevelJson, MasterManager.GameLevelManager);
    }

    // Load main json into temporary list
    public void LoadLevelFromMainJson()
    {
        string MainLevelFile = Application.dataPath + "/json/Levels.json";
        //Load Level File back in
        Rijndael LevelCrypto = new Rijndael();
        byte[] MainLevelSoupBackIn = File.ReadAllBytes(MainLevelFile);
        MainLevelJson = LevelCrypto.Decrypt(MainLevelSoupBackIn, JSON_ENCRYPTED_KEY);
        JsonUtility.FromJsonOverwrite(MainLevelJson, NewGameLevelTemp);
    }

    // For each entry in temporary list, check if level already exists - if so, don't add to GameLevelManager
    // Otherwise, add to GameLevelManager using AddLevel()
    public void UpdateMainLevelList()
    {
        foreach(var Level in NewGameLevelTemp.Levels)
        {
            if(MasterManager.GameLevelManager.Levels.Exists(x => x.LevelName == Level.LevelName))
            {
                Debug.Log("No need to add this level as it already exists");
            }
            else
            {
                Debug.Log("Need to add the following level: " + Level.LevelName);
                MasterManager.GameLevelManager.AddLevel(Level);
            }
        }
    }

    public void ResetStats()
    {
        StatsManager NewStats = new StatsManager();
        MasterManager.StatsManager = NewStats;
        UserManager NewUser = new UserManager();
        MasterManager.UserManager = NewUser;
        GameLevelManager NewGameLevel = new GameLevelManager();
        MasterManager.GameLevelManager = NewGameLevel;
        // Save newly created GameManagers
        SaveAll();
        // Import Level Data
        LoadLevelFromMainJson();
    }
}
