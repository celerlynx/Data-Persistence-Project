using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string bestPlayerName;
    public int bestScore;
    public string currentPlayerName;
    public int currentScore;

    private string _pathToDB;
    private List<Player> _playerList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _pathToDB = Application.persistentDataPath + "/breakout_db.json";
    }

    [Serializable]
    public class Player
    {
        public string name;
        public int score;
    }

    [Serializable]
    public class PlayerListData
    {
        public List<Player> players;
        public string lastPlayer;

        public PlayerListData(List<Player> players)
        {
            this.players = players;
        }
    }

    void Start()
    {

       //Load players list data
        if (string.IsNullOrEmpty(currentPlayerName))
            LoadPlayerData();
    }

    public void LoadPlayerData()
    {
        if (File.Exists(_pathToDB))
        {
            string json = File.ReadAllText(_pathToDB);
            PlayerListData data = JsonUtility.FromJson<PlayerListData>(json);

            if (data != null)
            {
                _playerList = data?.players;
                if (_playerList != null)
                {
                    bestPlayerName = _playerList[0].name;
                    bestScore = _playerList[0].score;
                    if (!string.IsNullOrEmpty(data.lastPlayer))
                    {
                        currentPlayerName = data.lastPlayer;
                    }

                }

            }
            
        }
    }

    public void SetCurrentPlayer()
    {
        if (_playerList != null && !string.IsNullOrEmpty(currentPlayerName))
        {
            Player foundCurPlayer = _playerList.FirstOrDefault(p => p.name == currentPlayerName);
            currentScore = (foundCurPlayer != null)? foundCurPlayer.score : 0;
        }
        if (!string.IsNullOrEmpty(currentPlayerName))
            SavePlayerData();

    }

    public void UpdateCurrentPlayer()
    {
        if (!string.IsNullOrEmpty(currentPlayerName))
        {
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestPlayerName = currentPlayerName;
            }

            SavePlayerData();
        }
    }

    private void SavePlayerData()
    {
        if (_playerList == null)
        {
            //Create new Player List
            _playerList = new List<Player>
            {
                new Player { name = currentPlayerName, score = currentScore }
            };
        }
        else
        {
            //Existing Player List
            int foundId = _playerList.FindIndex(p => p.name == currentPlayerName);
            if (foundId == -1)
            {
                //Add new player
                _playerList.Add(new Player { name = currentPlayerName, score = currentScore });
            }
            else
            {
                //Update existing player 
                _playerList[foundId].score = currentScore;
            }
        }

        //Sort player list by score (max to min)
        if (_playerList.Count > 1)
        {
            _playerList = _playerList.OrderByDescending(pl => pl.score)
                                     .ThenBy(pl => pl.name)
                                     .ToList();
        }
        PlayerListData data = new PlayerListData(_playerList);
        data.lastPlayer = currentPlayerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_pathToDB, json);

        Debug.Log(_pathToDB + "  " + json);
    }


}
