using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    public float posX { get; set; }
    public float posY { get; set; }
    public float posZ { get; set; }
    public int health { get; set; }

    public List<string> Powerups = new();

    public List<string> KeyCards = new();

    public static AutoSave Instance;
    [SerializeField] private GameEvent loadEvent;
    [SerializeField] private float saveCooldown = 10f;
    //[SerializeField] private AcidManager acidManager;

    private string _playerDataPath;
    private bool _canSave;

    private void Awake()
    {
        _playerDataPath = $"{Application.dataPath}/SaveData/PlayerSaveData.txt";

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CheckForFile();
        //Load();
        DontDestroyOnLoad(Instance);
        _canSave = true;
        StartCoroutine(CountdownCanSave());
    }

    public void Save()
    {
        if (_canSave)
        {
            _canSave = false;
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<Move>().ReportPosition();
            player.GetComponent<PlayerHealth>().ReportHealth();
            Powerups.Clear();
            foreach (var powerUp in PowerUpManager.Instance.PowerUpList)
            {
                Powerups.Add(powerUp);
            }
            File.WriteAllText($"{Application.dataPath}/SaveData/PlayerSaveData.txt", FormatSaveData());
        }
    }

    public void ResetHealth()
    {
        health = 3;
        File.WriteAllText($"{Application.dataPath}/SaveData/PlayerSaveData.txt", FormatSaveData());
        StartCoroutine(AsyncSceneLoad());
    }

    private IEnumerator AsyncSceneLoad()
    {
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Load();
    }

    private IEnumerator CountdownCanSave()
    {
        yield return new WaitForSeconds(saveCooldown);
        _canSave = true;
    }

    public void RespawnCountdownCanSave()
    {
        _canSave = false;
        StartCoroutine(CountdownCanSave());
    }

    public void Load()
    {
        _canSave = false;
        var reader = new StreamReader(_playerDataPath);
        var data = reader.ReadLine();
        int lines = 0;
        while (data != null && lines < 20)
        {
            var line = data.Split(" ");
            switch (line[0])
            {
                case "position":
                {
                    var position = line[1].Split(",");
                    posX = float.Parse(position[0]);
                    posY = float.Parse(position[1]);
                    posZ = float.Parse(position[2]);

                    break;
                }
                case "health":
                {
                    health = int.Parse(line[1]);
                    break;
                }
                case "keycard":
                {
                    var keyCards = line[1].Split(",");
                    foreach (var keyCard in keyCards)
                    {
                        if (keyCard != "")
                            KeyCards.Add(keyCard);
                    }

                    break;
                }
                case "powerups":
                {
                    var powerUps = line[1].Split(",");
                    foreach (var powerUp in powerUps)
                    {
                        if (powerUp != "")
                            PowerUpManager.Instance.PowerUpList.Add(powerUp);
                    }

                    break;
                }
                /*case "acidState":
                {
                    var acidState = line[1];
                    if (Enum.TryParse(acidState, out AcidState parsedAcidState))
                    {
                        acidManager.CurrentAcidState = parsedAcidState;
                    }

                    break;
                }*/
            }

            data = reader.ReadLine();
            lines++;
        }

        reader.Close();
        TriggerLoad();
    }

    public void TriggerLoad()
    {
        RespawnCountdownCanSave();
        loadEvent.TriggerEvent();
    }

    private string FormatSaveData()
    {
        var positionText = $"position {posX},{posY},{posZ}\n";
        var finalText = positionText;
        var healthText = $"health {health}\n";
        finalText += healthText;

        var keyCardNames = "keycard ";
        foreach (var keyCard in KeyCards)
        {
            keyCardNames += keyCard + ",";
        }

        keyCardNames += "\n";
        finalText += keyCardNames;

        var powerUpNames = "powerups ";
        foreach (var powerup in Powerups)
        {
            powerUpNames += powerup + ",";
        }

        powerUpNames += "\n";
        finalText += powerUpNames;

        /*if (acidManager != null)
        {
            var acidState = "acidState ";
            acidState += acidManager.CurrentAcidState;
            acidState += "\n";
        
            finalText += acidState;
        }*/



        return finalText;
    }
    
    public void AddKeyCard(string keyCardName)
    {
        KeyCards.Add(keyCardName);
    }

    public void CheckForFile()
    {
        if (!File.Exists(_playerDataPath))
        {
            File.WriteAllText(_playerDataPath, "");
        }
    }
}