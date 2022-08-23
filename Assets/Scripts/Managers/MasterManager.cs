using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/MasterManager")]
public class MasterManager : ScriptableObjectSingleton<MasterManager>
{
    [SerializeField]
    private PlayerGameManager _playerGameManager;
    public static PlayerGameManager PlayerGameManager { get { return Instance._playerGameManager; } }

    [SerializeField]
    private StatsManager _statsManager;
    public static StatsManager StatsManager { get { return Instance._statsManager; } set { } }

    [SerializeField]
    private UserManager _userManager;
    public static UserManager UserManager { get { return Instance._userManager; } set { } }

    [SerializeField]
    private SaveLoadManager _saveLoadManager;
    public static SaveLoadManager SaveLoadManager { get { return Instance._saveLoadManager; } set { } }

    [SerializeField]
    private GameLevelManager _gameLevelManager;
    public static GameLevelManager GameLevelManager { get { return Instance._gameLevelManager; } set { } }

    
}
